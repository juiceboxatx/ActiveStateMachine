using ApplicationServices;
using System;

namespace TelephoneStateMachine
{
    public class DeviceBell : Device
    {
        public DeviceBell(string name, Action<string, string, string> eventCallBack)
            : base(name, eventCallBack)
        {
        }

        public bool Ringing { get; set; }

        public override void OnInit()
        {
            this.Ringing = false;
        }

        public void Rings()
        {
            try
            {
                // Sample Errors
                // Catastrophic error stopping the system
                //throw (new SystemException("System device completely failed - Fatal hardware error!"));
                //throw (new SystemException("OnBellBroken"));

                // Normal operation
                this.Ringing = true;
                System.Media.SystemSounds.Hand.Play();
            }
            catch (Exception ex)
            {
                if (ex.Message == "OnBellBroken")
                {
                    this.DoNotificationCallBack("OnBellBroken", ex.Message, "Bell");
                }
                else
                {
                    this.DoNotificationCallBack("CompleteFailure", ex.Message, "Bell");
                }
            }
        }

        public void Silent()
        {
            this.Ringing = false;
        }
    }
}