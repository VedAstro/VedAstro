using System;
using System.Windows;
using Genso.Framework;


namespace Muhurtha.Desktop
{
    /// <summary>
    /// Simple class to only start the Program and catch crtical errors
    /// </summary>
    public static class Start
    {
        [STAThread]
        static void Main(string[] args)
        {
            //create the program
            var program = new Program();

            //run it in a loop
            RunAgain:
            try
            {
                //start the program
                program.Run();
            }
            //if error, catch it
            catch (Exception e)
            {

                LogManager.Error(e);

                //show user error
                MessageBox.Show(e.Message, "Unexpected error", MessageBoxButton.OK, MessageBoxImage.Error);

                return;
                //run the program agains
                // goto RunAgain;
            }
        }

    }

}
