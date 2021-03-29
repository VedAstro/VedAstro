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
        private EventsList _eventsList;
        private EventOptions _eventOptions;

        public event PropertyChangedEventHandler PropertyChanged;
        public event EventHandler WindowInitialized;
        public event EventHandler MainWindow_Closed;


        /// <summary>
        /// Create the modal to store UI data
        /// </summary>
        public MainGrid()
        {
            _eventsList = new();
            _eventOptions = new();
        }



        /** PROPERTIES **/

        public EventsList EventsList => _eventsList;
        public EventOptions EventOptions => _eventOptions;



        /** EVENT ROUTING **/
        public void Window_Initialized(object sender, EventArgs eventArgs) => WindowInitialized?.Invoke(sender, eventArgs);
        public void mainWindow_Closed(object sender, EventArgs eventArgs) => MainWindow_Closed?.Invoke(sender, eventArgs);



        /** INotifyPropertyChanged **/

        //[NotifyPropertyChangedInvocator][CallerMemberName]
        protected virtual void onPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

    }
}
