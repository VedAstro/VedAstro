using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Muhurtha.Desktop
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        //the grid that holds all stuff
        //frist element inside Window
        public MainGrid MainGrid;


        public MainWindow()
        {
            InitializeComponent();

            //get the view modal of the main grid, which holds all other viewmodals
            //get it here to pass events to it 
            MainGrid = TryFindResource("MainGrid") as MainGrid;

        }



        /** EVENT ROUTING **/

        private void CalculateEventsButton_Click(object sender, RoutedEventArgs e) => MainGrid.ViewEventOptions.CalculateEventsButton_Click(sender, e);
        private void Window_Closed(object sender, EventArgs e) => MainGrid.Window_Closed(sender, e);
        private void Window_Initialized(object sender, RoutedEventArgs e) => MainGrid.Window_Initialized(sender, e);
        private void SendToCalendarButton_Click(object sender, RoutedEventArgs e) => MainGrid.ViewEventOptions.SendToCalendarButton_Click(sender, e);
        private void CancelCalculateEventsButton_Click(object sender, RoutedEventArgs e) => MainGrid.EventsCalculatingPopup.CancelCalculateEventsButton_Click(sender, e);
        private void CancelSendEventsButton_Click(object sender, RoutedEventArgs e) => MainGrid.SendToCalendarPopup.CancelSendEventsButton_Click(sender, e);
        private void SendEventsButton_Click(object sender, RoutedEventArgs e) => MainGrid.SendToCalendarPopup.SendEventsButton_Click(sender, e);
        private void CancelSendingEventsButton_Click(object sender, RoutedEventArgs e) => MainGrid.SendingEventsPopup.CancelSendingEventsButton_Click(sender, e);
        private void Account_SelectionChanged(object sender, SelectionChangedEventArgs e) => MainGrid.SendToCalendarPopup.Account_SelectionChanged(sender, e);
        private void SendToCalendarPopup_OnIsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e) => MainGrid.SendToCalendarPopup.SendToCalendarBox_OnIsVisibleChanged(sender, e);
        private void CalendarList_OnPreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e) => MainGrid.SendToCalendarPopup.CalendarList_OnPreviewMouseLeftButtonDown(sender, e);
        private void OptionsPanel_OnSelectionChanged(object sender, SelectionChangedEventArgs e) => MainGrid?.OptionsPanel_OnSelectionChanged(sender, e); //watch for null, cause fires before viewmodal is ready
        private void LogView_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e) => MainGrid.LogView.IsVisibleChanged(sender, e);
        private void FindEventsButton_Click(object sender, RoutedEventArgs e) => MainGrid.FindEventOptions.FindEventsButton_Click(sender, e);
        private void EventsToFind_SelectionChanged(object sender, SelectionChangedEventArgs e) => MainGrid.FindEventOptions.EventsToFind_SelectionChanged(sender, e);
        private void EventListFilterText_OnTextChanged(object sender, TextChangedEventArgs e) => MainGrid.FindEventOptions.EventListFilterText_OnTextChanged(sender, e);


    }
}
