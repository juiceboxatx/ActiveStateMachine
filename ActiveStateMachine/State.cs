using System.Collections.Generic;

namespace ActiveStateMachine
{
    public class State
    {
        public State(string name, Dictionary<string, Transition> transitionList, List<StateMachineAction> entryActions, List<StateMachineAction> exitActions, bool defaultState = true)
        {
            this.StateName = name;
            this.StateTransitionList = transitionList;
            this.EntryActions = entryActions;
            this.ExitActions = exitActions;
            this.IsDefaultState = defaultState;
        }

        public List<StateMachineAction> EntryActions { get; private set; }

        public List<StateMachineAction> ExitActions { get; private set; }

        public bool IsDefaultState { get; private set; }

        public string StateName { get; private set; }

        public Dictionary<string, Transition> StateTransitionList { get; private set; }
    }
}