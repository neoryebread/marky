using System.Text.RegularExpressions;

namespace Marky.Rules;

/// <summary>
/// A rule to convert Markdown inline code (`code`) into HTML code tags.
/// </summary>
public class InlineCodeRule : IParseRule
{
    /// <inheritdoc/>
    public string Apply(string markdown)
    {
        return Regex.Replace(markdown, @"`(.*?)`", "<code>$1</code>");
    }
}
