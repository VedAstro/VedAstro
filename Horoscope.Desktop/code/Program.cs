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

namespace Horoscope.Desktop
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

        //HOROSCOPE CORE
        //this event is fired when event calculation have finished
        private void CalculationCompleted()
        {
            //turn off smoke screen
            gui.MainGrid.SmokeScreen.Hide();

            //hide event calculating message
            gui.MainGrid.EventsCalculatingMessageBox.Hide();

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

            //EVENTS CALCULATING MESSAGEBOX
            gui.MainGrid.EventsCalculatingMessageBox.CancelCalculateEventsButtonClicked += CancelCalculateEventsButtonClicked;

            //SENDING EVENTS MESSAGE BOX
            gui.MainGrid.SendingEventsMessageBox.CancelSendingEventsButtonClicked += CancelSendingEventsButtonClicked;


            //HOROSCOPE CORE
            HoroscopeCore.EventCalculationCompleted += CalculationCompleted;

        }


        private void LoadEventOptionsDefaultValues()
        {
            //load all person, location & tag list into combo box
            gui.MainGrid.EventOptions.PersonList = HoroscopeCore.GetAllPeopleList();

            //set default start & end times
            gui.MainGrid.EventOptions.StartTimeText = "00:00 19/03/2021 +08:00";
            gui.MainGrid.EventOptions.EndTimeText = "23:59 20/03/2021 +08:00";
        }
        private void CalculateAndUpdateEvents(object threadCanceler)
        {
            //get all the needed values
            var person = gui.MainGrid.EventOptions.SelectedPerson;
            var tag = gui.MainGrid.EventOptions.SelectedTag;

            //pass thread canceler MuhurthaCore, so that methods inside can be stopped if needed
            HoroscopeCore.threadCanceler = (CancellationToken)threadCanceler;

            //calculate events from values
            var events = HoroscopeCore.GetPrediction(person, tag);

            //set event into view
            gui.MainGrid.EventView.EventList = events;
        }

    }
}
