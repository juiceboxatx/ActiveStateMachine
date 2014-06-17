using Common;
using System;
using System.Collections.Generic;

namespace ApplicationServices
{
    public class EventManager
    {
        // Create a thread-safe singleton with lazy intitialization
        private static readonly Lazy<EventManager> eventManager = new Lazy<EventManager>(() => new EventManager());

        // Collection of registered Events
        private Dictionary<string, object> EventList;

        private EventManager()
        {
            this.EventList = new Dictionary<string, object>();
        }

        // Event manager event is used for logging
        public event EventHandler<StateMachineEventArgs> EventManagerEvent;
        public static EventManager Instance { get { return eventManager.Value; } }
        /// <summary>
        /// Registration of an event used in the system
        /// </summary>
        /// <param name="eventName"></param>
        /// <param name="source"></param>
        public void RegisterEvent(string eventName, object source)
        {
            this.EventList.Add(eventName, source);
        }

        /// <summary>
        /// Subscritption method maps handler method in a sink object to an event of the source object.
        /// Method signatures between delegate and handler need to match.
        /// </summary>
        /// <param name="eventName"></param>
        /// <param name="handlerMethodName"></param>
        /// <param name="sink"></param>
        /// <returns></returns>
        public bool SubscribeEvent(string eventName, string handlerMethodName, object sink)
        {
            try
            {
                // Get event from list
                var evt = this.EventList[eventName];

                //Determine meta data from event and handler
                var eventInfo = evt.GetType().GetEvent(eventName);
                var methodInfo = sink.GetType().GetMethod(handlerMethodName);

                // Create new delegate mapping event to handler
                Delegate handler = Delegate.CreateDelegate(eventInfo.EventHandlerType, sink, methodInfo);
                eventInfo.AddEventHandler(evt, handler);
                return true;
            }
            catch (Exception ex)
            {
                // Log Failure.
                var message = "Exception while subscribing to handler. Event:" + eventName +
                    " - Handler: " + handlerMethodName + " Exception: " + ex.ToString();
                this.RaiseEventManagerEvent("EventManagerSystemEvent", message, StateMachineEventType.System);
                return false;
            }
        }

        private void RaiseEventManagerEvent(string eventName, string eventInfo, StateMachineEventType eventType)
        {
            var evtMgrEvt = this.EventManagerEvent;
            if (evtMgrEvt != null)
            {
                var newArgs = new StateMachineEventArgs(eventName, eventInfo, eventType, "Event Manager");
                evtMgrEvt(this, newArgs);
            }
        }
    }
}