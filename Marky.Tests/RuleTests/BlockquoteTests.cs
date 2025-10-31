using Xunit;

namespace Marky.Tests.RuleTests;

public class BlockquoteTests
{
    private readonly MarkdownConverter _converter = new();

    [Fact]
    public void Convert_ShouldHandleSingleLineBlockquote()
    {
        var markdown = "> Hello world";
        var expected = "<blockquote>\n<p>Hello world</p>\n</blockquote>";
        Assert.Equal(expected, _converter.Convert(markdown));
    }

    [Fact]
    public void Convert_ShouldHandleMultiLineBlockquote()
    {
        var markdown = "> First line.\n> Second line.";
        var expected = "<blockquote>\n<p>First line.</p>\n<p>Second line.</p>\n</blockquote>";
        Assert.Equal(expected, _converter.Convert(markdown));
    }

    [Fact]
    public void Convert_ShouldHandleBlockquoteWithInlineStyles()
    {
        var markdown = "> This is **bold** and *italic*.";
        var expected = "<blockquote>\n<p>This is <strong>bold</strong> and <em>italic</em>.</p>\n</blockquote>";
        Assert.Equal(expected, _converter.Convert(markdown));
    }

    [Fact]
    public void Convert_ShouldSeparateBlockquotesWithBlankLines()
    {
        var markdown = "> Quote one.\n\n> Quote two.";
        var expected = "<blockquote>\n<p>Quote one.</p>\n</blockquote>\n<blockquote>\n<p>Quote two.</p>\n</blockquote>";
        Assert.Equal(expected, _converter.Convert(markdown));
    }

    [Fact]
    public void Convert_ShouldHandleParagraphThenBlockquote()
    {
        var markdown = "A normal paragraph.\n\n> A quote.";
        var expected = "<p>A normal paragraph.</p>\n<blockquote>\n<p>A quote.</p>\n</blockquote>";
        Assert.Equal(expected, _converter.Convert(markdown));
    }
}
