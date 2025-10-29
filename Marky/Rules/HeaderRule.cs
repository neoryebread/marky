using System.Text.RegularExpressions;

namespace Marky.Rules;

/// <summary>
/// A rule to convert Markdown headers (e.g., # Header 1) into HTML h tags.
/// Supports header levels 1 through 6.
/// </summary>
public class HeaderRule : IParseRule
{
    public string Apply(string markdown)
    {
        var match = Regex.Match(markdown, @"^(#{1,6})\s(.*)");
        if (match.Success)
        {
            var level = match.Groups[1].Length;
            var content = match.Groups[2].Value;
            return $"<h{level}>{content}</h{level}>";
        }
        return markdown;
    }
}
