using System;

namespace ActiveStateMachine
{
    public class StateMachineAction
    {
        private Action method;

        public StateMachineAction(string name, Action method)
        {
            this.Name = name;
            this.method = method;
        }

        public string Name { get; private set; }

        public void Execute()
        {
            // Invoke the state machine action method
            this.method.Invoke();
        }
    }
}