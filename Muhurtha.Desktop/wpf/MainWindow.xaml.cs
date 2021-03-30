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

        private void CalculateEventsButton_Click(object sender, RoutedEventArgs e) => MainGrid.EventOptions.CalculateEventsButton_Click(sender, e);
        private void Window_Closed(object sender, EventArgs e) => MainGrid.Window_Closed(sender, e);
        private void Window_Initialized(object sender, RoutedEventArgs e) => MainGrid.Window_Initialized(sender, e);
        private void SendToCalendarButton_Click(object sender, RoutedEventArgs e) => MainGrid.EventOptions.SendToCalendarButton_Click(sender, e);
        private void CancelCalculateEventsButton_Click(object sender, RoutedEventArgs e) => MainGrid.EventsCalculatingMessageBox.CancelCalculateEventsButton_Click(sender, e);
    }
}
