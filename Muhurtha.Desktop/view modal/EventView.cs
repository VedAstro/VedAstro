using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Genso.Astrology.Library;

namespace Muhurtha.Desktop
{


    public class EventView : INotifyPropertyChanged
    {
        /** BACKING FIELDS **/
        private Visibility _visibility;
        private Brush _newSubDomainBorderColor = DefaultBorderColor; //set defaults
        private Thickness _newSubDomainBorderThickness = DefaultTextInputThickness; //set defaults
        private List<Event> _eventList;
        private ComboBoxItem _selectedEvent;


        /** PRESET STYLING **/
        private static readonly Brush DefaultBorderColor = new SolidColorBrush(Color.FromRgb(170, 170, 170));
        private static readonly Thickness DefaultTextInputThickness = new Thickness(1);

        private static readonly Brush ErrorBorderColor = new SolidColorBrush(Color.FromRgb(255, 0, 0));
        private static readonly Thickness ErrorBorderThickness = new Thickness(2);



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
        public ComboBoxItem SelectedEvent
        {
            get => _selectedEvent;
            set
            {
                _selectedEvent = value;
                PropertyChanged(this, new PropertyChangedEventArgs("SelectedEvent"));
            }
        }
        public List<Event> EventList
        {
            get => _eventList;
            set
            {
                _eventList = value;
                //todo might need to observable collection test needed
                PropertyChanged(this, new PropertyChangedEventArgs("EventList"));
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
