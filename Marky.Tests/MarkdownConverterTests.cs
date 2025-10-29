using Xunit;

namespace Marky.Tests;

/// <summary>
/// These are integration tests for the MarkdownConverter, ensuring that
/// different types of rules interact correctly.
/// </summary>
public class MarkdownConverterTests
{
    private readonly MarkdownConverter _converter = new();

    [Fact]
    public void Convert_ShouldHandleParagraphsAndHeaders()
    {
        var markdown = "This is a paragraph.\n# This is a header.";
        var result = _converter.Convert(markdown);
        Assert.Contains("<p>This is a paragraph.</p>", result);
        Assert.Contains("<h1>This is a header.</h1>", result);
    }

    [Fact]
    public void Convert_ShouldHandleAComplexDocument()
    {
        var markdown = "# Document Title\n\n" +
                       "This is the first paragraph with **bold** text and a [link](http://a.com).\n\n" +
                       "* List item one\n" +
                       "* List item `two` with code\n\n" +
                       "> A blockquote follows the list.\n\n" +
                       "## A Subheading\n\n" +
                       "![Image alt](img.png)";

        var result = _converter.Convert(markdown);

        // Check for major elements
        Assert.Contains("<h1>Document Title</h1>", result);
        Assert.Contains("<h2>A Subheading</h2>", result);
        Assert.Contains("<p>This is the first paragraph with <strong>bold</strong> text and a <a href=\"http://a.com\">link</a>.</p>", result);
        Assert.Contains("<ul>", result);
        Assert.Contains("<li>List item one</li>", result);
        Assert.Contains("<li>List item <code>two</code> with code</li>", result);
        Assert.Contains("</ul>", result);
        Assert.Contains("<blockquote>A blockquote follows the list.</blockquote>", result);
        Assert.Contains("<img src=\"img.png\" alt=\"Image alt\">", result);
    }

    [Fact]
    public void Convert_ShouldCorrectlyOpenAndCloseUnorderedLists()
    {
        var markdown = "Para 1\n* Item 1\n* Item 2\nPara 2";
        var result = _converter.Convert(markdown);
        
        var expected = "<p>Para 1</p>\n" +
                       "<ul>\n" +
                       "<li>Item 1</li>\n" +
                       "<li>Item 2</li>\n" +
                       "</ul>\n" +
                       "<p>Para 2</p>";

        Assert.Equal(expected, result);
    }
}
