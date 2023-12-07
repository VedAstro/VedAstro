using System.Windows;

namespace Muhurtha.Desktop
{
    /// <summary>
    /// smoke screen used to mask main content while showing popups
    /// </summary>
    public class SmokeScreen : ViewModal
    {


        /** CTOR **/
        public SmokeScreen()
        {
            //start default hidden
            Visibility = Visibility.Hidden;
        }

    }
}