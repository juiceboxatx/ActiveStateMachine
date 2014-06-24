using ApplicationServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TelephoneUI
{
    public class TelephoneViewStateConfiguration : IViewStateConfiguration
    {
        // List of available view states
        public Dictionary<string, object> ViewStates { get; set; }

        // Plain array holding just the names of the view states, to be used in view manager
        public string[] ViewStateList { get; set; }

        public string DefaultViewState
        {
            get
            {
                foreach (var item in this.ViewStates.Values.Cast<TelephoneViewState>().Where(item => item.IsDefaultViewState))
                {
                    return item.Name;
                }
                throw new Exception("Missing default view state");
            }
        }

        public TelephoneViewStateConfiguration()
        {
            this.InitViewStates();
        }

        private void InitViewStates()
        {
            // Create new view states and add them to the dictionary
            this.ViewStates = new Dictionary<string, object>
            {
                {"ViewPhoneIdle", new TelephoneViewState("ViewPhoneIdle",false,false,true,true)},
                {"ViewPhoneRings", new TelephoneViewState("ViewPhoneRings",true,false,true)},
                {"ViewTalking", new TelephoneViewState("ViewTalking",false,true,false)},
            };

            this.ViewStateList = this.ViewStates.Keys.ToArray(); 
        }
    }
}
