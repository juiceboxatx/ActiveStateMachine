using System;

namespace Common
{
    public enum StateMachineEventType
    {
        System,
        Command,
        Notification,
        External
    }

    public class StateMachineEventArgs
    {
        public StateMachineEventArgs(string eventName, string eventInfo, StateMachineEventType eventType, string source, string target = "")
        {
            this.EventName = eventName;
            this.EventInfo = eventInfo;
            this.EventType = eventType;
            this.Source = source;
            this.Target = target;
            this.TimeStamp = DateTime.Now;
        }

        public string EventInfo { get; set; }

        public string EventName { get; set; }
        public StateMachineEventType EventType { get; set; }

        public string Source { get; set; }

        public string Target { get; set; }

        public DateTime TimeStamp { get; set; }
    }
}