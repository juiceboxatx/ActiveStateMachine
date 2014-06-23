using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TelephoneStateMachine
{
    public class TelephoneStateMachine : ActiveStateMachine.ActiveStateMachine
    {
        private TelephoneStateMachineConfiguration config;

        public TelephoneStateMachine(TelephoneStateMachineConfiguration telConfig)
            : base(telConfig.TelephoneStateMachineStateList,telConfig.MaxEntries)
        {
            // Get config for the phone
            this.config = telConfig;

            // Configure event manager routing information. Vents are registered and mapped to handlers.
            telConfig.DoEventMappings(this, this.config.TelephoneActivities);
        }
    }
}
