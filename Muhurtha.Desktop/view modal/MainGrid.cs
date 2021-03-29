using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Muhurtha.Desktop
{
    /// <summary>
    /// The view modal of the Main Window that contains all the rest of view modals
    /// All manipulation to underlying XML is done through this modal
    /// </summary>
    public class MainGrid : INotifyPropertyChanged
    {
        /** BACKING FIELDS **/
        private EventView _eventView;
        private EventOptions _eventOptions;

        public event PropertyChangedEventHandler PropertyChanged;
        public event EventHandler WindowInitialized;
        public event EventHandler WindowClosed;


        /// <summary>
        /// Create the modal to store UI data
        /// </summary>
        public MainGrid()
        {
            _eventView = new();
            _eventOptions = new();
        }



        /** PROPERTIES **/

        public EventView EventView => _eventView;
        public EventOptions EventOptions => _eventOptions;



        /** EVENT ROUTING **/
        public void Window_Initialized(object sender, EventArgs eventArgs) => WindowInitialized?.Invoke(sender, eventArgs);
        public void Window_Closed(object sender, EventArgs eventArgs) => WindowClosed?.Invoke(sender, eventArgs);



        /** INotifyPropertyChanged **/

        //[NotifyPropertyChangedInvocator][CallerMemberName]
        protected virtual void onPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

    }
}
