using System.Collections.Generic;
using System.Linq;
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

    private enum BlockState { None, InUnorderedList, InBlockquote }
    private BlockState _currentState = BlockState.None;
    private BlockState _previousState = BlockState.None;

    /// <summary>
    /// Initializes a new instance of the <see cref="MarkdownConverter"/> class.
    /// </summary>
    public MarkdownConverter()
    {
        // The order of block rules can be important.
        _blockRules = new List<IParseRule>
        {
            new HorizontalRule(),
            new HeaderRule(),
            new UnorderedListRule(),
            new BlockquoteRule(),
        };

        // The order of inline rules is critical to handle nesting.
        _inlineRules = new List<IParseRule>
        {
            new ImageRule(),
            new LinkRule(),
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
        _currentState = BlockState.None;
        _previousState = BlockState.None;

        foreach (var line in lines)
        {
            ProcessLine(line, htmlBody);
        }

        // Close any open blocks at the end of the document
        if (_currentState != BlockState.None)
        {
            htmlBody.Add(GetClosingTagForState(_currentState));
        }

        return string.Join("\n", htmlBody);
    }

    private void ProcessLine(string line, List<string> htmlBody)
    {
        if (string.IsNullOrWhiteSpace(line))
        {
            // If we were in a block, close it
            if (_currentState != BlockState.None)
            {
                htmlBody.Add(GetClosingTagForState(_currentState));
                _currentState = BlockState.None;
            }
            return;
        }

        _previousState = _currentState;
        UpdateState(line);

        // Close block if state changes from list/quote to none
        if (_previousState != BlockState.None && _currentState == BlockState.None)
        {
            htmlBody.Add(GetClosingTagForState(_previousState));
        }
        
        // Open block if state changes from none to list/quote
        if (_previousState == BlockState.None && _currentState != BlockState.None)
        {
            htmlBody.Add(GetOpeningTagForState(_currentState));
        }

        // --- Rule Application ---
        string processedLine = line;
        bool blockRuleApplied = false;

        foreach (var rule in _blockRules)
        {
            var result = rule.Apply(line);
            if (result != line)
            {
                // Find the content within the first tag, e.g., <h1>content</h1> -> content
                var firstTagEnd = result.IndexOf('>') + 1;
                var lastTagStart = result.LastIndexOf('<');
                
                if (lastTagStart > firstTagEnd)
                {
                    var tagContent = result.Substring(firstTagEnd, lastTagStart - firstTagEnd);
                    var processedContent = ApplyInlineRules(tagContent);
                    processedLine = result.Substring(0, firstTagEnd) + processedContent + result.Substring(lastTagStart);
                }
                else // Handle self-closing tags like <hr>
                {
                    processedLine = result;
                }
                
                blockRuleApplied = true;
                break;
            }
        }

        if (!blockRuleApplied)
        {
            processedLine = $"<p>{ApplyInlineRules(line)}</p>";
        }

        htmlBody.Add(processedLine);
    }

    private void UpdateState(string currentLine)
    {
        if (currentLine.StartsWith("* ")) _currentState = BlockState.InUnorderedList;
        else if (currentLine.StartsWith("> ")) _currentState = BlockState.InBlockquote;
        else _currentState = BlockState.None;
    }
    
    private string GetOpeningTagForState(BlockState state)
    {
        return state switch
        {
            BlockState.InUnorderedList => "<ul>",
            BlockState.InBlockquote => "<blockquote>",
            _ => ""
        };
    }

    private string GetClosingTagForState(BlockState state)
    {
        return state switch
        {
            BlockState.InUnorderedList => "</ul>",
            BlockState.InBlockquote => "</blockquote>",
            _ => ""
        };
    }

    private string ApplyInlineRules(string text)
    {
        // This is a simplification. True nested parsing is more complex.
        // For this project, applying rules in a specific order gets us close.
        foreach (var rule in _inlineRules)
        {
            text = rule.Apply(text);
        }
        return text;
    }
}