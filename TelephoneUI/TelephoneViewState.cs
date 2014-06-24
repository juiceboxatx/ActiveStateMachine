using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common;

namespace TelephoneUI
{
    public class TelephoneViewState : IViewState
    {
        public string Name { get; private set; }

        public bool Bell { get; private set; }

        public bool Line { get; private set; }

        public bool ReceiverHungUp { get; private set; }

        public bool IsDefaultViewState { get; private set; }

        public TelephoneViewState(string name, bool bell, bool line, bool receiverHungUp, bool isDefaultViewState = false)
        {
            this.Name = name;
            this.Bell = bell;
            this.Line = line;
            this.ReceiverHungUp = receiverHungUp;
            this.IsDefaultViewState = isDefaultViewState;
        }
    }
}
