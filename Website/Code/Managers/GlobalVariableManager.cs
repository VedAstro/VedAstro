using System.Xml.Linq;
using Website.Pages;

namespace Website
{

    /// <summary>
    /// Place to store global data accessible by all
    /// </summary>
    public class GlobalVariableManager
    {
        /// <summary>
        /// Time spent waiting before checking if components are ready
        /// </summary>
        private const int ComponentWaitDelayMs = 10;



        //-----------------------------PROPERTIES


        /// <summary>
        /// LoadingMessage box showed over page when loading, located in MainLayout
        /// </summary>
        public LoadingMessage LoadingMessage { get; set; }



        //-----------------------------METHODS

        /// <summary>
        /// The current time in the system
        /// </summary>
        public DateTimeOffset SystemTimeNow => DateTimeOffset.Now;

        public XElement CachedDasaReport { get; set; }

        /// <summary>
        /// Hold the control till components have been loaded
        /// Poll time 10ms
        /// TODO CAN make as static tool
        /// </summary>
        public async Task WaitTillComponentReady()
        {
            //wait till every component needed is ready
            while (LoadingMessage == null)
            {
                Console.WriteLine("WaitForLoadingComplete");
                await Task.Delay(ComponentWaitDelayMs);
            }
        }




    }
}
