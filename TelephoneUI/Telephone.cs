using ApplicationServices;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TelephoneStateMachine;

namespace TelephoneUI
{
    public partial class Telephone : Form, IUserInterface
    {
        private TelephoneStateMachine.TelephoneStateMachine telephoneStateMachine;
        private ViewManager viewMan;
        private TelephoneViewStateConfiguration viewStateConfiguration;
        private DeviceManager devMan;
        private TelephoneDeviceConfiguration devManConfiguation;

        public TelephoneViewState CurrentViewState { get; private set; }

        public Telephone()
        {
            InitializeComponent();
        }

        private void Telephone_Load(object sender, System.EventArgs e)
        {
            
        }

        public void LoadViewState(string viewState)
        {
            var telephoneViewState = (TelephoneViewState)viewMan.ViewStateConfiguration.ViewStates[viewState];
            this.SetValues(telephoneViewState);
            this.CurrentViewState = telephoneViewState;
        }

        private void SetValues(object viewState)
        {
            var telephoneViewState = (TelephoneViewState)viewState;

            // Bell
            if (telephoneViewState.Bell)
            {
                // thread safe windows form property update
                this.Invoke(new MethodInvoker(() => this.labelBellValue.Text = "Ringing"));
                this.Invoke(new MethodInvoker(() => this.labelBellValue.BackColor = Color.Red));
            }
            else
            {
                this.Invoke(new MethodInvoker(() => this.labelBellValue.Text = "Silent"));
                this.Invoke(new MethodInvoker(() => this.labelBellValue.BackColor = Color.Green));
            }

            // Line
            if (telephoneViewState.Line)
            {
                // thread safe windows form property update
                this.Invoke(new MethodInvoker(() => this.labelBellValue.Text = "Active"));
                this.Invoke(new MethodInvoker(() => this.labelBellValue.BackColor = Color.Red));
            }
            else
            {
                this.Invoke(new MethodInvoker(() => this.labelBellValue.Text = "Off"));
                this.Invoke(new MethodInvoker(() => this.labelBellValue.BackColor = Color.Green));
            }

            // Receiver
            if (telephoneViewState.ReceiverHungUp)
            {
                // thread safe windows form property update
                this.Invoke(new MethodInvoker(() => this.labelBellValue.Text = "Down"));
                this.Invoke(new MethodInvoker(() => this.labelBellValue.BackColor = Color.Green));
            }
            else
            {
                this.Invoke(new MethodInvoker(() => this.labelBellValue.Text = "Lifted"));
                this.Invoke(new MethodInvoker(() => this.labelBellValue.BackColor = Color.Red));
            }

            // current view state
            this.Invoke(new MethodInvoker(() => this.labelCurrentViewState.Text = telephoneViewState.Name));
        }

        private void bttn_ReceiverDown_Click(object sender, EventArgs e)
        {
            this.viewMan.RaiseUICommand("OnReceiverDown", "Receiver Hang Up button pressed in view state:" + this.CurrentViewState.Name, "UI", "Receiver");
        }

        private void bttn_ReceiverLifted_Click(object sender, EventArgs e)
        {
            this.viewMan.RaiseUICommand("OnReceiverUp", "Receiver Lift button pressed in view state:" + this.CurrentViewState.Name, "UI", "Receiver");
        }

        private void bttn_Call_Click(object sender, EventArgs e)
        {
            this.viewMan.RaiseUICommand("ActiveExternal", "Initiate Call button pressed in view state:" + this.CurrentViewState.Name, "UI", "Receiver");
        }
    }
}
