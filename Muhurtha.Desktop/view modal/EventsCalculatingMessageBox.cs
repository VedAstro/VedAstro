using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Genso.Astrology.Library;

namespace Muhurtha.Desktop
{

    public class EventsCalculatingMessageBox : INotifyPropertyChanged
    {
        /** BACKING FIELDS **/
        private Visibility _visibility = Visibility.Hidden; //set default as hidden
        private string _messageText = "Calculating events...";


        /** PRESET STYLING **/
        private static readonly Brush DefaultBorderColor = new SolidColorBrush(Color.FromRgb(170, 170, 170));
        private static readonly Thickness DefaultTextInputThickness = new Thickness(1);

        private static readonly Brush ErrorBorderColor = new SolidColorBrush(Color.FromRgb(255, 0, 0));
        private static readonly Thickness ErrorBorderThickness = new Thickness(2);


        /** EVENTS **/
        public event EventHandler CancelCalculateEventsButtonClicked;
        public event PropertyChangedEventHandler PropertyChanged;



        /** PROPERTIES **/
        public Visibility Visibility
        {
            get => _visibility;
            set
            {
                _visibility = value;
                PropertyChanged(this, new PropertyChangedEventArgs("Visibility"));
            }
        }
        public string MessageText
        {
            get => _messageText;
            set
            {
                _messageText = value;
                PropertyChanged(this, new PropertyChangedEventArgs("MessageText"));
            }
        }



        /** PUBLIC METHODS **/


        /** EVENT ROUTING **/
        public void CancelCalculateEventsButton_Click(object sender, RoutedEventArgs routedEventArgs) => CancelCalculateEventsButtonClicked?.Invoke(sender, routedEventArgs);
    }


}
