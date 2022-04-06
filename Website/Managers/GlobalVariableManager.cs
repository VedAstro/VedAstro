using Microsoft.AspNetCore.Components;
using Website.Pages;
using Website.Shared;

namespace Website
{

    /// <summary>
    /// Place to store global data accessible by all
    /// </summary>
    public class GlobalVariableManager
    {
        public LoadingMessage LoadingMessage { get; set; }


        /// <summary>
        /// This instance is referencing the AlertMessage located in MainLayout
        /// </summary>
        public AlertMessage Alert { get; set; }

        /// <summary>
        /// The current time in the system
        /// </summary>
        public DateTimeOffset SystemTimeNow => DateTimeOffset.Now;

        /// <summary>
        /// Hold the control till components have been loaded
        /// Poll time 10ms
        /// </summary>
        public async Task WaitTillComponentReady()
        {
            while (!this.LoadingMessage.PageReady)
            {
                Console.WriteLine("WaitForLoadingComplete");
                await Task.Delay(10);
            }

        }
    }
}
