using System.Text.RegularExpressions;

namespace Marky.Rules;

/// <summary>
/// A rule to convert Markdown ordered list items (e.g., "1. item") into HTML li tags.
/// This rule only handles the item itself, not the parent ol tag.
/// </summary>
public class OrderedListRule : IParseRule
{
    // This regex now also captures the indentation level.
    private static readonly Regex OrderedListRegex = new(@"^(\s*)(\d+)\.\s(.*)");

    /// <inheritdoc/>
    public string Apply(string markdown)
    {
        var match = OrderedListRegex.Match(markdown);
        if (match.Success)
        {
            // The converter will handle the indentation; this rule just transforms to an <li>.
            return $"{match.Groups[1].Value}<li>{match.Groups[3].Value}</li>";
        }
        return markdown;
    }

    /// <summary>
    /// Checks if a line is an ordered list item.
    /// </summary>
    public static bool IsOrderedList(string markdown) => OrderedListRegex.IsMatch(markdown);

    /// <summary>
    /// Gets the indentation level of a list item.
    /// </summary>
    public static int GetIndentation(string markdown) => OrderedListRegex.Match(markdown).Groups[1].Length;
}
