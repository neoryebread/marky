using System.Text.RegularExpressions;
using Marky.Rules;
using Marky.States;

namespace Marky;

/// <summary>
/// Orchestrates the conversion of a full Markdown document to HTML.
/// This converter is stateful and processes a document line by line.
/// </summary>
public class MarkdownConverter
{
    private readonly List<IParseRule> _blockRules;
    private readonly List<IParseRule> _inlineRules;
    private readonly Dictionary<BlockState, IState> _states;

    private readonly Stack<(ListType Type, int Indentation)> _listStateStack = new();
    private readonly List<string> _paragraphBuffer = new();
    private IState _currentState;
    
    internal string? CodeBlockLanguage { get; set; }

    /// <summary>
    /// Initializes a new instance of the <see cref="MarkdownConverter"/> class,
    /// pre-populating the rule sets and states.
    /// </summary>
    public MarkdownConverter()
    {
        _blockRules = new List<IParseRule> { new HorizontalRule(), new HeaderRule(), new FencedCodeBlockRule() };
        _inlineRules = new List<IParseRule>
        {
            new EscapeRule(), // Process escaped characters first
            new ImageRule(), new LinkRule(), new BoldItalicRule(),
            new BoldRule(), new ItalicRule(), new InlineCodeRule()
        };
        
        _states = new Dictionary<BlockState, IState>
        {
            { BlockState.None, new NoneState() },
            { BlockState.InList, new InListState() },
            { BlockState.InBlockquote, new InBlockquoteState() },
            { BlockState.InFencedCodeBlock, new InFencedCodeBlockState() }
        };
        _currentState = _states[BlockState.None];
    }

    /// <summary>
    /// Converts a multi-line string of Markdown into a complete HTML document body.
    /// </summary>
    /// <param name="markdownText">The complete Markdown text.</param>
    /// <returns>The converted HTML body content.</returns>
    public string Convert(string markdownText)
    {
        var htmlBody = new List<string>();
        var lines = markdownText.Split('\n').Select(l => l.TrimEnd()).ToArray();
        
        _listStateStack.Clear();
        _paragraphBuffer.Clear();
        _currentState = _states[BlockState.None];
        CodeBlockLanguage = string.Empty;

        foreach (var line in lines)
        {
            ProcessLine(line, htmlBody);
        }

        _currentState.ExitState(this, htmlBody);

        return string.Join("\n", htmlBody).Trim();
    }

    /// <summary>
    /// Processes a single line of Markdown, managing state and applying rules.
    /// </summary>
    internal void ProcessLine(string line, List<string> htmlBody)
    {
        _currentState.ProcessLine(this, line, htmlBody);
    }

    internal void TransitionToState(BlockState newState, string line, List<string> htmlBody)
    {
        _currentState.ExitState(this, htmlBody);
        _currentState = _states[newState];
        _currentState.EnterState(this, line, htmlBody);
    }

    internal BlockState GetStateForLine(string line)
    {
        if (FencedCodeBlockRule.IsFencedCodeBlock(line)) return BlockState.InFencedCodeBlock;
        if (BlockquoteRule.IsBlockquote(line)) return BlockState.InBlockquote;
        if (UnorderedListRule.IsUnorderedList(line) || OrderedListRule.IsOrderedList(line)) return BlockState.InList;
        return BlockState.None;
    }
    
    internal void HandleFencedCodeBlockLogic(string line, List<string> htmlBody)
    {
        if (htmlBody.Any())
        {
            if (htmlBody.Last().EndsWith(">"))
            {
                htmlBody[htmlBody.Count - 1] += System.Security.SecurityElement.Escape(line);
            }
            else
            {
                htmlBody[htmlBody.Count - 1] += "\n" + System.Security.SecurityElement.Escape(line);
            }
        }
    }
    
    internal void HandleListLogic(string line, List<string> htmlBody)
    {
        bool isUnordered = UnorderedListRule.IsUnorderedList(line);
        var type = isUnordered ? ListType.Unordered : ListType.Ordered;
        var indentation = isUnordered ? UnorderedListRule.GetIndentation(line) : OrderedListRule.GetIndentation(line);
        
        while (_listStateStack.Any() && indentation < _listStateStack.Peek().Indentation)
        {
            htmlBody.Add(GetClosingTag(_listStateStack.Pop().Type));
        }

        if (!_listStateStack.Any() || indentation > _listStateStack.Peek().Indentation)
        {
            _listStateStack.Push((type, indentation));
            htmlBody.Add(GetOpeningTag(type));
        }
        else if (indentation == _listStateStack.Peek().Indentation && type != _listStateStack.Peek().Type)
        {
            htmlBody.Add(GetClosingTag(_listStateStack.Pop().Type));
            _listStateStack.Push((type, indentation));
            htmlBody.Add(GetOpeningTag(type));
        }

        var rule = isUnordered ? (IParseRule)new UnorderedListRule() : new OrderedListRule();
        htmlBody.Add(ApplyInlineRules(rule.Apply(line)));
    }

    internal void HandleDefaultLogic(string line, List<string> htmlBody)
    {
        if (FencedCodeBlockRule.IsFencedCodeBlock(line)) return;
        
        foreach (var rule in _blockRules)
        {
            var result = rule.Apply(line);
            if (result != line)
            {
                FlushParagraphBuffer(htmlBody);
                htmlBody.Add(ApplyInlineRules(result));
                return;
            }
        }
        _paragraphBuffer.Add(line);
    }

    /// <summary>
    /// Closes all currently open list tags.
    /// </summary>
    internal void CloseAllLists(List<string> htmlBody)
    {
        while (_listStateStack.Any())
        {
            htmlBody.Add(GetClosingTag(_listStateStack.Pop().Type));
        }
    }

    /// <summary>
    /// Processes and clears the paragraph buffer, adding a new paragraph to the HTML body.
    /// </summary>
    internal void FlushParagraphBuffer(List<string> htmlBody)
    {
        if (!_paragraphBuffer.Any()) return;
        var fullParagraph = string.Join(" ", _paragraphBuffer);
        htmlBody.Add($"<p>{ApplyInlineRulesToContent(fullParagraph)}</p>");
        _paragraphBuffer.Clear();
    }

    private string GetOpeningTag(ListType type) => type == ListType.Ordered ? "<ol>" : "<ul>";
    private string GetClosingTag(ListType type) => type == ListType.Ordered ? "</ol>" : "</ul>";

    /// <summary>
    /// Applies inline rules to the content within an HTML tag.
    /// </summary>
    private string ApplyInlineRules(string text)
    {
        return Regex.Replace(text, @">([^<]*)<", m => $">{ApplyInlineRulesToContent(m.Groups[1].Value)}<");
    }

    /// <summary>
    /// Applies all registered inline rules to a string of content.
    /// </summary>
    internal string ApplyInlineRulesToContent(string content)
    {
        return _inlineRules.Aggregate(content, (current, rule) => rule.Apply(current));
    }
}