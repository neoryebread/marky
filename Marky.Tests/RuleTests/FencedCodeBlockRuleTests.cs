using Xunit;

namespace Marky.Tests.RuleTests
{
    public class FencedCodeBlockRuleTests
    {
        private readonly MarkdownConverter _converter = new();

        [Fact]
        public void FencedCodeBlock_ShouldBeConverted()
        {
            var markdown = "```\n" +
                           "public void main() {\n" +
                           "  Console.WriteLine(\"Hello, World!\");\n" +
                           "}\n" +
                           "```";
            var expected = "<pre><code>" +
                           "public void main() {\n" +
                           "  Console.WriteLine(&quot;Hello, World!&quot;);\n" +
                           "}\n" +
                           "</code></pre>";

            Assert.Equal(expected, _converter.Convert(markdown));
        }

        [Fact]
        public void FencedCodeBlock_WithLanguage_ShouldBeConverted()
        {
            var markdown = "```csharp\n" +
                           "public void main() {\n" +
                           "  Console.WriteLine(\"Hello, World!\");\n" +
                           "}\n" +
                           "```";
            var expected = "<pre><code class=\"language-csharp\">" +
                           "public void main() {\n" +
                           "  Console.WriteLine(&quot;Hello, World!&quot;);\n" +
                           "}\n" +
                           "</code></pre>";

            Assert.Equal(expected, _converter.Convert(markdown));
        }

        [Fact]
        public void FencedCodeBlock_WithOtherText_ShouldBeConverted()
        {
            var markdown = "Some text before\n" +
                           "```csharp\n" +
                           "public void main() {\n" +
                           "  Console.WriteLine(\"Hello, World!\");\n" +
                           "}\n" +
                           "```\n" +
                           "Some text after";
            var expected = "<p>Some text before</p>\n" +
                           "<pre><code class=\"language-csharp\">" +
                           "public void main() {\n" +
                           "  Console.WriteLine(&quot;Hello, World!&quot;);\n" +
                           "}\n" +
                           "</code></pre>\n" +
                           "<p>Some text after</p>";

            Assert.Equal(expected, _converter.Convert(markdown));
        }

        [Fact]
        public void FencedCodeBlock_EmptyBlock_ShouldBeConverted()
        {
            var markdown = "```\n```";
            var expected = "<pre><code>\n</code></pre>";
            Assert.Equal(expected, _converter.Convert(markdown));
        }

        [Fact]
        public void FencedCodeBlock_SingleLine_ShouldBeConverted()
        {
            var markdown = "```\n" +
                           "hello\n" +
                           "```";
            var expected = "<pre><code>" +
                           "hello\n" +
                           "</code></pre>";
            Assert.Equal(expected, _converter.Convert(markdown));
        }

        [Fact]
        public void FencedCodeBlock_WithSpecialCharacters_ShouldBeEscaped()
        {
            var markdown = "```\n" +
                           "<p>Hello</p>\n" +
                           "```";
            var expected = "<pre><code>" +
                           "&lt;p&gt;Hello&lt;/p&gt;\n" +
                           "</code></pre>";
            Assert.Equal(expected, _converter.Convert(markdown));
        }

        [Fact]
        public void FencedCodeBlock_MultipleBlocks_ShouldBeConverted()
        {
            var markdown = "```\n" +
                           "one\n" +
                           "```\n" +
                           "some text\n" +
                           "```\n" +
                           "two\n" +
                           "```";
            var expected = "<pre><code>" +
                           "one\n" +
                           "</code></pre>\n" +
                           "<p>some text</p>\n" +
                           "<pre><code>" +
                           "two\n" +
                           "</code></pre>";
            Assert.Equal(expected, _converter.Convert(markdown));
        }

        [Fact]
        public void FencedCodeBlock_WithBlankLines_ShouldPreserveThem()
        {
            var markdown = "```\n" +
                           "line 1\n" +
                           "\n" +
                           "line 3\n" +
                           "```";
            var expected = "<pre><code>" +
                           "line 1\n" +
                           "\n" +
                           "line 3\n" +
                           "</code></pre>";
            Assert.Equal(expected, _converter.Convert(markdown));
        }
    }
}
