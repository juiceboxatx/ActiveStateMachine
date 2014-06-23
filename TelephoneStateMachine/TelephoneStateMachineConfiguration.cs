using ActiveStateMachine;
using ApplicationServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TelephoneStateMachine
{
    public class TelephoneStateMachineConfiguration
    {
        // List of valid states for thsi state machine
        public Dictionary<string, State> TelephoneStateMachineStateList { get; set; }

        // List of Activities in the system
        public TelephoneActivities TelephoneActivities { get; set; }

        // Max number of entries in trigger queue
        public int MaxEntries = 50;

        // Event Manager
        public EventManager TelephoneEventManager;

        // View Manager
        public ViewManager TelephoneViewManager;

        public DeviceManager TelephoneDeviceManager;

        // logger
        public LogManager TelephoneLogManager;

        public TelephoneStateMachineConfiguration()
        {
            this.BuildConfig();
        }

        /// <summary>
        /// Build telephone state configuration
        /// </summary>
        private void BuildConfig()
        {
            // Transitions and actions
            #region preparation of transitions and actions
            TelephoneActivities = new TelephoneActivities();

            // create actions and map action methods into the corresponding action object
            // Device actions
            var actionBellRings = new StateMachineAction("ActionBellRings", this.TelephoneActivities.ActionBellRings);
            var actionBellSilent = new StateMachineAction("ActionBellSilent", this.TelephoneActivities.ActionBellSilent);
            var actionLineOff = new StateMachineAction("ActionLineOff", this.TelephoneActivities.ActionLineOff);
            var actionLineActive = new StateMachineAction("ActionLineActive", this.TelephoneActivities.ActionLineActive);
            
            // View Actions
            var actionViewPhoneRings = new StateMachineAction("ActionViewPhoneRings", this.TelephoneActivities.ActionViewPhoneRings);
            var actionViewPhoneIdle = new StateMachineAction("ActionViewPhoneIdle", this.TelephoneActivities.ActionViewPhoneIdle);
            var actionViewTalking = new StateMachineAction("ActionViewTalking", this.TelephoneActivities.ActionViewTalking);

            // Create transitions and corresponding triggers, states need to be added
            var emptyList = new List<StateMachineAction>(); // to avoid null reference exceptions, use an empty list
            // transition IncomingCall
            var IncomingCallActions = new List<StateMachineAction>();
            IncomingCallActions.Add(actionViewPhoneRings);
            var transIncomingCall = new Transition("TransitionIncomingCall", "StatePhoneIdle", "StatePhoneRings", emptyList, IncomingCallActions, "OnLineExternalActive");

            // transition CallBlocked
            var CallBlockedActions = new List<StateMachineAction>();
            CallBlockedActions.Add(actionViewPhoneIdle);
            var transCallBlocked = new Transition("TransitionCallBlocked", "StatePhoneRings", "StatePhoneIdle", emptyList, CallBlockedActions, "OnReceiverDown");
            
            // transition CallAccepted
            var CallAcceptedActions = new List<StateMachineAction>();
            CallAcceptedActions.Add(actionViewTalking);
            var transCallAccepted = new Transition("TransitionCallAccepted", "StatePhoneRings", "StateTalking", emptyList, CallAcceptedActions, "OnReceiverUp");
            
            // transition CallEnded
            var CallEndedActions = new List<StateMachineAction>();
            CallEndedActions.Add(actionViewPhoneIdle);
            var transCallEnded = new Transition("TransitionCallEnded", "StateTalking", "StatePhoneIdle", emptyList, CallEndedActions, "OnReceiverDown");
            #endregion

            #region Assemble all states
            // create states
            // state: PhoneIdle
            var transitionsPhoneIdle = new Dictionary<string, Transition>();
            var entryActionsPhoneIdle = new List<StateMachineAction>();
            var exitActionsPhoneIdle = new List<StateMachineAction>();
            transitionsPhoneIdle.Add("TransitionIncomingCall", transIncomingCall);
            // Always specify all action lists, even empty ones, do not pass null into a state Lists are read via foreach, which will return an error
            var phoneIdle = new State("StatePhoneIdle", transitionsPhoneIdle, entryActionsPhoneIdle, exitActionsPhoneIdle, true);

            // state: PhoneRings
            var transitionsPhoneRings = new Dictionary<string, Transition>();
            var entryActionsPhoneRings = new List<StateMachineAction>();
            entryActionsPhoneRings.Add(actionBellRings);
            var exitActionsPhoneRings = new List<StateMachineAction>();
            exitActionsPhoneRings.Add(actionBellSilent);
            transitionsPhoneRings.Add("TransitionCallBlocked", transCallBlocked);
            transitionsPhoneRings.Add("TransitionCallAccepted", transCallAccepted);
            // Always specify all action lists, even empty ones, do not pass null into a state Lists are read via foreach, which will return an error
            var phoneRings = new State("StatePhoneRings", transitionsPhoneRings, entryActionsPhoneRings, exitActionsPhoneRings);

            // state: Talking
            var transitionsTalking = new Dictionary<string, Transition>();
            var entryActionsTalking = new List<StateMachineAction>();
            entryActionsTalking.Add(actionLineActive);
            var exitActionsTalking = new List<StateMachineAction>();
            exitActionsTalking.Add(actionLineOff);
            transitionsTalking.Add("TransitionCallEnded", transCallEnded);
            // Always specify all action lists, even empty ones, do not pass null into a state Lists are read via foreach, which will return an error
            var talking = new State("StateTalking", transitionsTalking, entryActionsTalking, exitActionsTalking);

            this.TelephoneStateMachineStateList = new Dictionary<string, State>
            {
                {"StatePhoneIdle", phoneIdle},
                {"StatePhoneRings", phoneRings},
                {"StateTalking", talking}
            };

            #endregion

            // Application Services
            #region Appilcation Services
            // Get application services
            this.TelephoneEventManager = EventManager.Instance;
            this.TelephoneViewManager = ViewManager.Instance;
            this.TelephoneLogManager = LogManager.Instance;
            this.TelephoneDeviceManager = DeviceManager.Instance;

            #endregion
        }

        /// <summary>
        /// Register all events and add subscribers
        /// Needs to be called from state machine, because the state machine instance is required
        /// </summary>
        /// <param name="telephoneStateMachine"></param>
        /// <param name="telephoneActivities"></param>
        internal void DoEventMappings(TelephoneStateMachine telephoneStateMachine, TelephoneActivities telephoneActivities)
        {
            // Register all events
            #region Register events
            // Events implemented for use case
            this.TelephoneEventManager.RegisterEvent("TelephoneUiEvent", telephoneActivities);
            this.TelephoneEventManager.RegisterEvent("TelephoneDeviceEvent", telephoneActivities);

            // Framework/Infrastructure event
            this.TelephoneEventManager.RegisterEvent("StateMachineEvent", telephoneStateMachine);
            this.TelephoneEventManager.RegisterEvent("UINotification", this.TelephoneViewManager);
            this.TelephoneEventManager.RegisterEvent("DeviceManagerNotification", this.TelephoneDeviceManager);
            this.TelephoneEventManager.RegisterEvent("EventManagerEvent", this.TelephoneEventManager);
            this.TelephoneEventManager.RegisterEvent("ViewManagerEvent", this.TelephoneViewManager);
            this.TelephoneEventManager.RegisterEvent("DeviceManagerEvent", this.TelephoneDeviceManager);


            #endregion

            #region event mappings

            // logging
            this.TelephoneEventManager.SubscribeEvent("StateMachineEvent", "LogEventHandler", this.TelephoneLogManager);
            this.TelephoneEventManager.SubscribeEvent("UINotification", "LogEventHandler", this.TelephoneLogManager);
            this.TelephoneEventManager.SubscribeEvent("DeviceManagerNotification", "LogEventHandler", this.TelephoneLogManager);
            this.TelephoneEventManager.SubscribeEvent("EventManagerEvent", "LogEventHandler", this.TelephoneLogManager);
            this.TelephoneEventManager.SubscribeEvent("ViewManagerEvent", "LogEventHandler", this.TelephoneLogManager);
            this.TelephoneEventManager.SubscribeEvent("DeviceManagerEvent", "LogEventHandler", this.TelephoneLogManager);

            // Notifications/Triggers
            this.TelephoneEventManager.SubscribeEvent("UINotification", "InternalNotificationHandler", telephoneStateMachine);
            this.TelephoneEventManager.SubscribeEvent("DeviceManagerNotification", "InternalNotificationHandler", telephoneStateMachine);

            // System event listeners in managers
            this.TelephoneEventManager.SubscribeEvent("TelephoneUiEvent", "ViewCommandHandler", this.TelephoneViewManager);
            this.TelephoneEventManager.SubscribeEvent("TelephoneDeviceEvent", "DeviceCommandHandler", this.TelephoneDeviceManager);
            this.TelephoneEventManager.SubscribeEvent("StateMachineEvent", "SystemEventHandler", this.TelephoneViewManager);
            this.TelephoneEventManager.SubscribeEvent("StateMachineEvent", "SystemEventHandler", this.TelephoneDeviceManager);
            //Sends UI button clickes to device manager
            this.TelephoneEventManager.SubscribeEvent("ViewManagerEvent", "DeviceCommandHandler", this.TelephoneDeviceManager);
            
            
            #endregion
        }
    }
}
