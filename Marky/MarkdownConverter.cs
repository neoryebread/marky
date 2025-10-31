using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Marky.Rules;

namespace Marky;

/// <summary>
/// Orchestrates the conversion of a full Markdown document to HTML.
/// This converter is stateful and processes a document line by line.
/// </summary>
public class MarkdownConverter
{
    private readonly List<IParseRule> _blockRules;
    private readonly List<IParseRule> _inlineRules;

    private enum ListType { Unordered, Ordered }
    private readonly Stack<(ListType Type, int Indentation)> _listStateStack = new();
    private readonly List<string> _paragraphBuffer = new();

    /// <summary>
    /// Initializes a new instance of the <see cref="MarkdownConverter"/> class,
    /// pre-populating the rule sets.
    /// </summary>
    public MarkdownConverter()
    {
        _blockRules = new List<IParseRule>
        {
            new HorizontalRule(),
            new HeaderRule(),
            new BlockquoteRule(),
        };

        _inlineRules = new List<IParseRule>
        {
            new ImageRule(), 
            new LinkRule(), 
            new BoldItalicRule(),
            new BoldRule(), 
            new ItalicRule(), 
            new InlineCodeRule()
        };
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

        foreach (var line in lines)
        {
            ProcessLine(line, htmlBody);
        }

        FlushParagraphBuffer(htmlBody);
        CloseAllLists(htmlBody);

        return string.Join("\n", htmlBody).Trim();
    }

    /// <summary>
    /// Processes a single line of Markdown, managing state and applying rules.
    /// </summary>
    private void ProcessLine(string line, List<string> htmlBody)
    {
        if (string.IsNullOrWhiteSpace(line))
        {
            FlushParagraphBuffer(htmlBody);
            CloseAllLists(htmlBody);
            return;
        }

        if (HandleListLogic(line, htmlBody))
        {
            return;
        }

        // If we were in a list and now we are not, close all list tags first.
        CloseAllLists(htmlBody);

        bool blockRuleApplied = false;
        foreach (var rule in _blockRules)
        {
            var result = rule.Apply(line);
            if (result != line)
            {
                // A block rule is about to be applied, so flush any pending paragraph.
                FlushParagraphBuffer(htmlBody);
                htmlBody.Add(ApplyInlineRules(result));
                blockRuleApplied = true;
                break;
            }
        }

        if (!blockRuleApplied)
        {
            _paragraphBuffer.Add(line);
        }
    }

    /// <summary>
    /// Manages the state of nested lists, opening and closing list tags as needed.
    /// </summary>
    /// <returns>True if the line was handled as a list item, false otherwise.</returns>
    private bool HandleListLogic(string line, List<string> htmlBody)
    {
        bool isUnordered = UnorderedListRule.IsUnorderedList(line);
        bool isOrdered = OrderedListRule.IsOrderedList(line);

        if (!isUnordered && !isOrdered) return false;

        FlushParagraphBuffer(htmlBody);

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

        return true;
    }

    /// <summary>
    /// Closes all currently open list tags.
    /// </summary>
    private void CloseAllLists(List<string> htmlBody)
    {
        while (_listStateStack.Any())
        {
            htmlBody.Add(GetClosingTag(_listStateStack.Pop().Type));
        }
    }

    /// <summary>
    /// Processes and clears the paragraph buffer, adding a new paragraph to the HTML body.
    /// </summary>
    private void FlushParagraphBuffer(List<string> htmlBody)
    {
        if (_paragraphBuffer.Any())
        {
            var fullParagraph = string.Join(" ", _paragraphBuffer);
            htmlBody.Add($"<p>{ApplyInlineRulesToContent(fullParagraph)}</p>");
            _paragraphBuffer.Clear();
        }
    }

    private string GetOpeningTag(ListType type) => type == ListType.Ordered ? "<ol>" : "<ul>";
    private string GetClosingTag(ListType type) => type == ListType.Ordered ? "</ol>" : "</ul>";

    /// <summary>
    /// Applies inline rules to the content within an HTML tag.
    /// </summary>
    private string ApplyInlineRules(string text)
    {
        // This regex is more robust. It finds content between > and <.
        return Regex.Replace(text, @">([^<]*?)<", m => $">{ApplyInlineRulesToContent(m.Groups[1].Value)}<");
    }

    /// <summary>
    /// Applies all registered inline rules to a string of content.
    /// </summary>
    private string ApplyInlineRulesToContent(string content)
    {
        foreach (var rule in _inlineRules)
        {
            content = rule.Apply(content);
        }
        return content;
    }
}
