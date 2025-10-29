using System.Text.RegularExpressions;

namespace Marky.Rules;

/// <summary>
/// A rule to convert Markdown bold text (**text**) into HTML strong tags.
/// </summary>
public class BoldRule : IParseRule
{
    public string Apply(string markdown)
    {
        return Regex.Replace(markdown, @"\*\*(.*?)\*\*", "<strong>$1</strong>");
    }
}
