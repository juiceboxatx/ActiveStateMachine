using Common;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ActiveStateMachine
{
    public enum EngineState
    {
        Running,
        Stopped,
        Paused,
        Initialized
    }

    public class ActiveStateMachine
    {
        private readonly State initialState;

        private Task queueWorkerTask;

        private ManualResetEvent resumer;

        private CancellationTokenSource tokenSource;

        public ActiveStateMachine(Dictionary<string, State> stateList, int queueCapacity)
        {
            // configure state machine
            this.StateList = stateList;

            // Everything needs to start somewhere - the initial state
            this.initialState = new State("InitialState", null, null, null);

            // Collection taking all triggers. It is thread-safe and blocking as well as FIFO
            // Limiting its capacity protects against DOS like errors or attacks
            this.TriggerQueue = new BlockingCollection<string>(queueCapacity);

            // Initialize
            this.InitStateMachine();

            // Raise an event
            this.RaiseStateMachineSystemEvent("StateMachine: Initialized", "System ready to start");
            this.StateMachineEngine = EngineState.Initialized;
        }

        public event EventHandler<StateMachineEventArgs> StateMachineEvent;

        public State CurrentState { get; private set; }

        public State PreviousState { get; private set; }

        public Dictionary<string, State> StateList { get; private set; }

        public EngineState StateMachineEngine { get; private set; }

        public BlockingCollection<string> TriggerQueue { get; private set; }

        public void Pause()
        {
            // Set engine state
            this.StateMachineEngine = EngineState.Paused;
            this.resumer.Reset();
            this.RaiseStateMachineSystemEvent("StateMachine: Paused", "System waiting.");
        }

        public void Resume()
        {
            this.resumer.Set();
            // Set engine state
            this.StateMachineEngine = EngineState.Running;
            this.RaiseStateMachineSystemEvent("StateMachine: Resumed", "System running.");
        }

        public void Start()
        {
            // Create cencellation token for QueueWorker method
            this.tokenSource = new CancellationTokenSource();

            // Create a new worker thread, if it does not exist.
            this.queueWorkerTask = Task.Factory.StartNew(QueueWorkerMethod, this.tokenSource, TaskCreationOptions.LongRunning);

            // Set engine state
            this.StateMachineEngine = EngineState.Running;
            this.RaiseStateMachineSystemEvent("StateMachine: Started", "System running.");
        }

        public void Stop()
        {
            // Cancel Processing
            this.tokenSource.Cancel();

            // Wait for thread to return
            this.queueWorkerTask.Wait();

            // Free resources
            this.queueWorkerTask.Dispose();

            // Set engine state
            this.StateMachineEngine = EngineState.Stopped;
            this.RaiseStateMachineSystemEvent("StateMachine: Stopped", "System execution stopped.");
        }

        private void EnterTrigger(string newTrigger)
        {
            // Put trigger in queue
            try
            {
                this.TriggerQueue.Add(newTrigger);
            }
            catch (Exception ex)
            {
                this.RaiseStateMachineSystemEvent("ActiveStateMacnine - Error entering trigger", newTrigger + " - " + ex.ToString());
            }

            // Raise an event
            this.RaiseStateMachineSystemEvent("ActiveStateMachine - Trigger entered", newTrigger);
        }

        private void ExecuteTransition(Transition transition)
        {
            // Default checking, if this is a valid transaction
            if (this.CurrentState.StateName != transition.SourceStateName)
            {
                string message = string.Format("Transition has wrong source state {0}, when system is in {1}",
                                    transition.SourceStateName, this.CurrentState.StateName);
                this.RaiseStateMachineSystemEvent("State machine: Default guard execute transition.", message);
                return;
            }
            if (!this.StateList.ContainsKey(transition.TargetStateName))
            {
                string message = string.Format("Transition has wrong target state {0}, when system is in {1}. State not in global State List",
                                    transition.SourceStateName, this.CurrentState.StateName);
                this.RaiseStateMachineSystemEvent("State machine: Default guard execute transition.", message);
                return;
            }

            // Run all exit actions of teh old state
            this.CurrentState.ExitActions.ForEach(a => a.Execute());

            // Run all guars of the transition
            transition.GuardList.ForEach(g => g.Execute());
            string info = transition.GuardList.Count + " guard actions executed!";
            this.RaiseStateMachineSystemEvent("State machine: ExcuteTransition", info);

            // Run all actions of the transition
            transition.TransitionActionList.ForEach(t => t.Execute());

            ////////////////
            // State change
            ////////////////
            info = transition.TransitionActionList.Count + " transition actions executed.";
            this.RaiseStateMachineSystemEvent("State machine: Begin state change.", info);

            // First resolve the target state with the help of its name
            var targetState = this.GetStateFromStateList(transition.TargetStateName);

            // Transition succesfsul - Change state
            this.PreviousState = this.CurrentState;
            this.CurrentState = targetState;

            // run all entry actions of the new state
            foreach (var entryAction in this.CurrentState.EntryActions)
            {
                entryAction.Execute();
            }

            this.RaiseStateMachineSystemEvent("State machine: State change completed successfully.", "Previous state: " +
                                     this.PreviousState.StateName + " - New state = " + this.CurrentState.StateName);
        }

        private State GetStateFromStateList(string targetStateName)
        {
            return this.StateList[targetStateName];
        }

        public void InitStateMachine()
        {
            // Set previous state to an unspecific initial state. The intial state never will be used during normal operation.
            this.PreviousState = this.initialState;

            // Look for the default state, which is the state to begin with in StateList
            foreach (var state in this.StateList)
            {
                if (state.Value.IsDefaultState)
                {
                    this.CurrentState = state.Value;
                    this.RaiseStateMachineSystemEvent("OnInit", "StateMachineInitialized");
                }
            }

            // This is the synchronization object for resuming - passing true means non-blocking (signaled)
            this.resumer = new ManualResetEvent(true);
        }

        private void QueueWorkerMethod(object arg)
        {
            // Blocks execution until it is reset. Used to pause the state machine
            this.resumer.WaitOne();

            // Block the queue and loop through all the triggers available. Blocking queue guarantees FIFO and the GetConsumingEnumerable
            // automatically removes triggers from the queue
            try
            {
                foreach (var trigger in this.TriggerQueue.GetConsumingEnumerable())
                {
                    if (this.tokenSource.IsCancellationRequested)
                    {
                        this.RaiseStateMachineSystemEvent("State machine: QueueWorker", "Processing Canceled");
                        return;
                    }
                    // Compare Trigger
                    foreach (var transition in this.CurrentState.StateTransitionList.Where(
                                    t => trigger == t.Value.Trigger))
                    {
                        this.ExecuteTransition(transition.Value);
                    }

                    
                }

                // do not place any code here because it will not be executed!
                // The foreach loop keeps spinning on the queue until thread is canceled
            }
            catch (Exception ex)
            {
                this.RaiseStateMachineSystemEvent("State machine queueWorker", "Processing canceled! Exception: " + ex.ToString());
                // Create a new queue worker task. The previous one is completing right now
                this.Start();
            }
        }

        private void RaiseStateMachineSystemCommand(string eventName, string eventInfo)
        {
            var stateEvent = this.StateMachineEvent;
            if (stateEvent != null)
            {
                stateEvent(this, new StateMachineEventArgs(eventName, eventInfo, StateMachineEventType.Command, "State machine"));
            }
        }

        private void RaiseStateMachineSystemEvent(string eventName, string eventInfo)
        {
            var stateEvent = this.StateMachineEvent;
            if (stateEvent != null)
            {
                stateEvent(this, new StateMachineEventArgs(eventName, eventInfo, StateMachineEventType.System, "State machine"));
            }
        }

        public void InternalNotificationHandler(object sender, StateMachineEventArgs intArgs)
        {
            this.EnterTrigger(intArgs.EventName);
        }
    }
}