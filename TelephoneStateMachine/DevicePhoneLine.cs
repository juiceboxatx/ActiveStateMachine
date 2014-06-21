using ApplicationServices;
using System;

namespace TelephoneStateMachine
{
    public class DevicePhoneLine : Device
    {
        public DevicePhoneLine(string deviceName, Action<string, string, string> eventCallBack)
            : base(deviceName, eventCallBack)
        {
        }

        public bool LineActiveExternal { get; set; }

        public bool LineActiveInternal { get; set; }

        public void ActiveExternal()
        {
            this.LineActiveExternal = true;
            this.DoNotificationCallBack("OnLineExternalActive", "Phone Line set ToString active", this.DevName);
        }

        public void ActiveInternal()
        {
            this.LineActiveInternal = true;
        }

        public void OffInterval()
        {
            this.LineActiveInternal = false;
            System.Media.SystemSounds.Hand.Play();
        }
        public override void OnInit()
        {
            this.LineActiveExternal = false;
            this.LineActiveInternal = false;
        }
    }
}