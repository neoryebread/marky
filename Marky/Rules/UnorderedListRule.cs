using System.Text.RegularExpressions;

namespace Marky.Rules;

/// <summary>
/// A rule to convert Markdown unordered list items (* item) into HTML li tags.
/// This rule only handles the item itself, not the parent ul tag.
/// </summary>
public class UnorderedListRule : IParseRule
{
    private static readonly Regex UnorderedListRegex = new(@"^(\s*)\*\s(.*)");

    public string Apply(string markdown)
    {
        var match = UnorderedListRegex.Match(markdown);
        if (match.Success)
        {
            return $"{match.Groups[1].Value}<li>{match.Groups[2].Value}</li>";
        }
        return markdown;
    }
    
    /// <summary>
    /// Checks if a line is an unordered list item.
    /// </summary>
    public static bool IsUnorderedList(string markdown) => UnorderedListRegex.IsMatch(markdown);

    /// <summary>
    /// Gets the indentation level of a list item.
    /// </summary>
    public static int GetIndentation(string markdown) => UnorderedListRegex.Match(markdown).Groups[1].Length;
}
