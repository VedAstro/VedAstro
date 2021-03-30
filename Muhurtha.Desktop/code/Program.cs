using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using Genso.Astrology.Library;
using Genso.Astrology.Muhurtha.Core;

namespace Muhurtha.Desktop
{
    public class Program
    {


        //managers to handle data & encapsulated logic
        private GuiManager _guiManager;

        //token used to cancel calculator thread if needed
        private CancellationTokenSource calculatorThreadControl;


        /** CTOR **/

        /// <summary>
        /// Prepare data for the program to run
        /// </summary>
        public Program()
        {
            //create managers
            _guiManager = new GuiManager();

        }





        /** EVENT HANDLERS **/

        //EVENT OPTIONS
        private void CalculateEventsButtonClicked(object sender, EventArgs e)
        {
            //turn on smoke screen
            _guiManager.ShowSmokeScreen();

            //show events being calculated message
            _guiManager.ShowEventsCalculatingMessage();

            //place heavy event calculation on a seperate thread & start it off
            //note: upon completion an event will fire, it's handled elsewhere
            calculatorThreadControl = new CancellationTokenSource(); //placed here so that only initialized when needed
            ThreadPool.QueueUserWorkItem(new WaitCallback(CalculateAndUpdateEvents), calculatorThreadControl.Token);

        }
        private void SendToCalendarButtonClicked(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        //EVENTS CALCULATING MESSAGEBOX
        private void CancelCalculateEventsButtonClicked(object? sender, EventArgs e)
        {
            //turn off smoke screen
            _guiManager.HideSmokeScreen();

            //hide event calculating message
            _guiManager.HideEventsCalculatingMessage();

            //terminate thread
            calculatorThreadControl.Cancel();
            calculatorThreadControl.Dispose();
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
            _guiManager.HideSmokeScreen();

            //hide event calculating message
            _guiManager.HideEventsCalculatingMessage();

        }





        /** PUBLIC METHODS **/
        //runs the program
        public void Run()
        {
            //attach handlers to the GUI events
            AttachEventHandlers();

            //run the gui
            _guiManager.Run();

        }




        /** PRIVATE METHODS **/

        //attaches all event handlers to the from the gui & muhurtha core
        private void AttachEventHandlers()
        {
            //MAIN GRID
            _guiManager.MainGrid.WindowInitialized += WindowInitialized;
            _guiManager.MainGrid.WindowClosed += WindowClosed;

            //EVENT OPTIONS
            _guiManager.MainGrid.EventOptions.CalculateEventsButtonClicked += CalculateEventsButtonClicked;
            _guiManager.MainGrid.EventOptions.SendToCalendarButtonClicked += SendToCalendarButtonClicked;


            //EVENTS CALCULATING MESSAGEBOX
            _guiManager.MainGrid.EventsCalculatingMessageBox.CancelCalculateEventsButtonClicked += CancelCalculateEventsButtonClicked;


            //MUHURTHA CORE
            MuhurthaCore.EventCalculationCompleted += CalculationCompleted;
        }



        private void LoadEventOptionsDefaultValues()
        {
            //load all person, location & tag list into combo box
            _guiManager.MainGrid.EventOptions.PersonList = MuhurthaCore.GetAllPeopleList();
            _guiManager.MainGrid.EventOptions.TagList = MuhurthaCore.GetAllTagList();
            _guiManager.MainGrid.EventOptions.LocationList = MuhurthaCore.GetAllLocationList();

            //set default start & end times
            _guiManager.MainGrid.EventOptions.StartTimeText = "00:00 19/03/2021 +08:00";
            _guiManager.MainGrid.EventOptions.EndTimeText = "23:59 20/03/2021 +08:00";
        }

        private void CalculateAndUpdateEvents(object threadCanceler)
        {
            //get all the needed values
            var startTime = _guiManager.MainGrid.EventOptions.StartTimeText;
            var endTime = _guiManager.MainGrid.EventOptions.EndTimeText;
            var location = _guiManager.MainGrid.EventOptions.SelectedLocation;
            var person = _guiManager.MainGrid.EventOptions.SelectedPerson;
            var tag = _guiManager.MainGrid.EventOptions.SelectedTag;

            //pass thread canceler MuhurthaCore, so that methods inside can be stopped if needed
            MuhurthaCore.threadCanceler = (CancellationToken)threadCanceler;

            //calculate events from values
            var events = MuhurthaCore.GetEvents(startTime, endTime, location, person, tag);

            //set event into view
            _guiManager.MainGrid.EventView.EventList = events;
        }


    }
}
