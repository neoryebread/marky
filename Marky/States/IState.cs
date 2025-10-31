namespace Marky.States
{
    internal interface IState
    {
        void ProcessLine(MarkdownConverter converter, string line, List<string> htmlBody);
        void EnterState(MarkdownConverter converter, string line, List<string> htmlBody);
        void ExitState(MarkdownConverter converter, List<string> htmlBody);
    }
}
