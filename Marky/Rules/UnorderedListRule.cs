namespace Marky.Rules;

/// <summary>
/// A rule to convert Markdown unordered list items (* item) into HTML li tags.
/// This rule only handles the item itself, not the parent ul tag.
/// </summary>
public class UnorderedListRule : IParseRule
{
    public string Apply(string markdown)
    {
        if (markdown.StartsWith("* "))
        {
            return $"<li>{markdown.Substring(2)}</li>";
        }
        return markdown;
    }
}