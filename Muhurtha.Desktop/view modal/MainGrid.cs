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
    public class MainGrid : ViewModal
    {
        /** BACKING FIELDS **/
        private EventView _eventView;
        private EventOptions _eventOptions;
        private EventsCalculatingMessageBox _eventsCalculatingMessageBox;
        private SmokeScreen _smokeScreen;
        private SendToCalendarBox _sendToCalendarBox;
        private SendingEventsMessageBox _sendingEventsMessageBox;


        /** EVENTS **/
        public event EventHandler WindowInitialized;
        public event EventHandler WindowClosed;


        /// <summary>
        /// Create the modal to store UI data
        /// </summary>
        public MainGrid()
        {
            //creates a new instance for all the view models
            //this also sets any defaults that the view modal implementss
            _eventView = new();
            _eventOptions = new();
            _eventsCalculatingMessageBox = new();
            _smokeScreen = new();
            _sendToCalendarBox = new();
            _sendingEventsMessageBox = new();
        }



        /** PROPERTIES **/

        public EventView EventView => _eventView;
        public EventOptions EventOptions => _eventOptions;
        public EventsCalculatingMessageBox EventsCalculatingMessageBox => _eventsCalculatingMessageBox;
        public SmokeScreen SmokeScreen => _smokeScreen;
        public SendToCalendarBox SendToCalendarBox => _sendToCalendarBox;
        public SendingEventsMessageBox SendingEventsMessageBox => _sendingEventsMessageBox;



        /** EVENT ROUTING **/
        public void Window_Initialized(object sender, EventArgs eventArgs) => WindowInitialized?.Invoke(sender, eventArgs);
        public void Window_Closed(object sender, EventArgs eventArgs) => WindowClosed?.Invoke(sender, eventArgs);



        /** PUBLIC METHODS **/


    }
}
