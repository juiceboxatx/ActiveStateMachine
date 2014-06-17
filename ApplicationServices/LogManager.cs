using Common;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationServices
{
    public class LogManager
    {
        #region singleton implementation
        // Create a thread-safe singleton with lazy initialization
        private static readonly Lazy<LogManager> logger = new Lazy<LogManager>(() => new LogManager());

        public static LogManager Instance { get { return logger.Value; } }

        private LogManager()
        {

        }
        #endregion

        /// <summary>
        /// Log infos to dubug window
        /// </summary>
        /// <param name="sender">"sender"</param>
        /// <param name="args">"args"</param>
        public void LogEventHandler(object sender, StateMachineEventArgs args)
        {
            // Log system events
            if (args.EventType != StateMachineEventType.Notification)
            {
                Debug.Print(args.TimeStamp + 
                    " SystemEvent: " + args.EventName + 
                    " - Info: " + args.EventInfo + 
                    " - StateMachineArgumentType: " + args.EventType + 
                    " - Source: " + args.Source + 
                    " - Target: " + args.Target); ;
            }
            else
            {
                // Log state machine notifications
                Debug.Print(args.TimeStamp + 
                    " Notification: " + args.EventName + 
                    " - Info: " + args.EventInfo + 
                    " - StateMachineArgumentType: " + args.EventType + 
                    " - Source: " + args.Source + 
                    " - Target: " + args.Target); ;
            }
        }
    }
}
