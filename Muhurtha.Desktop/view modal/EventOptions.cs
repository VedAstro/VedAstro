using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Genso.Astrology.Library;

namespace Muhurtha.Desktop
{
    public class EventOptions : INotifyPropertyChanged
    {
        /** BACKING FIELDS **/
        private Visibility _visibility;
        private string _startTimeText;
        private string _endTimeText;
        private Brush _newSubDomainBorderColor = DefaultBorderColor; //set defaults
        private Thickness _newSubDomainBorderThickness = DefaultTextInputThickness; //set defaults
        private ComboBoxItem _selectedPerson;
        private List<Person> _personList;


        /** PRESET STYLING **/
        private static readonly Brush DefaultBorderColor = new SolidColorBrush(Color.FromRgb(170, 170, 170));
        private static readonly Thickness DefaultTextInputThickness = new Thickness(1);

        private static readonly Brush ErrorBorderColor = new SolidColorBrush(Color.FromRgb(255, 0, 0));
        private static readonly Thickness ErrorBorderThickness = new Thickness(2);
        private List<EventTag> _tagList;
        private ComboBoxItem _selectedTag;
        private List<GeoLocation> _locationList;
        private ComboBoxItem _selectedLocation;


        /** EVENTS **/
        public event EventHandler CalculateEventsButtonClicked;
        public event EventHandler CancelButtonClicked;
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
        public string StartTimeText
        {
            get => _startTimeText;
            set
            {
                _startTimeText = value;
                PropertyChanged(this, new PropertyChangedEventArgs("StartTimeText"));
            }
        }
        public string EndTimeText
        {
            get => _endTimeText;
            set
            {
                _endTimeText = value;
                PropertyChanged(this, new PropertyChangedEventArgs("EndTimeText"));
            }
        }
        public ComboBoxItem SelectedPerson
        {
            get => _selectedPerson;
            set
            {
                _selectedPerson = value;
                PropertyChanged(this, new PropertyChangedEventArgs("SelectedPerson"));
            }
        }
        public ComboBoxItem SelectedLocation
        {
            get => _selectedLocation;
            set
            {
                _selectedLocation = value;
                PropertyChanged(this, new PropertyChangedEventArgs("SelectedLocation"));
            }
        }
        public ComboBoxItem SelectedTag
        {
            get => _selectedTag;
            set
            {
                _selectedTag = value;
                PropertyChanged(this, new PropertyChangedEventArgs("SelectedTag"));
            }
        }
        public List<Person> PersonList
        {
            get => _personList;
            set
            {
                _personList = value;
                //todo might need to observable collection test needed
                PropertyChanged(this, new PropertyChangedEventArgs("PersonList"));
            }
        }
        public List<EventTag> TagList
        {
            get => _tagList;
            set
            {
                _tagList = value;
                //todo might need to observable collection test needed
                PropertyChanged(this, new PropertyChangedEventArgs("TagList"));
            }
        }
        public List<GeoLocation> LocationList
        {
            get => _locationList;
            set
            {
                _locationList = value;
                PropertyChanged(this, new PropertyChangedEventArgs("LocationList"));
            }
        }

        public Brush NewSubDomainBorderColor
        {
            get => _newSubDomainBorderColor;
            set
            {
                _newSubDomainBorderColor = value;
                PropertyChanged(this, new PropertyChangedEventArgs("NewSubDomainBorderColor"));
            }
        }
        public Thickness NewSubDomainBorderThickness
        {
            get => _newSubDomainBorderThickness;
            set
            {
                _newSubDomainBorderThickness = value;
                PropertyChanged(this, new PropertyChangedEventArgs("NewSubDomainBorderThickness"));
            }
        }



        /** PUBLIC METHODS **/
        public void setDefaultStyling()
        {
            //name shortning
            var color = DefaultBorderColor;
            var thick = DefaultTextInputThickness;


            //set default value only if not default (stops unnecessary style updates)
            if (NewSubDomainBorderColor != color) { NewSubDomainBorderColor = color; }
            if (NewSubDomainBorderThickness != thick) { NewSubDomainBorderThickness = thick; }
        }

        /// <summary>
        /// Shows domain not available error
        /// </summary>
        public void DomainNotAvailableError()
        {
            NewSubDomainBorderColor = ErrorBorderColor;
            NewSubDomainBorderThickness = ErrorBorderThickness;
        }



        /** EVENT ROUTING **/
        public void CalculateEventsButton_Click(object sender, RoutedEventArgs routedEventArgs) => CalculateEventsButtonClicked?.Invoke(sender, routedEventArgs);
        public void CancelButton_OnClick(object sender, RoutedEventArgs routedEventArgs) => CancelButtonClicked?.Invoke(sender, routedEventArgs);

    }
}
