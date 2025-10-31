using System.Text.RegularExpressions;

namespace Marky.Rules;

/// <summary>
/// A rule to convert a Markdown blockquote line (> text) into its content.
/// The converter is responsible for wrapping the content in blockquote tags.
/// </summary>
public class BlockquoteRule : IParseRule
{
    private static readonly Regex BlockquoteRegex = new(@"^>\s(.*)");

    /// <inheritdoc/>
    public string Apply(string markdown)
    {
        var match = BlockquoteRegex.Match(markdown);
        if (match.Success)
        {
            // Return only the content; the converter will handle the tags.
            return match.Groups[1].Value;
        }
        return markdown;
    }

    /// <summary>
    /// Checks if a line is a blockquote item.
    /// </summary>
    public static bool IsBlockquote(string markdown) => BlockquoteRegex.IsMatch(markdown);
}