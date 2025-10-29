namespace Marky.Rules;

/// <summary>
/// A rule to convert Markdown blockquotes (> text) into HTML blockquote tags.
/// </summary>
public class BlockquoteRule : IParseRule
{
    /// <inheritdoc/>
    public string Apply(string markdown)
    {
        if (markdown.StartsWith("> "))
        {
            return $"<blockquote>{markdown.Substring(2)}</blockquote>";
        }
        return markdown;
    }
}
