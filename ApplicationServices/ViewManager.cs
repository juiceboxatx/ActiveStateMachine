using Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationServices
{
	class ViewManager
	{
		private string[] viewStates;
		private string DefaultViewState;
		// UI - make this a Dictionary<string,IUserInterface>, if you have to handle more than one
		private IUserInterface _UI;

		public event EventHandler<StateMachineEventArgs> ViewManagerEvent;
		public string CurrentView { get; private set; }

		public IViewStateConfiguration ViewStateConfiguration { get; private set; }

		#region singleton implementation

		// Create a thread-safe singleton with lazy initialization
		private static readonly Lazy<ViewManager> viewManager = new Lazy<ViewManager>(() => new ViewManager());

		public static ViewManager Instance { get { return viewManager.Value;} }

		private ViewManager()
		{

		}
		#endregion

		public void LoadViewStateConfiguration(IViewStateConfiguration viewStateConfiguration, IUserInterface userInterface)
		{
			this.ViewStateConfiguration = viewStateConfiguration;
			viewStates = viewStateConfiguration.ViewStateList;
			this._UI = userInterface;
			this.DefaultViewState = viewStateConfiguration.DefaultViewState;
		}

		public void ViewCommandHandler(object sender, StateMachineEventArgs args)
		{
			// This approach assumes that there is a dedicated view state for every state machine UI command
			try
			{
				if (this.viewStates.Contains(args.EventName))
				{
					// Convention: view command event names matches corresponding view state
					this._UI.LoadViewState(args.EventName);
					this.CurrentView = args.EventName;
					this.RaiseViewManagerEvent("View Manager Command", "Successfully loaded viewStates state: " + args.EventName);

				}
				else
				{
					this.RaiseViewManagerEvent("View Manager Command", "View state not found!");
				}
			}
			catch (Exception ex)
			{

				this.RaiseViewManagerEvent("View Manager Command - Error", ex.ToString());
			}
		}

        public void SystemEventHandler(object sender, StateMachineEventArgs args)
        {
            // Initialize
            if (args.EventName == "OnInit")
            {
                this._UI.LoadViewState(this.DefaultViewState);
                this.CurrentView = this.DefaultViewState;
            }
        }
        /// <summary>
        /// Method to raise a view manager event for logging, etc.
        /// </summary>
        /// <param name="name">"name"</param>
        /// <param name="info">"info"</param>
        /// <param name="eventType">"eventType"</param>
		private void RaiseViewManagerEvent(string name, string info, StateMachineEventType eventType = StateMachineEventType.System)
		{            
            var vmEventHandler = this.ViewManagerEvent;
            if (vmEventHandler != null)
            {
                var newVMargs = new StateMachineEventArgs(name, "View manager event: " + info, eventType, "View manager");
                vmEventHandler(this, newVMargs);
            }
		}

        /// <summary>
        /// Sends a command to 
        /// </summary>
        /// <param name="command"></param>
        /// <param name="info"></param>
        /// <param name="source"></param>
        /// <param name="target"></param>
        private void RaiseUICommand(string command, string info, string source, string target)
        {
            var vmEventHandler = this.ViewManagerEvent;
            if (vmEventHandler != null)
            {
                var newVMargs = new StateMachineEventArgs(command, info, StateMachineEventType.Command, source, target);
                vmEventHandler(this, newVMargs);
            }
        }
	}
}
