using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Genso.Astrology.Muhurtha.GUI
{
    /// <summary>
    /// This class encapsulates the whole GUI,
    /// it exposes the Window view modal & basic GUI functionality
    /// The Program does all things GUI via this class
    /// </summary>
    public class GuiManager
    {
        //keep a record of the previous & currently visible panel
        private string _previousPanel = "";
        private string _currentPanel = "";
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
        public void ShowLoginPanel() => SwitchPanels("Login");
        public void ShowHomePanel() => SwitchPanels("Home");
        public void ShowAddDomainPanel() => SwitchPanels("AddDomain");
        public void ShowMessagePanel() => SwitchPanels("Message");
        public void ShowSignUpPanel() => SwitchPanels("SignUp");
        public void HideAllPanel()
        {
            //set hidden value only if visible (stops unnecessary style updates)
            if (MainGrid.LoginPanel.Visibility != Visibility.Hidden) { MainGrid.LoginPanel.Visibility = Visibility.Hidden; }
            if (MainGrid.HomePanel.Visibility != Visibility.Hidden) { MainGrid.HomePanel.Visibility = Visibility.Hidden; }
            if (MainGrid.MessagePanel.Visibility != Visibility.Hidden) { MainGrid.MessagePanel.Visibility = Visibility.Hidden; }
            if (MainGrid.SignUpPanel.Visibility != Visibility.Hidden) { MainGrid.SignUpPanel.Visibility = Visibility.Hidden; }
            if (MainGrid.AddDomainPanel.Visibility != Visibility.Hidden) { MainGrid.AddDomainPanel.Visibility = Visibility.Hidden; }
        }
        /// <summary>
        /// Sets the status message for the GUI
        /// </summary>
        public void SetStatus(string msg) => MainGrid.StatusPanel.StatusText = msg;
        /// <summary>
        /// Shows full screen message to user, on message panel
        /// </summary>
        public void ShowMessage(string msg)
        {
            MainGrid.MessagePanel.MessageText = msg;
            ShowMessagePanel();
        }





        /** PRIVATE METHODS **/

        /// <summary>
        /// Switches the visibility between panels, only 1 panel is visible at a time
        /// </summary>
        private void SwitchPanels(string panelToShow)
        {
            //hide all panels
            HideAllPanel();

            //when ever switching panels reset status panel message
            MainGrid.StatusPanel.resetPanel();

            //make a record of the current panel
            setCurrentPanel(panelToShow);

            //make the requested panel visible
            switch (panelToShow)
            {
                case "Home":
                    MainGrid.HomePanel.Visibility = Visibility.Visible; break;
                case "Login":
                    MainGrid.LoginPanel.Visibility = Visibility.Visible; break;
                case "Message":
                    MainGrid.MessagePanel.Visibility = Visibility.Visible; break;
                case "SignUp":
                    MainGrid.SignUpPanel.Visibility = Visibility.Visible; break;
                case "AddDomain":
                    MainGrid.AddDomainPanel.Visibility = Visibility.Visible; break;
            }
        }

        /// <summary>
        /// Saves the name of the currently visible panel,
        /// moves panel before that into previous panel name
        /// </summary>
        private void setCurrentPanel(string newPanel)
        {
            //move previous panel name to previous record
            _previousPanel = _currentPanel;

            //save new panel name to current record
            _currentPanel = newPanel;
        }

        /// <summary>
        /// Makes previously visible panel visible again
        /// </summary>
        public void ShowPreviousPanel() => SwitchPanels(_previousPanel);
    }
}
