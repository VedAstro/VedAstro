using System.Windows.Forms;
using System.Drawing;
using Colorful;
using Console = Colorful.Console;
using VedAstro.Library;
using Parquet.Schema;
using ShellProgressBar;
using ShellProgressBar;
using ProgressBar = ShellProgressBar.ProgressBar;

namespace MLTableGenerator
{
    internal class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            //Console.WriteWithGradient("Hello, World!", Color.Red, Color.Blue, 14);
            //Console.WriteStyled("Hello, World!", new StyleSheet(Color.Yellow));
            //greet message
            //FigletFont font = FigletFont.Load(@"C:\Users\ASUS\Desktop\Projects\VedAstro\MLTableGenerator\banner.flf");
            //Figlet figlet = new Figlet(font);
            //Console.WriteLine(figlet.ToAscii("VedAstro"), Color.Magenta);
            Console.WriteAscii("ML Table Generator", Color.Yellow);
            Console.WriteLine("VedAstro - 2023\n\n", Color.Magenta);


            //# STEP 1
            //ask user to enter path to time source file
            Console.WriteLine("STEP 1:", Color.Yellow);
            Console.WriteLine("Press ENTER, to select Source EXCEL file with list of Time.");
            Console.ReadLine();

            //show GUI and to let user find file
            var dialog = new OpenFileDialog();
            var result = dialog.ShowDialog();
            var sourceFilePath = dialog.FileName;

            //let user know file selected
            Console.WriteLine($"Selected File:\n{sourceFilePath}");

            //# STEP 2
            Console.WriteLine("STEP 2:", Color.Yellow);
            Console.WriteLine("Processing file...");

            int totalTicks = 10;
            var options = new ProgressBarOptions
            {
                ProgressCharacter = '─',
                ProgressBarOnBottom = true
            };
            using (var pbar = new ProgressBar(totalTicks, "initial message", options))
            {
                for (int i = 0; i <= totalTicks; i++)
                {
                    pbar.Tick("Updating... " + i + " of " + totalTicks);
                    // Simulate work being done
                    System.Threading.Thread.Sleep(1000);
                }
            }

            //get file as binary
            var inputedFile = File.OpenRead(sourceFilePath);
            var foundRawTimeList = Tools.ExtractTimeColumnFromExcel(inputedFile).Result;
            var foundGeoLocationList =  Tools.ExtractLocationColumnFromExcel(inputedFile).Result;

            //3 : COMBINE DATA
            var returnList = foundRawTimeList.Select(dateTimeOffset => new Time(dateTimeOffset, foundGeoLocationList[foundRawTimeList.IndexOf(dateTimeOffset)])).ToList();


            Console.ReadLine();
        }
    }
}
