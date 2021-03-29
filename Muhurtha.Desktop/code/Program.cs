using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
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

            //get all the needed values
            var startTime = _guiManager.MainGrid.EventOptions.StartTimeText;
            var endTime = _guiManager.MainGrid.EventOptions.EndTimeText;
            var location = _guiManager.MainGrid.EventOptions.SelectedLocation;
            var person = _guiManager.MainGrid.EventOptions.SelectedPerson;
            var tag = _guiManager.MainGrid.EventOptions.SelectedTag;

            //get events from values
            var events = MuhurthaCore.GetEvents(startTime, endTime, location, person, tag);

            //set event into view
            _guiManager.MainGrid.EventView.EventList = events;

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

        //attaches all event handlers to the from the gui
        private void AttachEventHandlers()
        {
            //MAIN GRID
            _guiManager.MainGrid.WindowInitialized += WindowInitialized;
            _guiManager.MainGrid.WindowClosed += WindowClosed;

            //LOGIN PANEL
            _guiManager.MainGrid.EventOptions.CalculateEventsButtonClicked += CalculateEventsButtonClicked;

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



    }
}
