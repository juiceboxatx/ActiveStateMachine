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

        private void Telephone_Load(object sender, EventArgs e)
        {
            // Application Services
            // Initialized view states and view manager
            this.viewStateConfiguration = new TelephoneViewStateConfiguration();
            this.viewMan = ViewManager.Instance;
            this.viewMan.LoadViewStateConfiguration(this.viewStateConfiguration, this);

            // Load devices controlled by state machine into device manager
            this.devManConfiguation = new TelephoneDeviceConfiguration();
            this.devMan = DeviceManager.Instance;
            this.devMan.LoadDeviceConfiguration(this.devManConfiguation);

            // State Machine
            // Initialize state machine and start state machine
            this.telephoneStateMachine = new TelephoneStateMachine.TelephoneStateMachine(new TelephoneStateMachineConfiguration());
            this.telephoneStateMachine.InitStateMachine();
            this.telephoneStateMachine.Start();
        }

        public void LoadViewState(string viewState)
        {
            if (viewState == "CompleteFailure")
            {
                var result = MessageBox.Show("Application failed irreparably and will be shut down.", "Global Error", MessageBoxButtons.OK);
                this.Invoke(new MethodInvoker(this.Close));
                return;
            }
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
                this.Invoke(new MethodInvoker(() => this.labelLineValue.Text = "Active"));
                this.Invoke(new MethodInvoker(() => this.labelLineValue.BackColor = Color.Red));
            }
            else
            {
                this.Invoke(new MethodInvoker(() => this.labelLineValue.Text = "Off"));
                this.Invoke(new MethodInvoker(() => this.labelLineValue.BackColor = Color.Green));
            }

            // Receiver
            if (telephoneViewState.ReceiverHungUp)
            {
                // thread safe windows form property update
                this.Invoke(new MethodInvoker(() => this.labelReceiverValue.Text = "Down"));
                this.Invoke(new MethodInvoker(() => this.labelReceiverValue.BackColor = Color.Green));
            }
            else
            {
                this.Invoke(new MethodInvoker(() => this.labelReceiverValue.Text = "Lifted"));
                this.Invoke(new MethodInvoker(() => this.labelReceiverValue.BackColor = Color.Red));
            }

            // Error handling
            if (telephoneViewState.Name == "ViewErrorPhoneRings")// Error display
            {
                this.Invoke(new MethodInvoker(() => this.labelCurrentViewState.Text = "Bell is broken"));
                this.Invoke(new MethodInvoker(() => this.labelCurrentViewState.BackColor = Color.Red));
            }
            else // current view state
            {
                this.Invoke(new MethodInvoker(() => this.labelCurrentViewState.Text = telephoneViewState.Name));
                this.Invoke(new MethodInvoker(() => this.labelCurrentViewState.BackColor = Color.White));
            }
            
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
            this.viewMan.RaiseUICommand("OnLineExternalActive", "Initiate Call button pressed in view state:" + this.CurrentViewState.Name, "UI", "Receiver");
        }

        
    }
}
