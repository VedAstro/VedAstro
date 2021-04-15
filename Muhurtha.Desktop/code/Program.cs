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
using Genso.Framework;
using System.Media;
using Microsoft.Win32;

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
            //check if options choosen by user is ok
            //if not it tells user which options is wrong, and returns false
            //end here if validation failed
            if (!isEventOptionsValid()) { return; }

            //turn on smoke screen
            gui.MainGrid.SmokeScreen.Show();

            //show events being calculated message
            gui.MainGrid.EventsCalculatingPopup.Show();

            //place heavy event calculation on a seperate thread & start it off
            //note: upon completion an event will fire, it's handled elsewhere
            calculatorThreadControl = new CancellationTokenSource(); //placed here so that only initialized when needed
            ThreadPool.QueueUserWorkItem(new WaitCallback(CalculateAndUpdateEvents), calculatorThreadControl.Token);


            //------------------------FUNTIONS------------------------------------
            //checks if all the event options is valid, returns true
            //also raises the error to user here
            bool isEventOptionsValid()
            {
                bool isValid = true; //default is valid

                //check date time
                var startTime = gui.MainGrid.ViewEventOptions.StartTimeText;
                isValid = isStartTimeValid(startTime);
                if (!isValid) { gui.ShowPopupMessage("Start time not correct!"); return false; }

                //check date time
                var endTime = gui.MainGrid.ViewEventOptions.EndTimeText;
                isValid = isEndTimeValid(endTime);
                if (!isValid) { gui.ShowPopupMessage("End time not correct!"); return false; }


                //check each combo box
                isValid = !EqualityComparer<GeoLocation>.Default.Equals(gui.MainGrid.ViewEventOptions.SelectedLocation, default(GeoLocation));
                if (!isValid) { gui.ShowPopupMessage("Please select a location!"); return false; }

                isValid = !EqualityComparer<Person>.Default.Equals(gui.MainGrid.ViewEventOptions.SelectedPerson, default(Person));
                if (!isValid) { gui.ShowPopupMessage("Please select a person!"); return false; }

                //todo for now disabled since EventTag is an enum & does not have null/default value
                //isValid = !EqualityComparer<EventTag>.Default.Equals(gui.MainGrid.EventOptions.SelectedTag, default(EventTag));
                //if (!isValid) { gui.ShowPopupMessage("Please select a tag!"); return false; }


                return isValid;

                //--------------------FUNCTIONS-----------------
                bool isStartTimeValid(string startTime)
                {
                    //todo not yet implemented
                    return true;
                }

                bool isEndTimeValid(string endTime)
                {
                    //todo not yet implemented
                    return true;
                }
            }

        }
        private void SendToCalendarButtonClicked(object sender, EventArgs e)
        {
            //turn on smoke screen
            gui.MainGrid.SmokeScreen.Show();

            //open send to calendar dialog
            gui.MainGrid.SendToCalendarPopup.Show();

        }

        //FIND EVENT OPTIONS
        private void FindEventsButtonClicked(object sender, EventArgs e)
        {
            //check if options choosen by user is ok
            //if not it tells user which options is wrong, and returns false
            //end here if validation failed
            if (!isFindEventOptionsValid()) { return; }

            //turn on smoke screen
            gui.MainGrid.SmokeScreen.Show();

            //show events being calculated message
            gui.MainGrid.EventsCalculatingPopup.Show();

            //place heavy event calculation on a seperate thread & start it off
            //note: upon completion an event will fire, it's handled elsewhere
            calculatorThreadControl = new CancellationTokenSource(); //placed here so that only initialized when needed
            ThreadPool.QueueUserWorkItem(new WaitCallback(FindAndUpdateEvents), calculatorThreadControl.Token);


            //------------------------FUNTIONS------------------------------------
            //checks if all the event options is valid, returns true
            //also raises the error to user here
            bool isFindEventOptionsValid()
            {
                bool isValid = true; //default is valid

                //check date time
                var startTime = gui.MainGrid.FindEventOptions.StartTimeText;
                isValid = isStartTimeValid(startTime);
                if (!isValid) { gui.ShowPopupMessage("Start time not correct!"); return false; }

                //check date time
                var endTime = gui.MainGrid.FindEventOptions.EndTimeText;
                isValid = isEndTimeValid(endTime);
                if (!isValid) { gui.ShowPopupMessage("End time not correct!"); return false; }


                //check each combo box
                isValid = !EqualityComparer<GeoLocation>.Default.Equals(gui.MainGrid.FindEventOptions.SelectedLocation, default(GeoLocation));
                if (!isValid) { gui.ShowPopupMessage("Please select a location!"); return false; }

                isValid = !EqualityComparer<Person>.Default.Equals(gui.MainGrid.FindEventOptions.SelectedPerson, default(Person));
                if (!isValid) { gui.ShowPopupMessage("Please select a person!"); return false; }

                //todo for now disabled since EventTag is an enum & does not have null/default value
                //isValid = !EqualityComparer<EventTag>.Default.Equals(gui.MainGrid.EventOptions.SelectedTag, default(EventTag));
                //if (!isValid) { gui.ShowPopupMessage("Please select a tag!"); return false; }


                return isValid;

                //--------------------FUNCTIONS-----------------
                bool isStartTimeValid(string startTime)
                {
                    //todo not yet implemented
                    return true;
                }

                bool isEndTimeValid(string endTime)
                {
                    //todo not yet implemented
                    return true;
                }
            }

        }


        //EVENTS CALCULATING POPUP
        private void CancelCalculateEventsButtonClicked(object sender, EventArgs e)
        {
            //turn off smoke screen
            gui.MainGrid.SmokeScreen.Hide();

            //hide event calculating message
            gui.MainGrid.EventsCalculatingPopup.Hide();

            //terminate thread
            calculatorThreadControl.Cancel();
            calculatorThreadControl.Dispose();
        }

        //SENDING PROGESS EVENTS POPUP
        private void CancelSendingEventsButtonClicked(object sender, EventArgs e)
        {
            //turn off smoke screen
            gui.MainGrid.SmokeScreen.Hide();
            //hide event calculating message
            gui.MainGrid.SendingEventsPopup.Hide();

            //terminate thread
            calculatorThreadControl.Cancel();
            calculatorThreadControl.Dispose();

        }


        //SEND TO CALENDAR POPUP
        //popup becomes visible
        private void SendToCalendarPopupOnIsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            //if becoming visible then, load calendar accounts into dropdown
            if (gui.MainGrid.SendToCalendarPopup.Visibility == Visibility.Visible)
            {
                //get all calendar account names
                var allAccounts = MuhurthaCore.GetAllCalendarAccounts();

                //before setting account list, disable handler listening for account list changes
                gui.MainGrid.SendToCalendarPopup.AccountSelectionChanged -= AccountSelectionChanged;

                gui.MainGrid.SendToCalendarPopup.AccountList = allAccounts;

                //enable handler back
                gui.MainGrid.SendToCalendarPopup.AccountSelectionChanged += AccountSelectionChanged;

            }

        }
        private void SendEventsButtonClicked(object sender, EventArgs e)
        {
            //hide send events dialog box
            gui.MainGrid.SendToCalendarPopup.Hide();

            //show sending events message box
            gui.MainGrid.SendingEventsPopup.Show();


            //place time consuming sending on a seperate thread & start it off
            //note: upon completion an event will fire, it's handled elsewhere
            calculatorThreadControl = new CancellationTokenSource(); //placed here so that only initialized when needed
            ThreadPool.QueueUserWorkItem(new WaitCallback(SendEventsToCalendar), calculatorThreadControl.Token);
        }
        //just close the box, not yet start sending
        private void CancelSendEventsButtonClicked(object sender, EventArgs e)
        {
            //turn off smoke screen
            gui.MainGrid.SmokeScreen.Hide();

            //hide event send fvents dialog box
            gui.MainGrid.SendToCalendarPopup.Hide();
        }
        private void AccountSelectionChanged(object sender, EventArgs e) => UpdateCalendarListDropdown();
        private void CalendarListOnPreviewMouseLeftButtonDown(object sender, EventArgs e)
        {
            //only update if calendar list is empty
            if (gui.MainGrid.SendToCalendarPopup.CalendarList == null)
            {
                UpdateCalendarListDropdown();
            }
        }

        //MAIN GRID
        //when optional panel selection drop down is changed
        private void MainGridOnOptionsPanelOnSelectionChanged(object sender, EventArgs e) => LoadSelectedOptionsPanel();


        //MAIN WINDOW
        private void WindowInitialized(object sender, EventArgs e)
        {
            //load options panel that is selected
            LoadSelectedOptionsPanel();


        }

        /// <summary>
        /// Closes the program once the main window is closed
        /// </summary>
        private void WindowClosed(object sender, EventArgs e) => System.Environment.Exit(1);


        //MUHURTHA CORE
        //this event is fired when event calculation have finished
        private void CalculationCompleted()
        {
            //play notification sound
            PlayNotificationSound();

            //turn off smoke screen
            gui.MainGrid.SmokeScreen.Hide();

            //hide event calculating message
            gui.MainGrid.EventsCalculatingPopup.Hide();

        }
        private void SendingEventsCompleted()
        {
            //play notification sound
            PlayNotificationSound();

            //turn off smoke screen
            gui.MainGrid.SmokeScreen.Hide();

            //let user know all has been sent
            //gui.MainGrid.SendingEventsMessageBox.MessageText = "Sending event completed!";
            //Thread.Sleep(1500);

            //hide event diolog box
            gui.MainGrid.SendingEventsPopup.Hide();

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



        /** DEFAULT VALUE LOADERS **/
        private void LoadViewEventOptionsDefaultValues()
        {
            //only load defaults if the property is null
            //this allows users to switch views without getting data reset

            //load all person, location & tag list into combo box
            gui.MainGrid.ViewEventOptions.PersonList ??= MuhurthaCore.GetAllPeopleList();
            gui.MainGrid.ViewEventOptions.TagList ??= MuhurthaCore.GetAllTagList();
            gui.MainGrid.ViewEventOptions.LocationList ??= MuhurthaCore.GetAllLocationList();

            //set default start & end times
            var todayStart = DateTime.Today.ToString(Time.GetDateTimeFormat());
            var todayEnd = DateTime.Today.AddHours(23.999).ToString(Time.GetDateTimeFormat());
            gui.MainGrid.ViewEventOptions.StartTimeText ??= todayStart;
            gui.MainGrid.ViewEventOptions.EndTimeText ??= todayEnd;

            //set default combobox option to be none
            //gui.MainGrid.EventOptions.SelectedLocationIndex = -1;
            //gui.MainGrid.EventOptions.SelectedPersonIndex = -1;
            //gui.MainGrid.EventOptions.SelectedTag = ;
        }
        private void LoadFindEventOptionsDefaultValues()
        {
            //only load defaults if the property is null
            //this allows users to switch views without getting data reset

            //load all person, location & tag list into combo box
            gui.MainGrid.FindEventOptions.EventsToFindList ??= MuhurthaCore.GetAllEventDataList();
            gui.MainGrid.FindEventOptions.PersonList ??= MuhurthaCore.GetAllPeopleList();
            gui.MainGrid.FindEventOptions.LocationList ??= MuhurthaCore.GetAllLocationList();

            //set default start & end times to begining and end of the day
            var todayStart = DateTime.Today.ToString(Time.GetDateTimeFormat());
            var todayEnd = DateTime.Today.AddHours(23.999).ToString(Time.GetDateTimeFormat());
            gui.MainGrid.FindEventOptions.StartTimeText ??= todayStart;
            gui.MainGrid.FindEventOptions.EndTimeText ??= todayEnd;

            //set default combobox option to be none
            //gui.MainGrid.EventOptions.SelectedLocationIndex = -1;
            //gui.MainGrid.EventOptions.SelectedPersonIndex = -1;
            //gui.MainGrid.EventOptions.SelectedTag = ;
        }



        /** PRIVATE METHODS **/

        //attaches all event handlers to the from the gui & muhurtha core
        private void AttachEventHandlers()
        {
            //MAIN GRID
            gui.MainGrid.WindowInitialized += WindowInitialized;
            gui.MainGrid.WindowClosed += WindowClosed;
            gui.MainGrid.OptionsPanelOnSelectionChanged += MainGridOnOptionsPanelOnSelectionChanged;

            //EVENT OPTIONS
            gui.MainGrid.ViewEventOptions.CalculateEventsButtonClicked += CalculateEventsButtonClicked;
            gui.MainGrid.ViewEventOptions.SendToCalendarButtonClicked += SendToCalendarButtonClicked;

            //FIND EVENT OPTIONS
            gui.MainGrid.FindEventOptions.FindEventsButtonClicked += FindEventsButtonClicked;


            //SEND TO CALENDAR BOX
            gui.MainGrid.SendToCalendarPopup.SendToCalendarBoxOnIsVisibleChanged += SendToCalendarPopupOnIsVisibleChanged;
            gui.MainGrid.SendToCalendarPopup.CancelSendEventsButtonClicked += CancelSendEventsButtonClicked;
            gui.MainGrid.SendToCalendarPopup.SendEventsButtonClicked += SendEventsButtonClicked;
            gui.MainGrid.SendToCalendarPopup.AccountSelectionChanged += AccountSelectionChanged;
            gui.MainGrid.SendToCalendarPopup.CalendarListOnPreviewMouseLeftButtonDown += CalendarListOnPreviewMouseLeftButtonDown;

            //EVENTS CALCULATING MESSAGEBOX
            gui.MainGrid.EventsCalculatingPopup.CancelCalculateEventsButtonClicked += CancelCalculateEventsButtonClicked;

            //SENDING EVENTS MESSAGE BOX
            gui.MainGrid.SendingEventsPopup.CancelSendingEventsButtonClicked += CancelSendingEventsButtonClicked;


            //MUHURTHA CORE
            MuhurthaCore.EventCalculationCompleted += CalculationCompleted;
            MuhurthaCore.SendingEventsCompleted += SendingEventsCompleted;
        }
        /// <summary>
        /// based on which panel is selected in dropdown, that panel is loaded with deafult values
        /// </summary>
        private void LoadSelectedOptionsPanel()
        {
            //get selected value
            var selectedPanel = gui.MainGrid.SelectedOptionsPanel;

            //show/hide based on which panel is choosen
            var panelName = selectedPanel.Content;

            switch (panelName)
            {
                case "Find Events":
                    LoadFindEventOptionsDefaultValues();
                    gui.MainGrid.FindEventOptions.Show();
                    gui.MainGrid.ViewEventOptions.Hide();
                    gui.MainGrid.LogView.Hide();
                    gui.MainGrid.EventView.Show();

                    break;
                case "View Events":
                    LoadViewEventOptionsDefaultValues();
                    gui.MainGrid.FindEventOptions.Hide();
                    gui.MainGrid.ViewEventOptions.Show();
                    gui.MainGrid.LogView.Hide();
                    gui.MainGrid.EventView.Show();

                    break;
                case "Logs":
                    gui.MainGrid.FindEventOptions.Hide();
                    gui.MainGrid.ViewEventOptions.Hide();
                    gui.MainGrid.LogView.Show();
                    gui.MainGrid.EventView.Hide();
                    break;
                default:
                    //log error if panel not found
                    LogManager.Error($"Panel not accounted for! : {panelName}"); break;
            }


        }
        private void CalculateAndUpdateEvents(object threadCanceler)
        {
            //get all the needed values
            var startTime = gui.MainGrid.ViewEventOptions.StartTimeText;
            var endTime = gui.MainGrid.ViewEventOptions.EndTimeText;
            var location = gui.MainGrid.ViewEventOptions.SelectedLocation;
            var person = gui.MainGrid.ViewEventOptions.SelectedPerson;
            var tag = gui.MainGrid.ViewEventOptions.SelectedTag;


            //pass thread canceler MuhurthaCore, so that methods inside can be stopped if needed
            MuhurthaCore.threadCanceler = (CancellationToken)threadCanceler;

            //calculate events from values
            var events = MuhurthaCore.GetEvents(startTime, endTime, location, person, tag);

            //set event into view
            gui.MainGrid.EventView.EventList = events;



        }
        private void FindAndUpdateEvents(object threadCanceler)
        {
            //get all the needed values
            var startTime = gui.MainGrid.FindEventOptions.StartTimeText;
            var endTime = gui.MainGrid.FindEventOptions.EndTimeText;
            var location = gui.MainGrid.FindEventOptions.SelectedLocation;
            var person = gui.MainGrid.FindEventOptions.SelectedPerson;
            var selectedEvents = gui.MainGrid.FindEventOptions.SelectedEventsToFind;

            //pass thread canceler MuhurthaCore, so that methods inside can be stopped if needed
            MuhurthaCore.threadCanceler = (CancellationToken)threadCanceler;

            //calculate events from values
            var events = MuhurthaCore.FindCombinedEvents(startTime, endTime, location, person, selectedEvents);

            //set event into view
            gui.MainGrid.EventView.EventList = events;

        }
        private void SendEventsToCalendar(object threadCanceler)
        {
            //get name of the selected calendar
            var calendarName = gui.MainGrid.SendToCalendarPopup.SelectedCalendar;
            var customEventName = gui.MainGrid.SendToCalendarPopup.CustomEventName;
            var isSplitEventsChecked = gui.MainGrid.SendToCalendarPopup.IsSplitEventsChecked;
            var isEnableRemindersChecked = gui.MainGrid.SendToCalendarPopup.IsEnableRemindersChecked;

            //get events to send
            var events = gui.MainGrid.EventView.EventList;

            //use thread canceler, so that sending events can be stopped if needed
            MuhurthaCore.threadCanceler = (CancellationToken)threadCanceler;

            //start uploading events to calendar
            MuhurthaCore.SendEventsToCalendar(events, calendarName, CalendarAccount.Google, isSplitEventsChecked, isEnableRemindersChecked, customEventName);

        }
        private void UpdateCalendarListDropdown()
        {
            //get selected account
            var selectedAccount = gui.MainGrid.SendToCalendarPopup.SelectedAccount;

            //get the calendars available for that account
            var calendarList = MuhurthaCore.GetCalendarsForAccount(selectedAccount);

            //place calendars into combobox
            gui.MainGrid.SendToCalendarPopup.CalendarList = calendarList;
        }
        public void PlayNotificationSound()
        {
            bool found = false;
            try
            {
                using (RegistryKey key = Registry.CurrentUser.OpenSubKey(@"AppEvents\Schemes\Apps\.Default\Notification.Default\.Current"))
                {
                    if (key != null)
                    {
                        Object o = key.GetValue(null); // pass null to get (Default)
                        if (o != null)
                        {
                            SoundPlayer theSound = new SoundPlayer((String)o);
                            theSound.Play();
                            found = true;
                        }
                    }
                }
            }
            catch
            { }
            if (!found)
                SystemSounds.Beep.Play(); // consolation prize
        }
    }
}
