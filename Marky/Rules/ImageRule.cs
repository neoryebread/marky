using System.Text.RegularExpressions;

namespace Marky.Rules;

/// <summary>
/// A rule to convert Markdown images (![alt](src)) into HTML img tags.
/// </summary>
public class ImageRule : IParseRule
{
    /// <inheritdoc/>
    public string Apply(string markdown)
    {
        return Regex.Replace(markdown, @"!\[(.*?)\]\((.*?)\)", "<img src=\"$2\" alt=\"$1\">");
    }
}
