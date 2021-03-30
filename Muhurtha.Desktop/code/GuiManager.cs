using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Muhurtha.Desktop
{
    /// <summary>
    /// This class encapsulates the whole GUI,
    /// it exposes the Window view modal & basic GUI functionality
    /// The Program does all things GUI via this class
    /// </summary>
    public class GuiManager
    {
        private MainWindow _mainWindow;
        private MainGrid _mainGrid;
        public bool isReady = false; //default is not ready



        /** CTOR **/
        public GuiManager()
        {
            //when view is created also create an instance of WPF main window
            _mainWindow = new MainWindow();

            //get the modal from the main window (WPF creates the modal)
            //WPF creates the modal cause it's easier for design time debugging
            _mainGrid = _mainWindow.MainGrid;
        }



        /** PROPERTIES **/

        /// <summary>
        /// The top view modal that holds the all the other view modals
        /// This the view modal of the MainGrid
        /// </summary>
        public MainGrid MainGrid => _mainGrid;




        /** PUBLIC METHODS **/

        /// <summary>
        /// Runs the GUI window, does not return until window is closed
        /// </summary>
        public void Run() => _mainWindow.ShowDialog();




        /** PRIVATE METHODS **/

        public void ShowSmokeScreen()
        {
            //check first before setting (stops unnecessary style updates)
            if (MainGrid.SmokeScreenVisibility != Visibility.Visible) { MainGrid.SmokeScreenVisibility = Visibility.Visible; }
        }
        public void HideSmokeScreen()
        {
            //check first before setting (stops unnecessary style updates)
            if (MainGrid.SmokeScreenVisibility != Visibility.Hidden) { MainGrid.SmokeScreenVisibility = Visibility.Hidden; }
        }
        public void ShowEventsCalculatingMessage()
        {
            //check first before setting (stops unnecessary style updates)
            if (MainGrid.EventsCalculatingMessageBox.Visibility != Visibility.Visible) { MainGrid.EventsCalculatingMessageBox.Visibility = Visibility.Visible; }
        }
        public void HideEventsCalculatingMessage()
        {
            //check first before setting (stops unnecessary style updates)
            if (MainGrid.EventsCalculatingMessageBox.Visibility != Visibility.Hidden) { MainGrid.EventsCalculatingMessageBox.Visibility = Visibility.Hidden; }
        }
    }


}
