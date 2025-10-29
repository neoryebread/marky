using Marky.Rules;
using Xunit;

namespace Marky.Tests.RuleTests;

public class BlockRuleTests
{
    [Theory]
    [InlineData("# H1", "<h1>H1</h1>")]
    [InlineData("## H2", "<h2>H2</h2>")]
    [InlineData("### H3", "<h3>H3</h3>")]
    [InlineData("#### H4", "<h4>H4</h4>")]
    [InlineData("##### H5", "<h5>H5</h5>")]
    [InlineData("###### H6", "<h6>H6</h6>")]
    public void HeaderRule_ShouldConvertAllLevels(string markdown, string expected)
    {
        var rule = new HeaderRule();
        Assert.Equal(expected, rule.Apply(markdown));
    }

    [Fact]
    public void HorizontalRule_ShouldConvert()
    {
        var rule = new HorizontalRule();
        Assert.Equal("<hr>", rule.Apply("---"));
    }

    [Fact]
    public void BlockquoteRule_ShouldConvert()
    {
        var rule = new BlockquoteRule();
        Assert.Equal("<blockquote>Hello</blockquote>", rule.Apply("> Hello"));
    }


    [Fact]
    public void UnorderedListRule_ShouldConvert()
    {
        var rule = new UnorderedListRule();
        Assert.Equal("<li>Item</li>", rule.Apply("* Item"));
    }
}
