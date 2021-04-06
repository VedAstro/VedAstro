using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Genso.Astrology.Library;

namespace Horoscope.Desktop
{

    public class EventsCalculatingMessageBox : ViewModal
    {
        /** BACKING FIELDS **/
        private string _messageText;

        /** EVENTS **/
        public event EventHandler CancelCalculateEventsButtonClicked;


        /** CTOR **/
        //defaults are set here
        public EventsCalculatingMessageBox()
        {
            _messageText = "Calculating events...";
            this.Hide(); //default hidden
        }


        /** PROPERTIES **/
        public string MessageText
        {
            get => _messageText;
            set
            {
                _messageText = value;
                OnPropertyChanged(nameof(MessageText));
            }
        }



        /** PUBLIC METHODS **/


        /** EVENT ROUTING **/
        public void CancelCalculateEventsButton_Click(object sender, RoutedEventArgs routedEventArgs) => CancelCalculateEventsButtonClicked?.Invoke(sender, routedEventArgs);
    }


}
