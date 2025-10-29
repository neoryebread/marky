namespace Marky.Rules;

/// <summary>
/// A rule to convert a Markdown horizontal rule (---) into an HTML hr tag.
/// </summary>
public class HorizontalRule : IParseRule
{
    /// <inheritdoc/>
    public string Apply(string markdown)
    {
        if (markdown.Trim() == "---")
        {
            return "<hr>";
        }
        return markdown;
    }
}
