using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace Muhurtha.Desktop
{
    /// <summary>
    /// The view modal of the Main Window that contains all the rest of view modals
    /// All manipulation to underlying XML is done through this modal
    /// </summary>
    public class MainGrid : ViewModal
    {
        /** BACKING FIELDS **/

        //VIEWS
        private EventView _eventView;
        private LogView _logView;

        //OPTIONS PANEL
        private ViewEventOptions _viewEventOptions;
        private FindEventOptions _findEventOptions;

        //POPUPS
        private SendToCalendarPopup _sendToCalendarPopup;
        private SendingEventsPopup _sendingEventsPopup;
        private EventsCalculatingPopup _eventsCalculatingPopup;

        //OTHERS GUI ELEMENTS
        private ComboBoxItem _selectedOptionsPanel; //options panel selector
        private SmokeScreen _smokeScreen; //background smoke when popups are used



        /** EVENTS **/
        public event EventHandler WindowInitialized;
        public event EventHandler WindowClosed;
        public event EventHandler OptionsPanelOnSelectionChanged;


        /// <summary>
        /// Create the modal to store UI data
        /// </summary>
        public MainGrid()
        {
            //creates a new instance for all the view models
            //this also sets any defaults that the view modal implementss
            _eventView = new();
            _logView = new();
            _viewEventOptions = new();
            _findEventOptions = new();
            _eventsCalculatingPopup = new();
            _smokeScreen = new();
            _sendToCalendarPopup = new();
            _sendingEventsPopup = new();
        }



        /** PROPERTIES **/

        public EventView EventView => _eventView;
        public LogView LogView => _logView;
        public ViewEventOptions ViewEventOptions => _viewEventOptions;
        public FindEventOptions FindEventOptions => _findEventOptions;
        public ComboBoxItem SelectedOptionsPanel
        {
            get => _selectedOptionsPanel;
            set
            {
                _selectedOptionsPanel = value;
                OnPropertyChanged(nameof(SelectedOptionsPanel));
            }
        }
        public EventsCalculatingPopup EventsCalculatingPopup => _eventsCalculatingPopup;
        public SmokeScreen SmokeScreen => _smokeScreen;
        public SendToCalendarPopup SendToCalendarPopup => _sendToCalendarPopup;
        public SendingEventsPopup SendingEventsPopup => _sendingEventsPopup;


        /** EVENT ROUTING **/
        //TODO Might need to be moved to mainwindow's viewmodal
        public void Window_Initialized(object sender, EventArgs eventArgs) => WindowInitialized?.Invoke(sender, eventArgs);
        public void Window_Closed(object sender, EventArgs eventArgs) => WindowClosed?.Invoke(sender, eventArgs);
        //fired when user selects to view a different event option panel
        public void OptionsPanel_OnSelectionChanged(object sender, SelectionChangedEventArgs e) => OptionsPanelOnSelectionChanged?.Invoke(sender, e);



        /** PUBLIC METHODS **/


    }
}
