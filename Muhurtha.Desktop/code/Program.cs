using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

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

            MessageBox.Show("Test");

        }

        //MAIN WINDOW
        private void WindowInitialized(object sender, EventArgs e)
        {


        }

        /// <summary>
        /// Closes the program once the main window is closed
        /// </summary>
        private void MainGrid_MainWindow_Closed(object sender, EventArgs e) => System.Environment.Exit(1);






        /** PUBLIC METHODS **/
        //starts the GUI
        //and handles the errors gracefuly
        public void Run()
        {
            //attach handlers to the GUI events
            attachEventHandlers();

            //run the gui
            _guiManager.Run();

        }




        /** PRIVATE METHODS **/

        private void attachEventHandlers()
        {
            //MAIN GRID
            _guiManager.MainGrid.WindowInitialized += WindowInitialized;
            _guiManager.MainGrid.MainWindow_Closed += MainGrid_MainWindow_Closed;

            //LOGIN PANEL
            _guiManager.MainGrid.EventOptions.CalculateEventsButtonClicked += CalculateEventsButtonClicked;

        }



    }
}
