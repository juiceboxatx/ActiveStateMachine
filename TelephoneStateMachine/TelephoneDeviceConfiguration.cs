using ApplicationServices;
using Common;
using System.Collections.Generic;

namespace TelephoneStateMachine
{
    public class TelephoneDeviceConfiguration : IDeviceConfiguration
    {
        private DeviceManager devMan;

        public TelephoneDeviceConfiguration()
        {
            this.devMan = DeviceManager.Instance;
            this.InitDevices();
        }

        public Dictionary<string, object> Devices { get; set; }

        private void InitDevices()
        {
            var bell = new DeviceBell("Bell", this.devMan.RaiseDeviceManagerNotification);
            var phoneLine = new DevicePhoneLine("PhoneLine", this.devMan.RaiseDeviceManagerNotification);
            var receiver = new DeviceReceiver("Receiver", this.devMan.RaiseDeviceManagerNotification);

            this.Devices = new Dictionary<string, object> { { "Bell", bell }, { "PhoneLine", phoneLine }, { "Receiver", receiver } };
        }
    }
}