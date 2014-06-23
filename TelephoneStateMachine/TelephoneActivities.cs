using Common;
using System;

namespace TelephoneStateMachine
{
    public class TelephoneActivities
    {
        public event EventHandler<StateMachineEventArgs> TelephoneDeviceEvent;

        // Events to communicated from state machine to managers - wireing will be done via event manager
        public event EventHandler<StateMachineEventArgs> TelephoneUiEvent;

        // Device Actions
        public void ActionBellRings()
        {
            this.RaiseDeviceEvent("Bell", "Rings");
        }

        public void ActionBellSilent()
        {
            this.RaiseDeviceEvent("Bell", "Silent");
        }

        public void ActionLineActive()
        {
            this.RaiseDeviceEvent("PhoneLine", "ActiveInternal");
        }

        public void ActionLineOff()
        {
            this.RaiseDeviceEvent("PhoneLine", "OffInternal");
        }

        public void ActionViewPhoneRings()
        {
            this.RaiseTelephoneUiEvent("ViewPhonerings");
        }

        public void ActionViewPhoneIdle()
        {
            this.RaiseTelephoneUiEvent("ViwePhoneIdle");
            System.Media.SystemSounds.Beep.Play();
        }

        public void ActionViewTalking()
        {
            this.RaiseTelephoneUiEvent("ViewTalking");
        }

        private void RaiseDeviceEvent(string target, string command)
        {
            var devEvent = this.TelephoneUiEvent;
            if (devEvent != null)
            {
                var telArgs = new StateMachineEventArgs(command, "Device command", StateMachineEventType.Command, "State machine action", target);
                devEvent(this, telArgs);
            }
        }

        /// <summary>
        /// Helper to raise the telphone UI event
        /// </summary>
        /// <param name="command"></param>
        private void RaiseTelephoneUiEvent(string command)
        {
            var uiEvent = this.TelephoneUiEvent;
            if (uiEvent != null)
            {
                var telArgs = new StateMachineEventArgs(command, "UI command", StateMachineEventType.Command, "State machine action", "View Manager");
                uiEvent(this, telArgs);
            }
        }
    }
}