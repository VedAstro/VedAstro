using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Genso.Astrology.Library;

namespace Horoscope.Desktop
{
    public class EventOptions : ViewModal
    {
        /** BACKING FIELDS **/
        private string _startTimeText;
        private string _endTimeText;
        private Person _selectedPerson;
        private EventTag _selectedTag;
        private GeoLocation _selectedLocation;
        private List<Person> _personList;
        private List<EventTag> _tagList;
        private List<GeoLocation> _locationList;





        /** EVENTS **/
        public event EventHandler CalculateEventsButtonClicked;
        public event EventHandler CancelButtonClicked;
        public event EventHandler SendToCalendarButtonClicked;



        /** PROPERTIES **/
        public string StartTimeText
        {
            get => _startTimeText;
            set
            {
                _startTimeText = value;
                OnPropertyChanged(nameof(StartTimeText));
            }
        }
        public string EndTimeText
        {
            get => _endTimeText;
            set
            {
                _endTimeText = value;
                OnPropertyChanged(nameof(EndTimeText));
            }
        }
        public Person SelectedPerson
        {
            get => _selectedPerson;
            set
            {
                _selectedPerson = value;
                OnPropertyChanged(nameof(SelectedPerson));
            }
        }
        public GeoLocation SelectedLocation
        {
            get => _selectedLocation;
            set
            {
                _selectedLocation = value;
                OnPropertyChanged(nameof(SelectedLocation));
            }
        }
        public EventTag SelectedTag
        {
            get => _selectedTag;
            set
            {
                _selectedTag = value;
                OnPropertyChanged(nameof(SelectedTag));
            }
        }
        public List<Person> PersonList
        {
            get => _personList;
            set
            {
                _personList = value;
                OnPropertyChanged(nameof(PersonList));
            }
        }
        public List<EventTag> TagList
        {
            get => _tagList;
            set
            {
                _tagList = value;
                OnPropertyChanged(nameof(TagList));
            }
        }
        public List<GeoLocation> LocationList
        {
            get => _locationList;
            set
            {
                _locationList = value;
                OnPropertyChanged(nameof(LocationList));
            }
        }



        /** PUBLIC METHODS **/



        /** EVENT ROUTING **/
        public void CalculateEventsButton_Click(object sender, RoutedEventArgs routedEventArgs) => CalculateEventsButtonClicked?.Invoke(sender, routedEventArgs);
        public void CancelButton_OnClick(object sender, RoutedEventArgs routedEventArgs) => CancelButtonClicked?.Invoke(sender, routedEventArgs);
        public void SendToCalendarButton_Click(object sender, RoutedEventArgs routedEventArgs) => SendToCalendarButtonClicked?.Invoke(sender, routedEventArgs);
    }
}
