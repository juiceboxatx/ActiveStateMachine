using System.Collections.Generic;

namespace ActiveStateMachine
{
    public class Transition
    {
        public Transition(string name, string sourceState, string targetState, List<StateMachineAction> guardList, List<StateMachineAction> transitionActionList, string trigger)
        {
            this.Name = name;
            this.SourceStateName = sourceState;
            this.TargetStateName = targetState;
            this.GuardList = guardList;
            this.TransitionActionList = transitionActionList;
            this.Trigger = trigger;
        }

        public List<StateMachineAction> GuardList { get; private set; }

        public string Name { get; private set; }

        public string SourceStateName { get; private set; }

        public string TargetStateName { get; private set; }
        public List<StateMachineAction> TransitionActionList { get; private set; }

        public string Trigger { get; private set; }
    }
}