using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Genso.Astrology.Library;

namespace Horoscope.Desktop
{


    public class EventView : ViewModal
    {
        /** BACKING FIELDS **/
        private List<Prediction> _eventList;
        private ComboBoxItem _selectedEvent;



        /** EVENTS **/
        public event EventHandler CalculateEventsButtonClicked;
        public event EventHandler CancelButtonClicked;



        /** PROPERTIES **/
        //Event that is selected (not yet used), maybe can be used to show more info
        public ComboBoxItem SelectedEvent
        {
            get => _selectedEvent;
            set
            {
                _selectedEvent = value;
                OnPropertyChanged(nameof(SelectedEvent));
            }
        }

        /// <summary>
        /// List of events that have been calculated
        /// 
        /// Note : List is auto sorted by Strength
        /// </summary>
        public List<Prediction> EventList
        {
            get => _eventList;
            set
            {   //sort list by Strength before storing
                _eventList = value.OrderBy(prediction => Double.Parse(prediction.Strength)).ToList();
                OnPropertyChanged(nameof(EventList));
            }
        }


        /** PUBLIC METHODS **/



        /** EVENT ROUTING **/
        public void CalculateEventsButton_Click(object sender, RoutedEventArgs routedEventArgs) => CalculateEventsButtonClicked?.Invoke(sender, routedEventArgs);
        public void CancelButton_OnClick(object sender, RoutedEventArgs routedEventArgs) => CancelButtonClicked?.Invoke(sender, routedEventArgs);

    }


}
