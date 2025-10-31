using Marky.Rules;

namespace Marky.States
{
    internal class InListState : IState
    {
        public void ProcessLine(MarkdownConverter converter, string line, List<string> htmlBody)
        {
            if (string.IsNullOrWhiteSpace(line) || !(UnorderedListRule.IsUnorderedList(line) || OrderedListRule.IsOrderedList(line)))
            {
                converter.TransitionToState(BlockState.None, line, htmlBody);
                converter.ProcessLine(line, htmlBody);
                return;
            }
            
            converter.HandleListLogic(line, htmlBody);
        }

        public void EnterState(MarkdownConverter converter, string line, List<string> htmlBody)
        {
            // List opening tags are handled within HandleListLogic due to nesting complexity.
        }

        public void ExitState(MarkdownConverter converter, List<string> htmlBody)
        {
            converter.CloseAllLists(htmlBody);
        }
    }
}
