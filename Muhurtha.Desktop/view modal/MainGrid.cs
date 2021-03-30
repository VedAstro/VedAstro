using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

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
        private Visibility _smokeScreenVisibility = Visibility.Hidden; //default hidden
        private EventsCalculatingMessageBox _eventsCalculatingMessageBox;


        /** EVENTS **/
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
            _eventsCalculatingMessageBox = new();
        }



        /** PROPERTIES **/

        public EventView EventView => _eventView;
        public EventOptions EventOptions => _eventOptions;
        public EventsCalculatingMessageBox EventsCalculatingMessageBox => _eventsCalculatingMessageBox;

        //smoke screen used to mask main content while showing other windows
        public Visibility SmokeScreenVisibility
        {
            get => _smokeScreenVisibility;
            set
            {
                _smokeScreenVisibility = value;
                PropertyChanged(this, new PropertyChangedEventArgs("SmokeScreenVisibility"));
            }
        }



        /** EVENT ROUTING **/
        public void Window_Initialized(object sender, EventArgs eventArgs) => WindowInitialized?.Invoke(sender, eventArgs);
        public void Window_Closed(object sender, EventArgs eventArgs) => WindowClosed?.Invoke(sender, eventArgs);



        /** PUBLIC METHODS ROUTING **/




        /** INotifyPropertyChanged **/
        protected virtual void onPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

    }
}
