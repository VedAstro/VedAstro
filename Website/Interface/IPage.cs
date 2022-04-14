//using Microsoft.AspNetCore.Components;

//namespace Website
//{
//    public class IPage : ComponentBase
//    {
//        /// <summary>
//        /// Fires when page/component is ready for viewing
//        /// </summary>
//        public Action? OnPageReady;

//        /// <summary>
//        /// Fires when page/component has left ready stage
//        /// and now is busy processing something
//        /// </summary>
//        public Action? OnPageBusy;

//        private bool _pageReady;




//        //--------------------CTOR

//        public IPage()
//        {
//            Console.WriteLine("IPage:CTOR");
//            //this call goes to the local method first
//            AttachEventHandlers();
//        }



//        protected sealed override async Task OnInitializedAsync()
//        {
//            //show loading box before page starts
//            PageReady = false;

//            //call implemented method if exist
//            await OnInitializedAsyncExtra();
//        }

//        /// <summary>
//        /// Hide the loading message that was open in init
//        /// </summary>
//        protected sealed override async Task OnAfterRenderAsync(bool firstRender)
//        {
//            Console.WriteLine("IPage:OnAfterRenderAsync");

//            await OnAfterRenderAsyncExtra(firstRender);

//            //hide the loading message that was open in init
//            if (firstRender) { PageReady = true; }
//        }


//        /// <summary>
//        /// Use this to set if page ready or busy,
//        /// this will call event handlers accordingly
//        /// </summary>
//        public bool PageReady
//        {
//            get => _pageReady;
//            set
//            {
//                //if same value ignore to avoid loops
//                //todo causes first change to not propagate
//                //if (_pageReady == value) { return; }

//                //call the event handlers accordingly
//                if (value) { OnPageReady?.Invoke(); }
//                else { OnPageBusy?.Invoke(); }

//                //save the input value
//                _pageReady = value;
//            }
//        }

//        /// <summary>
//        /// Attaches event handlers
//        /// If you override this method make sure call the base implementation
//        /// </summary>
//        public async Task AttachEventHandlers()
//        {
//            Console.WriteLine("IPage:AttachEventHandlers");

//            OnPageBusy += () => Console.WriteLine("Fired:PageOnBusy");
//            OnPageReady += () => Console.WriteLine("Fired:OnPageReady");

//            //attach extra handlers implemented by 
//            AttachExtraEventHandlers();
//        }


//        /// <summary>
//        /// Will be called from base after normal event handlers are attached
//        /// </summary>
//        protected virtual void AttachExtraEventHandlers()
//        {
//            Console.WriteLine("IPage:AttachExtraEventHandlers");

//            // does nothing unless sub-class overrides
//        }

//        /// <summary>
//        /// Will be called from base's OnAfterRenderAsync
//        /// </summary>
//        protected virtual Task OnAfterRenderAsyncExtra(bool firstRender)
//        {
//            Console.WriteLine("IPage:AttachExtraEventHandlers");

//            // does nothing unless sub-class overrides

//            return Task.CompletedTask;
//        }

//        protected virtual async Task OnInitializedAsyncExtra()
//        {
//            Console.WriteLine("IPage:OnInitializedAsyncExtra");
//        }


//    }
//}



