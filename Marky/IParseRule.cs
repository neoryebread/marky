namespace Marky;

/// <summary>
/// Defines the contract for a rule that converts a line of Markdown into HTML.
/// </summary>
public interface IParseRule
{
    /// <summary>
    /// Applies the parsing rule to a given line of Markdown text.
    /// </summary>
    /// <param name="markdown">The line of Markdown to process.</param>
    /// <returns>The converted HTML string, or the original string if the rule does not apply.</returns>
    string Apply(string markdown);
}