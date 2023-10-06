
using CsvHelper;
using CsvHelper.Configuration;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace DataProcessor
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello, World!");

            var inputCsv = File.ReadAllText("C:\\Users\\ASUS\\Desktop\\Projects\\VedAstro\\DataProcessor\\SampleInput.csv");

            //process it
            var outputCsv = ProcessCsv(inputCsv);

            File.WriteAllText("C:\\Users\\ASUS\\Desktop\\Projects\\VedAstro\\DataProcessor\\SampleOutput.csv", outputCsv);

            Console.WriteLine("Done!!");

            //hold control
            Console.ReadLine();
        }

        public static string ProcessCsv(string inputCsv)
        {
            using var reader = new StringReader(inputCsv);
            using var csvReader = new CsvReader(reader, CultureInfo.InvariantCulture);
            var records = csvReader.GetRecords<dynamic>().ToList();
            var outputRecords = records.Select(record =>
            {

                //correct incoming name data
                string rawName = record.prsName;
                string parsedName = "";
                if (rawName.Contains(','))
                {
                    string[] parts = rawName.Split(',');
                    parsedName = $"{parts[1].Trim()} {parts[0].Trim()}";
                }
                else
                {
                    parsedName = rawName;
                }

                //clean name of invalid chars
                parsedName = Regex.Replace(parsedName, "[^a-zA-Z\\s]", "");

                //compile correct time format
                string rawHourMin = record.hour; //07:08:00
                DateTime time = DateTime.ParseExact(rawHourMin, "HH:mm:ss", null);
                string parsedHourMinute = time.ToString("HH:mm");


                string rawDay = record.day;
                string rawMonth = record.month;
                string rawYear = record.year;
                var parsedDate = $"{rawDay.PadLeft(2, '0')}/{rawMonth.PadLeft(2, '0')}/{rawYear}";

                var rawTimeZone = record.dobTz;
                TimeSpan result;
                var parsedTimeZone = "";
                if (TimeSpan.TryParse(rawTimeZone, out result))
                {
                    parsedTimeZone = rawTimeZone;
                }
                else
                {
                    parsedTimeZone = "+00:00";
                }

                var parsedTimeString = $"{parsedHourMinute} {parsedDate} {parsedTimeZone}";

                var foo = new
                {
                    Name = parsedName,
                    Time = parsedTimeString,
                };

                return foo;
            });

            using var writer = new StringWriter();
            using var csvWriter = new CsvWriter(writer, new CsvConfiguration(CultureInfo.InvariantCulture) { NewLine = Environment.NewLine });
            csvWriter.WriteRecords(outputRecords);
            return writer.ToString();
        }
    }
}
