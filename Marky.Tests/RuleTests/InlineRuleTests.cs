using Marky.Rules;
using Xunit;

namespace Marky.Tests.RuleTests;

public class InlineRuleTests
{
    [Theory]
    [InlineData("This is **bold** text.", "This is <strong>bold</strong> text.")]
    [InlineData("Another **bold**.", "Another <strong>bold</strong>.")]
    [InlineData("**Multiple** bold **sections**.", "<strong>Multiple</strong> bold <strong>sections</strong>.")]
    public void BoldRule_ShouldConvert(string markdown, string expected)
    {
        var rule = new BoldRule();
        Assert.Equal(expected, rule.Apply(markdown));
    }

    [Theory]
    [InlineData("This is *italic* text.", "This is <em>italic</em> text.")]
    [InlineData("Another *italic*.", "Another <em>italic</em>.")]
    public void ItalicRule_ShouldConvert(string markdown, string expected)
    {
        var rule = new ItalicRule();
        Assert.Equal(expected, rule.Apply(markdown));
    }

    [Theory]
    [InlineData("Here is `inline code`.", "Here is <code>inline code</code>.")]
    [InlineData("`code`", "<code>code</code>")]
    public void InlineCodeRule_ShouldConvert(string markdown, string expected)
    {
        var rule = new InlineCodeRule();
        Assert.Equal(expected, rule.Apply(markdown));
    }

    [Theory]
    [InlineData("[A link](http://example.com)", @"<a href=""http://example.com"">A link</a>")]
    [InlineData("Text with [a link](https://google.com) inside.", @"Text with <a href=""https://google.com"">a link</a> inside.")]
    public void LinkRule_ShouldConvert(string markdown, string expected)
    {
        var rule = new LinkRule();
        Assert.Equal(expected, rule.Apply(markdown));
    }

    [Theory]
    [InlineData("![alt text](image.jpg)", @"<img src=""image.jpg"" alt=""alt text"">")]
    [InlineData("An image: ![alt](pic.png) here.", @"An image: <img src=""pic.png"" alt=""alt""> here.")]
    public void ImageRule_ShouldConvert(string markdown, string expected)
    {
        var rule = new ImageRule();
        Assert.Equal(expected, rule.Apply(markdown));
    }
}
