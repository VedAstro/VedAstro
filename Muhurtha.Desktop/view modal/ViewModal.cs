using Genso.Astrology.Library;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using Muhurtha.Desktop.Annotations;

namespace Muhurtha.Desktop
{
    /// <summary>
    /// This is the base class for all view modals, contains essantailai logic common to all viewmodals
    /// </summary>
    public abstract class ViewModal : INotifyPropertyChanged
    {
        /** BACKING FIELDS **/
        private Visibility _visibility;


        /** PRESET STYLING **/
        private static readonly Brush DefaultBorderColor = new SolidColorBrush(Color.FromRgb(170, 170, 170));
        private static readonly Thickness DefaultTextInputThickness = new Thickness(1);

        private static readonly Brush ErrorBorderColor = new SolidColorBrush(Color.FromRgb(255, 0, 0));
        private static readonly Thickness ErrorBorderThickness = new Thickness(2);


        /** EVENTS **/
        public event PropertyChangedEventHandler PropertyChanged;
        public event DependencyPropertyChangedEventHandler IsVisible_Changed;



        /** PROPERTIES **/
        public Visibility Visibility
        {
            get => _visibility;
            set
            {
                _visibility = value;
                OnPropertyChanged(nameof(Visibility));
            }
        }




        /** PUBLIC METHODS **/
        public void Show()
        {
            //check first before setting (stops unnecessary style updates)
            if (this.Visibility != Visibility.Visible) { this.Visibility = Visibility.Visible; }
        }
        public void Hide()
        {
            //check first before setting (stops unnecessary style updates)
            if (this.Visibility != Visibility.Hidden) { this.Visibility = Visibility.Hidden; }
        }



        /** EVENT ROUTING **/

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        public void IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs eventArgs) => IsVisible_Changed.Invoke(sender, eventArgs);
    }
}
