using System.Text;

namespace Marky;

/// <summary>
/// Constructs a valid HTML5 document.
/// </summary>
public class HtmlBuilder
{
    private readonly StringBuilder _bodyContent = new();
    private string _title = "Markdown Document";

    /// <summary>
    /// Sets the title of the HTML document.
    /// </summary>
    /// <param name="title">The text to use for the document title.</param>
    public void SetTitle(string title)
    {
        _title = title;
    }

    /// <summary>
    /// Appends a line of content to the body of the HTML document.
    /// </summary>
    /// <param name="content">The HTML content to append.</param>
    public void AppendBody(string content)
    {
        _bodyContent.AppendLine(content);
    }

    /// <summary>
    /// Generates the complete HTML document as a string.
    /// </summary>
    /// <returns>The full HTML document.</returns>
    public override string ToString()
    {
        var html = new StringBuilder();
        html.AppendLine("<!DOCTYPE html>");
        html.AppendLine("<html lang=\"en\">");
        html.AppendLine("<head>");
        html.AppendLine("  <meta charset=\"UTF-8\">");
        html.AppendLine("  <meta name=\"viewport\" content=\"width=device-width, initial-scale=1.0\">");
        html.AppendLine($"  <title>{_title}</title>");
        html.AppendLine("  <style>");
        html.AppendLine("    body { font-family: sans-serif; line-height: 1.6; padding: 2em; max-width: 800px; margin: 0 auto; }");
        html.AppendLine("    code { background-color: #f4f4f4; padding: 0.2em 0.4em; margin: 0; border-radius: 3px; }");
        html.AppendLine("    blockquote { border-left: 5px solid #ccc; padding-left: 1.5em; margin-left: 0; color: #666; }");
        html.AppendLine("  </style>");
        html.AppendLine("</head>");
        html.AppendLine("<body>");
        html.AppendLine(_bodyContent.ToString());
        html.AppendLine("</body>");
        html.AppendLine("</html>");
        return html.ToString();
    }
}
