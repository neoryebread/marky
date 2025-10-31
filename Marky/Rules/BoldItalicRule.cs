using System.Text.RegularExpressions;

namespace Marky.Rules;

/// <summary>
/// A rule to convert Markdown bold and italic text (***text***) into HTML strong and em tags.
/// </summary>
public class BoldItalicRule : IParseRule
{
    /// <inheritdoc/>
    public string Apply(string markdown)
    {
        return Regex.Replace(markdown, @"\*\*\*(.*?)\*\*\*", "<strong><em>$1</em></strong>");
    }
}
