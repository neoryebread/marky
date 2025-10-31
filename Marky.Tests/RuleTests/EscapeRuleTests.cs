using Marky.Rules;
using Xunit;

namespace Marky.Tests.RuleTests;

public class EscapeRuleTests
{
    [Fact]
    public void Apply_ShouldUnescapeAsterisk()
    {
        var rule = new EscapeRule();
        var markdown = @"This is \*bold";
        var expected = @"This is *bold";
        Assert.Equal(expected, rule.Apply(markdown));
    }

    [Fact]
    public void Apply_ShouldUnescapeBacktick()
    {
        var rule = new EscapeRule();
        var markdown = @"This is \`code";
        var expected = @"This is `code";
        Assert.Equal(expected, rule.Apply(markdown));
    }

    [Fact]
    public void Apply_ShouldUnescapeUnderscore()
    {
        var rule = new EscapeRule();
        var markdown = @"This is \_italic";
        var expected = @"This is _italic";
        Assert.Equal(expected, rule.Apply(markdown));
    }

    [Fact]
    public void Apply_ShouldUnescapeMultipleCharacters()
    {
        var rule = new EscapeRule();
        var markdown = @"\*Item 1\* \_Item 2\_ \`Item 3";
        var expected = @"*Item 1* _Item 2_ `Item 3";
        Assert.Equal(expected, rule.Apply(markdown));
    }

    [Fact]
    public void Apply_ShouldNotAffectUnescapedCharacters()
    {
        var rule = new EscapeRule();
        var markdown = "This is *not* escaped";
        var expected = "This is *not* escaped";
        Assert.Equal(expected, rule.Apply(markdown));
    }

    [Fact]
    public void Apply_ShouldHandleEscapedBackslash()
    {
        var rule = new EscapeRule();
        var markdown = @"A literal backslash: \\.";
        var expected = @"A literal backslash: \.";
        Assert.Equal(expected, rule.Apply(markdown));
    }

    [Fact]
    public void Apply_ShouldHandleEscapedPoundSign()
    {
        var rule = new EscapeRule();
        var markdown = @"\# A header";
        var expected = @"# A header";
        Assert.Equal(expected, rule.Apply(markdown));
    }

    [Fact]
    public void Apply_ShouldHandleEscapedPlusSign()
    {
        var rule = new EscapeRule();
        var markdown = @"Item \+ one";
        var expected = @"Item + one";
        Assert.Equal(expected, rule.Apply(markdown));
    }

    [Fact]
    public void Apply_ShouldHandleEscapedMinusSign()
    {
        var rule = new EscapeRule();
        var markdown = @"Item \- one";
        var expected = @"Item - one";
        Assert.Equal(expected, rule.Apply(markdown));
    }

    [Fact]
    public void Apply_ShouldHandleEscapedDot()
    {
        var rule = new EscapeRule();
        var markdown = @"1\. Item";
        var expected = @"1. Item";
        Assert.Equal(expected, rule.Apply(markdown));
    }

    [Fact]
    public void Apply_ShouldHandleEscapedExclamationMark()
    {
        var rule = new EscapeRule();
        var markdown = @"\!image";
        var expected = @"!image";
        Assert.Equal(expected, rule.Apply(markdown));
    }

    [Fact]
    public void Apply_ShouldHandleEscapedSquareBrackets()
    {
        var rule = new EscapeRule();
        var markdown = @"\[link\]";
        var expected = @"[link]";
        Assert.Equal(expected, rule.Apply(markdown));
    }

    [Fact]
    public void Apply_ShouldHandleEscapedParentheses()
    {
        var rule = new EscapeRule();
        var markdown = @"\(content\)";
        var expected = @"(content)";
        Assert.Equal(expected, rule.Apply(markdown));
    }

    [Fact]
    public void Apply_ShouldHandleEscapedCurlyBraces()
    {
        var rule = new EscapeRule();
        var markdown = @"\{key: value\}";
        var expected = @"{key: value}";
        Assert.Equal(expected, rule.Apply(markdown));
    }
}
