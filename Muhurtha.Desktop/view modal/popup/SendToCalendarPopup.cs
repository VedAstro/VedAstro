using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Genso.Astrology.Library.objects.Enum;
using Calendar = Genso.Astrology.Library.Calendar;

namespace Muhurtha.Desktop
{
    /// <summary>
    /// The dialog box used for sending/exporting events to google calendar
    /// </summary>
    public class SendToCalendarPopup : ViewModal
    {

        /** BACKING FIELDS **/
        private CalendarAccount _selectedAccount;
        private List<Calendar> _calendarList;
        private Calendar _selectedCalendar;
        private List<CalendarAccount> _accountList;


        /** EVENTS **/
        public event EventHandler CancelSendEventsButtonClicked;
        public event EventHandler SendEventsButtonClicked;
        public event EventHandler AccountSelectionChanged;
        public event EventHandler CalendarListOnPreviewMouseLeftButtonDown;
        public event DependencyPropertyChangedEventHandler SendToCalendarBoxOnIsVisibleChanged;


        /** CTOR **/
        public SendToCalendarPopup()
        {
            //start default hidden
            Visibility = Visibility.Hidden;
        }



        /** PROPERTIES **/
        public CalendarAccount SelectedAccount
        {
            get => _selectedAccount;
            set
            {
                _selectedAccount = value;
                OnPropertyChanged(nameof(SelectedAccount));
            }
        }

        public List<CalendarAccount> AccountList
        {
            get => _accountList;
            set
            {
                _accountList = value;
                OnPropertyChanged(nameof(AccountList));
            }

        }
        public List<Calendar> CalendarList
        {
            get => _calendarList;
            set
            {
                _calendarList = value;
                OnPropertyChanged(nameof(CalendarList));
            }

        }

        public Calendar SelectedCalendar
        {
            get => _selectedCalendar;
            set
            {
                _selectedCalendar = value;
                OnPropertyChanged(nameof(SelectedCalendar));
            }
        }


        /** EVENT ROUTING **/
        public void CancelSendEventsButton_Click(object sender, RoutedEventArgs routedEventArgs) => CancelSendEventsButtonClicked?.Invoke(sender, routedEventArgs);

        public void SendEventsButton_Click(object sender, RoutedEventArgs routedEventArgs) => SendEventsButtonClicked?.Invoke(sender, routedEventArgs);

        public void Account_SelectionChanged(object sender, SelectionChangedEventArgs selectionChangedEventArgs) => AccountSelectionChanged?.Invoke(sender, selectionChangedEventArgs);

        public void SendToCalendarBox_OnIsVisibleChanged(object sender, DependencyPropertyChangedEventArgs dependencyPropertyChangedEventArgs) => SendToCalendarBoxOnIsVisibleChanged?.Invoke(sender, dependencyPropertyChangedEventArgs);

        public void CalendarList_OnPreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs mouseButtonEventArgs) => CalendarListOnPreviewMouseLeftButtonDown?.Invoke(sender, mouseButtonEventArgs);
    }
}
