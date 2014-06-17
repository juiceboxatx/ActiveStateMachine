using Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace ApplicationServices
{
    public class DeviceManager
    {
        #region singleton implementation

        // Creat a thread-safe signleton with lazy initialization
        private static readonly Lazy<DeviceManager> deviceManager = new Lazy<DeviceManager>(() => new DeviceManager());

        public DeviceManager()
        {
            this.DeviceList = new Dictionary<string, object>();
        }

        public static DeviceManager Instance { get { return deviceManager.Value; } }

        #endregion singleton implementation

        // Device manager event is used for logging
        public event EventHandler<StateMachineEventArgs> DeviceManagerEvent;

        public event EventHandler<StateMachineEventArgs> DeviceManagerNotification;

        /// <summary>
        /// List of system devices
        /// </summary>
        public Dictionary<string, object> DeviceList { get; set; }

        public void AddDevice(string name, object device)
        {
            this.DeviceList.Add(name, device);
            this.RaiseDeviceManagerEvent("Added device", name);
        }

        public void DeviceCommandHandler(DeviceManager deviceManager, StateMachineEventArgs args)
        {
            // Listen to command events only
            if (args.EventType != StateMachineEventType.Command)
            {
                return;
            }

            // Get device and execute command action method
            try
            {
                if (!this.DeviceList.Keys.Contains(args.Target))
                {
                    return;
                }

                // Convention device commands and method names must match!
                var device = this.DeviceList[args.Target];
                MethodInfo deviceMethod = device.GetType().GetMethod(args.EventName);
                deviceMethod.Invoke(device, new object[] { });
                this.RaiseDeviceManagerEvent("DeviceCommand", "Successful device command: " + args.Target + " - " + args.EventInfo);
            }
            catch (Exception ex)
            {
                this.RaiseDeviceManagerEvent("DeviceCommand - Error", ex.ToString());
                throw;
            }
        }

        /// <summary>
        /// Loads device configuration
        /// </summary>
        /// <param name="devManConfiguration"></param>
        public void LoadDeviceConfiguration(IDeviceConfiguration devManConfiguration)
        {
            this.DeviceList = devManConfiguration.Devices;
        }

        public void RaiseDeviceManagerNotification(string command, string info, string source)
        {
            var devManagerEvent = this.DeviceManagerEvent;
            if (devManagerEvent != null)
            {
                var newDMArgs = new StateMachineEventArgs(command, info, StateMachineEventType.Notification, source, "State Machine");
                devManagerEvent(this, newDMArgs);
            }
        }

        public void RemoveDevice(string name, object device)
        {
            this.DeviceList.Remove(name);
            this.RaiseDeviceManagerEvent("Removed device", name);
        }

        /// <summary>
        /// Handler method for special system evnts, e.g. initialization
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="args">args</param>
        public void SystemEventHandler(object sender, StateMachineEventArgs args)
        {
            // Initialize
            if (args.EventName == "OnInit" && args.EventType == StateMachineEventType.Command)
            {
                foreach (var dev in this.DeviceList)
                {
                    try
                    {
                        MethodInfo initMethod = dev.Value.GetType().GetMethod("OnInit");
                        initMethod.Invoke(dev.Value, new object[] { });
                        this.RaiseDeviceManagerEvent("DeviceCommand - Initialization device", dev.Key);
                    }
                    catch (Exception ex)
                    {
                        this.RaiseDeviceManagerEvent("DeviceCommand - Initialization error device" + dev.Key, ex.ToString());
                    }
                }
            }

            // Notification Handler
            // because we use UI to trigger transitions devices would trigger normally themselves.
            // Nevertheless, this is common, if SW user interfacescontrol devices
            // View and device managers communicate on system event bus and use notifications to trigger state machine
            if (args.EventType == StateMachineEventType.Command)
            {
                // check for right condition
                if (args.EventName == "OnInit") return;
                if (!this.DeviceList.ContainsKey(args.Target)) return;

                //Dispatch commmand to device
                this.DeviceCommandHandler(this, args);
                //this.RaiseDeviceManagerNotification(args.EventName, "routed through device manager: " + args.EventInfo, args.Source);
            }
        }

        private void RaiseDeviceManagerEvent(string name, string info)
        {
            var devManagerEvent = this.DeviceManagerEvent;
            if (devManagerEvent != null)
            {
                var newDMArgs = new StateMachineEventArgs(name, "Device manager event: " + info, StateMachineEventType.System, "Device Manager");
                devManagerEvent(this, newDMArgs);
            }
        }
    }
}