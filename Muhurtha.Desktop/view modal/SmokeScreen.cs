using System.Windows;

namespace Muhurtha.Desktop
{
    /// <summary>
    /// smoke screen used to mask main content while showing other windows
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