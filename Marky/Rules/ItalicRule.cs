using System.Text.RegularExpressions;

namespace Marky.Rules;

/// <summary>
/// A rule to convert Markdown italic text (*text*) into HTML em tags.
/// </summary>
public class ItalicRule : IParseRule
{
    public string Apply(string markdown)
    {
        return Regex.Replace(markdown, @"\*(.*?)\*", "<em>$1</em>");
    }
}
