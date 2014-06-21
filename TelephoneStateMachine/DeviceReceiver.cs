using ApplicationServices;
using System;

namespace TelephoneStateMachine
{
    public class DeviceReceiver : Device
    {
        public DeviceReceiver(string deviceName, Action<string, string, string> eventCallBack)
            : base(deviceName, eventCallBack)
        {
        }

        public bool ReceiverLifter { get; set; }

        public override void OnInit()
        {
            this.ReceiverLifter = false;
        }

        public void OnReceiverDown()
        {
            this.ReceiverLifter = false;
            this.DoNotificationCallBack("OnReceiverDown", "Receiver down", "Receiver");
        }

        public void OnReceiverUp()
        {
            this.ReceiverLifter = true;
            this.DoNotificationCallBack("OnReceiverUp", "Receiver lifted", "Receiver");
        }
    }
}