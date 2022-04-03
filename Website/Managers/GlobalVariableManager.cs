using Microsoft.AspNetCore.Components;
using Website.Shared;

namespace Website
{

    /// <summary>
    /// Place to store global data accessible by all
    /// </summary>
    public class GlobalVariableManager
    {

        /// <summary>
        /// This instance is referencing the AlertMessage located in MainLayout
        /// </summary>
        public AlertMessage Alert { get; set; }

        /// <summary>
        /// The current time in the system
        /// </summary>
        public DateTimeOffset SystemTimeNow => DateTimeOffset.Now;

    }
}
