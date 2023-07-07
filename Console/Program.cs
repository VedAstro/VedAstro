using System.Configuration;
using API;
using Microsoft.Extensions.Configuration;
using VedAstro.Library;

namespace VedAstro.Console
{
    internal class Program
    {

        private static IConfigurationRoot _config;


        static async Task Main(string[] args)
        {
            //make instance to store variables from input nicely
            var thisInstance = await Program.CreateInstance();
        }

        public static async Task<Program> CreateInstance()
        {
            var app = new Program();

            try
            {
                System.Console.WriteLine("-- VedAstro Console -- MK1");



            //load the solution path
            //app._solutionPath = AskSolutionPath();
            //app._webProjectPath = Path.Combine(app._solutionPath, "Website");
            //app._apiProjectPath = Path.Combine(app._solutionPath, "API");
            Begin:

                //System.Console.WriteLine($"Project Path : {app._solutionPath}");

                System.Console.WriteLine("Choose wisely");
                System.Console.WriteLine("1:Find Birth Time - Life Predictor - Person");
                System.Console.WriteLine("2:Life Predictor - 100 Years - Super HD - 50MB");
                System.Console.WriteLine("3:...COMING SOON");
                System.Console.WriteLine("4:...UNDER DEVELOPMENT");

                //get selection from user
                var choiceRaw = System.Console.ReadLine();

                //start processing! (WARNING HEAVY COMPUTE)
                await ProcessControl(app, choiceRaw);

                //keep the app going in loop
                goto Begin;
            }
            catch (Exception e) { System.Console.WriteLine(e); }


            //hold your horses
            System.Console.ReadLine();

            return app;


        }

        private static async Task ProcessControl(Program app, string choice)
        {
            var failMessage = () =>
            {
                System.Console.WriteLine("TASK HALTED! ERROR");
                System.Console.ReadLine(); //hold control
            };

            //init config to get app secrets, like upload string
            _config = new ConfigurationBuilder().AddUserSecrets<Program>().Build();

            //load secrets, also makes compatible with azure API code
            Environment.SetEnvironmentVariable("API_STORAGE", _config["API_STORAGE"]);


            var result = new Dictionary<string, bool>();
            switch (choice)
            {
                //note master is beta
                case "1":
                    {
                        System.Console.WriteLine("\t\t Find Birth Time - Life Predictor - Person");
                        System.Console.WriteLine("*****************************************************");

                        //get person id from user
                        var personId = GetInputFromUser("Enter Person ID");

                        await FindBirthTimeEventsChartPerson(personId);


                        break;
                    }
                default: System.Console.WriteLine("Coming soon"); break;
            }

            System.Console.WriteLine("\t\t Done! You can go fly kites now.");
            System.Console.WriteLine("*****************************************************");
        }

        private static string GetInputFromUser(string inputAskMessage)
        {
            System.Console.WriteLine(inputAskMessage);
            var returnAnswer = System.Console.ReadLine();
            return returnAnswer;

        }

        private static async Task FindBirthTimeEventsChartPerson(string personId)
        {

            //ACT 1 : Generate data needed to make charts
            //get person specified by caller
            var foundPerson = await APITools.GetPersonById(personId);

            //generate the needed charts
            var chartList = new List<EventsChart>();
            var eventTags = new List<EventTag> { EventTag.PD1, EventTag.PD2, EventTag.PD3, EventTag.PD4, EventTag.PD5, EventTag.Gochara };

            //time range is preset to full life 100 years from birth
            var start = foundPerson.BirthTime;
            var end = foundPerson.BirthTime.AddYears(100);
            var timeRange = new TimeRange(start, end);

            //calculate based on max screen width,
            var daysPerPixel = EventsChart.GetDayPerPixel(timeRange, 1500);

            //get list of possible birth time slice in the current birth day
            var possibleTimeList = BirthTimeFinderAPI.GetTimeSlicesOnBirthDay(foundPerson, 1);


            //ACT 3 : Generate charts (Parallel)

            var combinedSvg = "";
            var chartYPosition = 30; //start with top padding
            var leftPadding = 10;
            foreach (var possibleTime in possibleTimeList)
            {
                //replace original birth time
                var personAdjusted = foundPerson.ChangeBirthTime(possibleTime);
                var newChart = await EventsChartAPI.GenerateNewChart(personAdjusted, timeRange, daysPerPixel, eventTags);
                var adjustedBirth = personAdjusted.BirthTimeString;

                //place in group with time above the chart
                var wrappedChart = $@"
                            <g transform=""matrix(1, 0, 0, 1, {leftPadding}, {chartYPosition})"">
                                <text style=""font-size: 16px; white-space: pre-wrap;"" x=""2"" y=""-6.727"">{adjustedBirth}</text>
                                {newChart.ContentSvg}
                              </g>
                            ";

                //combine charts together
                combinedSvg += wrappedChart;

                //next chart goes below this one
                //todo get actual chart height for dynamic stacking
                chartYPosition += 390;
            }

            //put all charts in 1 big container
            var finalSvg = EventsChartManager.WrapSvgElements(
                svgClass: "MultipleDasa",
                combinedSvgString: combinedSvg,
                svgWidth: 800,
                svgTotalHeight: chartYPosition,
                randomId: Tools.GenerateId(),
                svgBackgroundColor: "#757575"); //grey easy on the eyes


            //ACT 2 : Save to file
            //This is the part that could not be done in cloud, cost and time
            //recognizable name for file
            var fileName = $"BirthTimeFinder-EventsChart-{foundPerson.Name}-{foundPerson.BirthYear}.svg";

            //save to desktop, easy to spot
            string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            string fullPath = Path.Combine(desktopPath, fileName);
            
            //save to desktop
            await File.WriteAllTextAsync(fullPath, finalSvg);

            //let user know
            System.Console.WriteLine("File Done & Saved to Desktop!");
        }
    }
}