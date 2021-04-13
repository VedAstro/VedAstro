using Genso.Framework;

namespace Muhurtha.Desktop
{
    /// <summary>
    /// The place used to show internal logs from log manager
    /// </summary>
    public class LogView : ViewModal
    {
        /** BACKING FIELDS **/
        private string _log;



        /** PROPERTIES **/
        public string Log
        {
            get => _log;
            set
            {
                _log = value;
                OnPropertyChanged(nameof(Log));
            }
        }



        /** PUBLIC METHODS **/

        /// <summary>
        /// Gets latest logs from log manager, and load them into "Log" property which is watched by WPF binding
        /// </summary>
        public void ReloadLogText() => Log = LogManager.LogText;
    }
}