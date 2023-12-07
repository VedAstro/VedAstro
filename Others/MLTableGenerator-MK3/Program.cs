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
            Console.WriteLine("\nSTEP 2:", Color.Yellow);
            Console.WriteLine("Processing file...\n");

            int totalTicks = 10;
            var options = new ProgressBarOptions
            {
                ProgressCharacter = '─',
                ProgressBarOnBottom = true
            };
            using (var pbar = new ProgressBar(totalTicks, "initial message", options))
            {

                //get file as binary
                pbar.Tick($"Reading file... {1} of {totalTicks}");
                var inputedFile = File.OpenRead(sourceFilePath);

                pbar.Tick($"Parsing Time Column... {2} of {totalTicks}");
                var foundRawTimeList = Tools.ExtractTimeColumnFromExcel(inputedFile).Result;

                pbar.Tick($"Parsing Location Column... {3} of {totalTicks}");
                var foundGeoLocationList = Tools.ExtractLocationColumnFromExcel(inputedFile).Result;

                //3 : COMBINE DATA
                pbar.Tick($"Combining Time & Location... {4} of {totalTicks}");
                var returnList = foundRawTimeList.Select(dateTimeOffset => new Time(dateTimeOffset, foundGeoLocationList[foundRawTimeList.IndexOf(dateTimeOffset)])).ToList();


            }


            Console.ReadLine();
        }
    }
}
