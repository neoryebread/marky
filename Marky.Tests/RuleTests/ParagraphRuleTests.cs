using Xunit;

namespace Marky.Tests.RuleTests;

public class ParagraphRuleTests
{
    private readonly MarkdownConverter _converter = new();

    [Fact]
    public void Convert_ShouldHandleSingleLineParagraph()
    {
        var markdown = "This is a single line.";
        var expected = "<p>This is a single line.</p>";
        var result = _converter.Convert(markdown);
        Assert.Equal(expected, result);
    }

    [Fact]
    public void Convert_ShouldHandleMultiLineParagraph()
    {
        var markdown = "This is the first line.\nThis is the second line.";
        var expected = "<p>This is the first line. This is the second line.</p>";
        var result = _converter.Convert(markdown);
        Assert.Equal(expected, result);
    }

    [Fact]
    public void Convert_ShouldCreateSeparateParagraphsForTextSeparatedByBlankLines()
    {
        var markdown = "This is paragraph one.\n\nThis is paragraph two.";
        var expected = "<p>This is paragraph one.</p>\n<p>This is paragraph two.</p>";
        var result = _converter.Convert(markdown);
        Assert.Equal(expected, result);
    }

    [Fact]
    public void Convert_ShouldNotWrapBlockElementsInParagraphs()
    {
        var markdown = "# A Header\n\nJust a paragraph.";
        var expected = "<h1>A Header</h1>\n<p>Just a paragraph.</p>";
        var result = _converter.Convert(markdown);
        Assert.Equal(expected, result);
    }
}
