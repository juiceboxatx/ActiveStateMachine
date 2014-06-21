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
            this.Ringing = true;
            System.Media.SystemSounds.Hand.Play();
        }

        public void Silent()
        {
            this.Ringing = false;
        }
    }
}