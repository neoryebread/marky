using Xunit;

namespace Marky.Tests.RuleTests;

public class ListParsingTests
{
    private readonly MarkdownConverter _converter = new();

    [Fact]
    public void Convert_ShouldHandleSimpleOrderedList()
    {
        var markdown = "1. One\n2. Two";
        var expected = "<ol>\n<li>One</li>\n<li>Two</li>\n</ol>";
        Assert.Equal(expected, _converter.Convert(markdown));
    }

    [Fact]
    public void Convert_ShouldHandleSimpleUnorderedList()
    {
        var markdown = "* One\n* Two";
        var expected = "<ul>\n<li>One</li>\n<li>Two</li>\n</ul>";
        Assert.Equal(expected, _converter.Convert(markdown));
    }

    [Fact]
    public void Convert_ShouldHandleOrderedListWithNestedUnorderedList()
    {
        var markdown = "1. First\n  * Nested A\n  * Nested B\n2. Second";
        var expected = "<ol>\n<li>First</li>\n<ul>\n  <li>Nested A</li>\n  <li>Nested B</li>\n</ul>\n<li>Second</li>\n</ol>";
        Assert.Equal(expected, _converter.Convert(markdown));
    }

    [Fact]
    public void Convert_ShouldHandleUnorderedListWithNestedOrderedList()
    {
        var markdown = "* First\n  1. Nested A\n  2. Nested B\n* Second";
        var expected = "<ul>\n<li>First</li>\n<ol>\n  <li>Nested A</li>\n  <li>Nested B</li>\n</ol>\n<li>Second</li>\n</ul>";
        Assert.Equal(expected, _converter.Convert(markdown));
    }

    [Fact]
    public void Convert_ShouldHandleMultipleLevelsOfNesting()
    {
        var markdown = "1. Level 1\n  * Level 2\n    1. Level 3";
        var expected = "<ol>\n<li>Level 1</li>\n<ul>\n  <li>Level 2</li>\n<ol>\n    <li>Level 3</li>\n</ol>\n</ul>\n</ol>";
        Assert.Equal(expected, _converter.Convert(markdown));
    }

    [Fact]
    public void Convert_ShouldCloseAllTagsCorrectlyAfterList()
    {
        var markdown = "* Item A\n  1. Item B\n\nA new paragraph.";
        var expected = "<ul>\n<li>Item A</li>\n<ol>\n  <li>Item B</li>\n</ol>\n</ul>\n<p>A new paragraph.</p>";
        Assert.Equal(expected, _converter.Convert(markdown));
    }
}
