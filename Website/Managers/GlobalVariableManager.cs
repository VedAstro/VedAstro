using System.Xml.Linq;
using Genso.Astrology.Library;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using Website.Pages;
using Website.Shared;

namespace Website
{

    /// <summary>
    /// Place to store global data accessible by all
    /// </summary>
    public class GlobalVariableManager
    {


        //-----------------------------FIELDS

        /// <summary>
        /// Time spent waiting before checking if components are ready
        /// </summary>
        private const int ComponentWaitDelayMs = 10;

        /// <summary>
        /// Hard coded max width used in pages 
        /// </summary>
        public const string MaxWidth = "693px";


        //-----------------------------PROPERTIES

        /// <summary>
        /// Alert box showed at top of page, located in MainLayout
        /// </summary>
        public AlertMessage Alert { get; set; }

        /// <summary>
        /// LoadingMessage box showed over page when loading, located in MainLayout
        /// </summary>
        public LoadingMessage LoadingMessage { get; set; }



        //-----------------------------METHODS

        /// <summary>
        /// The current time in the system
        /// </summary>
        public DateTimeOffset SystemTimeNow => DateTimeOffset.Now;

        /// <summary>
        /// Hold the control till components have been loaded
        /// Poll time 10ms
        /// TODO CAN make as static tool
        /// </summary>
        public async Task WaitTillComponentReady()
        {
            //wait till every component needed is ready
            while (LoadingMessage == null && Alert == null)
            {
                Console.WriteLine("WaitForLoadingComplete");
                await Task.Delay(ComponentWaitDelayMs);
            }
        }




    }
}
