using Microsoft.AspNetCore.Components;

namespace Website
{
    public class IPage : ComponentBase
    {
        public Action? OnPageReady;
        //public Func<Task>? OnPageBusy;
        public Action? OnPageBusy;

        //private GlobalVariableManager _globalVariable;
        private bool _pageReady;
        //protected GlobalVariableManager baseGlobalVariable;

        public GlobalVariableManager _globalVariable;

        public IPage()
        {
            Console.WriteLine("IPage:CTOR");
            //this call goes to the overriden method if it exists
            AttachEventHandlers();
        }


        /// <summary>
        /// Use this to set if page ready or busy,
        /// this will call event handlers accordingly
        /// </summary>
        public bool PageReady
        {
            get => _pageReady;
            set
            {
                //if same value ignore to avoid loops
                //todo causes first change to not propagate
                //if (_pageReady == value) { return; }

                //call the event handlers accordingly
                if (value) { OnPageReady?.Invoke(); }
                else { OnPageBusy?.Invoke(); }

                //save the input value
                _pageReady = value;
            }
        }


        public virtual async Task AttachEventHandlers()
        {
            Console.WriteLine("IPage:AttachEventHandlers");

            OnPageBusy += () => Console.WriteLine("Fired:PageOnBusy");
            OnPageReady += () => Console.WriteLine("Fired:OnPageReady");

        }


    }
}

