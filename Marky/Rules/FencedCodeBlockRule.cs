using System.Text.RegularExpressions;

namespace Marky.Rules
{
    public class FencedCodeBlockRule : IParseRule
    {
        private static readonly Regex FencedCodeBlockRegex = new Regex(@"^```(\w*)$");

        public string Apply(string markdown)
        {
            return markdown;
        }

        public static bool IsFencedCodeBlock(string markdown)
        {
            return FencedCodeBlockRegex.IsMatch(markdown);
        }

        public static string GetLanguage(string markdown)
        {
            var match = FencedCodeBlockRegex.Match(markdown);
            if (match.Success && match.Groups.Count > 1)
            {
                return match.Groups[1].Value;
            }
            return string.Empty;
        }
    }
}
