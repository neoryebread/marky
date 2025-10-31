using Marky.Rules;

namespace Marky.States
{
    internal class InBlockquoteState : IState
    {
        public void ProcessLine(MarkdownConverter converter, string line, List<string> htmlBody)
        {
            if (string.IsNullOrWhiteSpace(line) || !BlockquoteRule.IsBlockquote(line))
            {
                converter.TransitionToState(BlockState.None, line, htmlBody);
                // The new state will handle the line.
                converter.ProcessLine(line, htmlBody);
                return;
            }

            var content = new BlockquoteRule().Apply(line);
            htmlBody.Add($"<p>{converter.ApplyInlineRulesToContent(content)}</p>");
        }

        public void EnterState(MarkdownConverter converter, string line, List<string> htmlBody)
        {
            htmlBody.Add("<blockquote>");
        }

        public void ExitState(MarkdownConverter converter, List<string> htmlBody)
        {
            htmlBody.Add("</blockquote>");
        }
    }
}
