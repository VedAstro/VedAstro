using Microsoft.Extensions.Configuration;
using System.Xml.Linq;
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
                System.Console.WriteLine("4:...COMING SOON");
                System.Console.WriteLine("5:...UNDER DEVELOPMENT");

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

                        var maxWidth = int.Parse(GetInputFromUser("Enter Max Width in Px"));
                        var precisionInHours = double.Parse(GetInputFromUser("Enter Scan Precision in Hours"));
                        System.Console.WriteLine("Start & End of Chart");
                        var startYear = GetInputFromUser("Enter Start Year : 0001-9999");
                        var endYear = GetInputFromUser("Enter End Year : 0001-9999");
                        System.Console.WriteLine("Start & End of Possible Birth Hour");
                        var startHour = GetInputFromUser("Enter Start 24Hour : 00-24");
                        var endHour = GetInputFromUser("Enter End 24Hour : 00-24");

                        await FindBirthTimeEventsChartPerson(personId, maxWidth, precisionInHours, startYear, endYear, startHour, endHour);

                        break;
                    }
                //case "3":
                //    await MigrateOldLifeEventsToNewFormat2();
                //    break;
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

        private static async Task FindBirthTimeEventsChartPerson(string personId, int maxWidth, double precisionInHours, string startYear, string endYear, string startHour, string endHour)
        {

            //ACT 1 : Generate data needed to make charts
            //get person specified by caller
            var foundPerson = await Tools.GetPersonById(personId);

            //generate the needed charts
            var eventTags = new List<EventTag> { EventTag.PD1, EventTag.PD2, EventTag.PD3, EventTag.PD4,EventTag.PD5, EventTag.Gochara };
            var algorithmFuncsList = new List<AlgorithmFuncs>() { EventsChartManager.Algorithm.GetGeneralScore };
            var summaryOptions = new ChartOptions(algorithmFuncsList);

            //time range is preset to full life 100 years from birth
            var start = new Time($"00:00 01/01/{startYear} {foundPerson.BirthTimeZone}", foundPerson.GetBirthLocation());
            var end = new Time($"00:00 31/12/{endYear} {foundPerson.BirthTimeZone}", foundPerson.GetBirthLocation());
            var timeRange = new TimeRange(start, end);

            //calculate based on max screen width,
            var daysPerPixel = EventsChart.GetDayPerPixel(timeRange, maxWidth);


            //get list of possible birth time slice in the current birth day by inputed hour
            var startHourParsed = new Time($"{startHour}:00 {foundPerson.BirthDateMonthYearOffset}", foundPerson.GetBirthLocation());
            var endHourParsed = new Time($"{endHour}:00 {foundPerson.BirthDateMonthYearOffset}", foundPerson.GetBirthLocation());
            var possibleTimeList = Time.GetTimeListFromRange(startHourParsed, endHourParsed, precisionInHours);

            //ACT 3 : Generate charts (Parallel)

            Dictionary<Time, string> dict = new Dictionary<Time, string>();
            // Adding elements to the Dictionary

            //Parallel.ForEach(possibleTimeList, async possibleTime =>
            //{
            //    var personAdjusted = foundPerson.ChangeBirthTime(possibleTime);

            //    var chart = await EventsChartAPI.GenerateNewChart(personAdjusted, timeRange, daysPerPixel, eventTags);
            //    //var timeHash = possibleTime.ToString(); //time to id the chart, because will complete asymmetrically
            //    dict.Add(possibleTime, chart.ContentSvg);
            //});

            foreach (var possibleTime in possibleTimeList)
            {
                var personAdjusted = foundPerson.ChangeBirthTime(possibleTime);

                var chart = await Tools.GenerateNewChart(personAdjusted, timeRange, daysPerPixel, eventTags, summaryOptions);
                //var timeHash = possibleTime.ToString(); //time to id the chart, because will complete asymmetrically
                dict.Add(possibleTime, chart.ContentSvg);

            }

            //List<Task> tasks = possibleTimeList.Select(async possibleTime =>
            //{

            //    var personAdjusted = foundPerson.ChangeBirthTime(possibleTime);

            //    var chart = await EventsChartAPI.GenerateNewChart(personAdjusted, timeRange, daysPerPixel, eventTags);
            //    //var timeHash = possibleTime.ToString(); //time to id the chart, because will complete asymmetrically
            //    dict.Add(possibleTime, chart.ContentSvg);

            //}).ToList();

            //await Task.WhenAll(tasks);





            //ACT 4 : Package to be vied together
            var combinedSvg = "";
            var chartYPosition = 30; //start with top padding
            var leftPadding = 10;
            foreach (var possibleTime in possibleTimeList)
            {
                //replace original birth time
                var personAdjusted = foundPerson.ChangeBirthTime(possibleTime);
                var newChartSvg = dict[possibleTime];
                var adjustedBirth = personAdjusted.BirthTimeString;

                //place in group with time above the chart
                var wrappedChart = $@"
                            <g transform=""matrix(1, 0, 0, 1, {leftPadding}, {chartYPosition})"">
                                <text style=""font-size: 16px; white-space: pre-wrap;"" x=""2"" y=""-6.727"">{adjustedBirth}</text>
                                {newChartSvg}
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
                svgWidth: maxWidth+100,
                svgTotalHeight: chartYPosition+100,
                randomId: Tools.GenerateId(),
                svgBackgroundColor: "#757575"); //grey easy on the eyes


            //ACT 5 : Save to file
            //This is the part that could not be done in cloud, cost and time
            //recognizable name for file
            var fileName = $"BirthTimeFinder-EventsChart-{foundPerson.Name}-{foundPerson.BirthYear}-{precisionInHours}PiH.svg";
            var folderName = $"VedAstro Console";

            //save to desktop, easy to spot
            string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            string folderPath = Path.Combine(desktopPath, folderName);

            //create folder if no show
            Directory.CreateDirectory(folderPath);

            //save to local file
            string fullPath = Path.Combine(folderPath, fileName);
            await File.WriteAllTextAsync(fullPath, finalSvg);

            //let user know
            System.Console.WriteLine("File Done & Saved to Desktop!");
        }

        private static async Task MigrateOldLifeEventsToNewFormat()
        {

            //get latest file from server
            //note how this creates & destroys per call to method
            //might cost little extra cycles but it's a functionality
            //to always get the latest list
            var personListXmlDoc = await Tools.GetPersonListFile();

            //list of person XMLs
            var personXmlList = personListXmlDoc?.Root?.Elements() ?? new List<XElement>();

            //parse to type
            var allPersonList = Person.FromXml(personXmlList);

            //only person with life events
            var filtered = allPersonList.Where(x => x.LifeEventList.Any());

            //convert to new xml format then inset into xdoc back
            foreach (var updatedPerson in filtered)
            {
                //directly updates and saves new person record to main list (does all the work, sleep easy)
                await Tools.UpdatePersonRecord(updatedPerson);
            }

            System.Console.WriteLine("All person record updated!");

        }
        
        private static async Task MigrateOldLifeEventsToNewFormat2()
        {

            //get latest file from server
            //note how this creates & destroys per call to method
            //might cost little extra cycles but it's a functionality
            //to always get the latest list
            //access to file
            var fileClient = await Tools.GetBlobClientAzure(Tools.PersonListFile, Tools.BlobContainerName);

            //get xml file
            var personListXmlDoc = await Tools.DownloadToXDoc(fileClient);

            //list of person XMLs
            var personXmlList = personListXmlDoc?.Root?.Elements() ?? new List<XElement>();

            //parse to type
            var allPersonList = Person.FromXml(personXmlList);

            //only person with life events
            var filtered = allPersonList.Where(x => x.LifeEventList.Any());

            //convert to new xml format then inset into xdoc back
            foreach (var updatedPerson in filtered)
            {

                //delete the old person record,
                Tools.DeleteXElementFromXDocumentAzure(updatedPerson.ToXml(), ref personListXmlDoc);

                //and insert updated record in the updated as new
                //add new person to main list
                Tools.AddXElementToXDocumentAzure(updatedPerson.ToXml(), ref personListXmlDoc);

            }

            //upload modified list to storage
            await Tools.OverwriteBlobData(fileClient, personListXmlDoc);

            System.Console.WriteLine("All person record updated!");

        }

    }
}