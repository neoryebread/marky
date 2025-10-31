using Marky.Rules;

namespace Marky.States
{
    internal class InFencedCodeBlockState : IState
    {
        public void ProcessLine(MarkdownConverter converter, string line, List<string> htmlBody)
        {
            if (FencedCodeBlockRule.IsFencedCodeBlock(line))
            {
                converter.TransitionToState(BlockState.None, line, htmlBody);
            }
            else
            {
                converter.HandleFencedCodeBlockLogic(line, htmlBody);
            }
        }

        public void EnterState(MarkdownConverter converter, string line, List<string> htmlBody)
        {
            converter.CodeBlockLanguage = FencedCodeBlockRule.GetLanguage(line);
            var langClass = !string.IsNullOrEmpty(converter.CodeBlockLanguage) ? $" class=\"language-{converter.CodeBlockLanguage}\"" : "";
            htmlBody.Add($"<pre><code{langClass}>");
        }

        public void ExitState(MarkdownConverter converter, List<string> htmlBody)
        {
            if (htmlBody.Any())
            {
                htmlBody[htmlBody.Count - 1] += "\n</code></pre>";
            }
        }
    }
}
