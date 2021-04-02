using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using Genso.Astrology.Library;
using Genso.Astrology.Library.objects.Enum;
using Genso.Astrology.Muhurtha.Core;

namespace Muhurtha.Desktop
{
    public class Program
    {
        /** FIELDS **/

        //managers to handle data & encapsulated logic
        private GuiManager gui;

        //token used to cancel calculator thread if needed
        private CancellationTokenSource calculatorThreadControl;



        /** CTOR **/

        /// <summary>
        /// Prepare data for the program to run
        /// </summary>
        public Program()
        {
            //create managers
            gui = new GuiManager();

        }




        /** EVENT HANDLERS **/

        //EVENT OPTIONS
        private void CalculateEventsButtonClicked(object sender, EventArgs e)
        {
            //turn on smoke screen
            gui.MainGrid.SmokeScreen.Show();

            //show events being calculated message
            gui.MainGrid.EventsCalculatingMessageBox.Show();

            //place heavy event calculation on a seperate thread & start it off
            //note: upon completion an event will fire, it's handled elsewhere
            calculatorThreadControl = new CancellationTokenSource(); //placed here so that only initialized when needed
            ThreadPool.QueueUserWorkItem(new WaitCallback(CalculateAndUpdateEvents), calculatorThreadControl.Token);

        }
        private void SendToCalendarButtonClicked(object sender, EventArgs e)
        {
            //turn on smoke screen
            gui.MainGrid.SmokeScreen.Show();

            //open send to calendar dialog
            gui.MainGrid.SendToCalendarBox.Show();

        }

        //EVENTS CALCULATING MESSAGEBOX
        private void CancelCalculateEventsButtonClicked(object sender, EventArgs e)
        {
            //turn off smoke screen
            gui.MainGrid.SmokeScreen.Hide();

            //hide event calculating message
            gui.MainGrid.EventsCalculatingMessageBox.Hide();

            //terminate thread
            calculatorThreadControl.Cancel();
            calculatorThreadControl.Dispose();
        }

        //SENDING EVENTS MESSAGE BOX
        private void CancelSendingEventsButtonClicked(object sender, EventArgs e)
        {
            //turn off smoke screen
            gui.MainGrid.SmokeScreen.Hide();

            //hide event calculating message
            gui.MainGrid.EventsCalculatingMessageBox.Hide();

            //terminate thread
            calculatorThreadControl.Cancel();
            calculatorThreadControl.Dispose();

        }


        //SEND TO CALENDAR BOX
        private void SendToCalendarBoxOnIsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            //if becoming visible then, load calendar accounts into dropdown
            if (gui.MainGrid.SendToCalendarBox.Visibility == Visibility.Visible)
            {
                //get all calendar account names
                var allAccounts = MuhurthaCore.GetAllCalendarAccounts();

                //before setting account list, disable handler listening for account list changes
                gui.MainGrid.SendToCalendarBox.AccountSelectionChanged -= AccountSelectionChanged;

                gui.MainGrid.SendToCalendarBox.AccountList = allAccounts;

                //enable handler back
                gui.MainGrid.SendToCalendarBox.AccountSelectionChanged += AccountSelectionChanged;

            }

        }
        private void SendEventsButtonClicked(object sender, EventArgs e)
        {
            //hide send events dialog box
            gui.MainGrid.SendToCalendarBox.Hide();

            //show sending events message box
            gui.MainGrid.SendingEventsMessageBox.Show();


            //place time consuming sending on a seperate thread & start it off
            //note: upon completion an event will fire, it's handled elsewhere
            calculatorThreadControl = new CancellationTokenSource(); //placed here so that only initialized when needed
            ThreadPool.QueueUserWorkItem(new WaitCallback(SendEventsToCalendar), calculatorThreadControl.Token);
        }
        private void CancelSendEventsButtonClicked(object sender, EventArgs e)
        {
            //turn off smoke screen
            gui.MainGrid.SmokeScreen.Hide();

            //hide event send fvents dialog box
            gui.MainGrid.SendToCalendarBox.Hide();
        }
        private void AccountSelectionChanged(object sender, EventArgs e) => UpdateCalendarListDropdown();
        private void CalendarListOnPreviewMouseLeftButtonDown(object sender, EventArgs e)
        {
            //only update if calendar list is empty
            if (gui.MainGrid.SendToCalendarBox.CalendarList == null)
            {
                UpdateCalendarListDropdown();
            }
        }


        //MAIN WINDOW
        private void WindowInitialized(object sender, EventArgs e)
        {
            //load values into event options panel
            LoadEventOptionsDefaultValues();


        }

        /// <summary>
        /// Closes the program once the main window is closed
        /// </summary>
        private void WindowClosed(object sender, EventArgs e) => System.Environment.Exit(1);

        //MUHURTHA CORE
        //this event is fired when event calculation have finished
        private void CalculationCompleted()
        {
            //turn off smoke screen
            gui.MainGrid.SmokeScreen.Hide();

            //hide event calculating message
            gui.MainGrid.EventsCalculatingMessageBox.Hide();

        }
        private void SendingEventsCompleted()
        {
            //turn off smoke screen
            gui.MainGrid.SmokeScreen.Hide();

            //let user know all has been sent
            //gui.MainGrid.SendingEventsMessageBox.MessageText = "Sending event completed!";
            //Thread.Sleep(1500);

            //hide event diolog box
            gui.MainGrid.SendingEventsMessageBox.Hide();

        }





        /** PUBLIC METHODS **/
        //runs the program
        public void Run()
        {
            //attach handlers to the GUI events
            AttachEventHandlers();

            //run the gui
            gui.Run();

        }




        /** PRIVATE METHODS **/

        //attaches all event handlers to the from the gui & muhurtha core
        private void AttachEventHandlers()
        {
            //MAIN GRID
            gui.MainGrid.WindowInitialized += WindowInitialized;
            gui.MainGrid.WindowClosed += WindowClosed;

            //EVENT OPTIONS
            gui.MainGrid.EventOptions.CalculateEventsButtonClicked += CalculateEventsButtonClicked;
            gui.MainGrid.EventOptions.SendToCalendarButtonClicked += SendToCalendarButtonClicked;

            //SEND TO CALENDAR BOX
            gui.MainGrid.SendToCalendarBox.SendToCalendarBoxOnIsVisibleChanged += SendToCalendarBoxOnIsVisibleChanged;
            gui.MainGrid.SendToCalendarBox.CancelSendEventsButtonClicked += CancelSendEventsButtonClicked;
            gui.MainGrid.SendToCalendarBox.SendEventsButtonClicked += SendEventsButtonClicked;
            gui.MainGrid.SendToCalendarBox.AccountSelectionChanged += AccountSelectionChanged;
            gui.MainGrid.SendToCalendarBox.CalendarListOnPreviewMouseLeftButtonDown += CalendarListOnPreviewMouseLeftButtonDown;

            //EVENTS CALCULATING MESSAGEBOX
            gui.MainGrid.EventsCalculatingMessageBox.CancelCalculateEventsButtonClicked += CancelCalculateEventsButtonClicked;

            //SENDING EVENTS MESSAGE BOX
            gui.MainGrid.SendingEventsMessageBox.CancelSendingEventsButtonClicked += CancelSendingEventsButtonClicked;


            //MUHURTHA CORE
            MuhurthaCore.EventCalculationCompleted += CalculationCompleted;
            MuhurthaCore.SendingEventsCompleted += SendingEventsCompleted;
        }


        private void LoadEventOptionsDefaultValues()
        {
            //load all person, location & tag list into combo box
            gui.MainGrid.EventOptions.PersonList = MuhurthaCore.GetAllPeopleList();
            gui.MainGrid.EventOptions.TagList = MuhurthaCore.GetAllTagList();
            gui.MainGrid.EventOptions.LocationList = MuhurthaCore.GetAllLocationList();

            //set default start & end times
            gui.MainGrid.EventOptions.StartTimeText = "00:00 19/03/2021 +08:00";
            gui.MainGrid.EventOptions.EndTimeText = "23:59 20/03/2021 +08:00";
        }
        private void CalculateAndUpdateEvents(object threadCanceler)
        {
            //get all the needed values
            var startTime = gui.MainGrid.EventOptions.StartTimeText;
            var endTime = gui.MainGrid.EventOptions.EndTimeText;
            var location = gui.MainGrid.EventOptions.SelectedLocation;
            var person = gui.MainGrid.EventOptions.SelectedPerson;
            var tag = gui.MainGrid.EventOptions.SelectedTag;

            //pass thread canceler MuhurthaCore, so that methods inside can be stopped if needed
            MuhurthaCore.threadCanceler = (CancellationToken)threadCanceler;

            //calculate events from values
            var events = MuhurthaCore.GetEvents(startTime, endTime, location, person, tag);

            //set event into view
            gui.MainGrid.EventView.EventList = events;
        }
        private void SendEventsToCalendar(object threadCanceler)
        {
            //get name of the selected calendar
            var calendarName = gui.MainGrid.SendToCalendarBox.SelectedCalendar;

            //get events to send
            var events = gui.MainGrid.EventView.EventList;

            //pass thread canceler MuhurthaCore, so that methods inside can be stopped if needed
            MuhurthaCore.threadCanceler = (CancellationToken)threadCanceler;

            //start uploading events to calendar
            MuhurthaCore.SendEventsToCalendar(events, calendarName, CalendarAccount.Google, true);

        }

        private void UpdateCalendarListDropdown()
        {
            //get selected account
            var selectedAccount = gui.MainGrid.SendToCalendarBox.SelectedAccount;

            //get the calendars available for that account
            var calendarList = MuhurthaCore.GetCalendarsForAccount(selectedAccount);

            //place calendars into combobox
            gui.MainGrid.SendToCalendarBox.CalendarList = calendarList;
        }

    }
}
