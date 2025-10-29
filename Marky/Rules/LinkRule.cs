using System.Text.RegularExpressions;

namespace Marky.Rules;

/// <summary>
/// A rule to convert Markdown links ([text](url)) into HTML anchor tags.
/// </summary>
public class LinkRule : IParseRule
{
    /// <inheritdoc/>
    public string Apply(string markdown)
    {
        return Regex.Replace(markdown, @"\[(.*?)\]\((.*?)\)", @"<a href=""$2"">$1</a>");
    }
}