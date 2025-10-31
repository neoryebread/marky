namespace Marky.States
{
    internal class NoneState : IState
    {
        public void ProcessLine(MarkdownConverter converter, string line, List<string> htmlBody)
        {
            if (string.IsNullOrWhiteSpace(line))
            {
                converter.FlushParagraphBuffer(htmlBody);
                return;
            }

            var targetState = converter.GetStateForLine(line);

            if (targetState != BlockState.None)
            {
                converter.TransitionToState(targetState, line, htmlBody);
                if (targetState != BlockState.InFencedCodeBlock)
                {
                    converter.ProcessLine(line, htmlBody);
                }
                return;
            }
            
            converter.HandleDefaultLogic(line, htmlBody);
        }

        public void EnterState(MarkdownConverter converter, string line, List<string> htmlBody)
        {
            // No special action on entering None state.
        }

        public void ExitState(MarkdownConverter converter, List<string> htmlBody)
        {
            converter.FlushParagraphBuffer(htmlBody);
        }
    }
}
