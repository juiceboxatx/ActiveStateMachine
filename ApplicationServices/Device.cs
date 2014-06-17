using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationServices
{
    public abstract class Device
    {
        Action<string, string, string> devEvMethod;

        public string DevName { get; private set; }

        /// <summary>
        /// Constructor taking device name and callback method of device manager
        /// </summary>
        /// <param name="deviceName">deviceName</param>
        /// <param name="eventCallBack">eventCallBack</param>
        public Device(string deviceName, Action<string,string,string> eventCallBack)
        {
            this.DevName = deviceName;
            this.devEvMethod = eventCallBack;
        }

        /// <summary>
        /// Device initialization method - needs to be implemented in derived classes
        /// </summary>
        public abstract void OnInit();

        public void RegisterEventCallback(Action<string,string,string> method)
        {
            this.devEvMethod = method;
        }

        public void DoNotificationCallBack(string Name, string EventInfo, string source)
        {
            this.devEvMethod.Invoke(Name, EventInfo, source);
        }

    }
}
