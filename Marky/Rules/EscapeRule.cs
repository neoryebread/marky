using System.Text.RegularExpressions;

namespace Marky.Rules;

/// <summary>
/// A rule to handle escaped Markdown characters (e.g., \* becomes *).
/// </summary>
public class EscapeRule : IParseRule
{
    /// <inheritdoc/>
    public string Apply(string markdown)
    {
        // This regex looks for a backslash followed by any Markdown special character.
        // The special characters are: \ ` * _ { } [ ] ( ) # + - . !
        // It replaces "\\char" with "char".
        return Regex.Replace(markdown, @"\\([\\`*_{}\[\]()#+\-.!])", "$1");
    }
}
