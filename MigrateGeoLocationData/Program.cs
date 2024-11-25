using System;
using System.Formats.Asn1;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using CsvHelper;
using CsvHelper.Configuration;
using VedAstro.Library;

namespace MigrateGeoLocationData
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            // Specify the path to the CSV file
            string csvFilePath = "C:\\Users\\ASUS\\Downloads\\GeoLocationCacheWithTimestamp.csv";

            try
            {
                // Check if the file exists
                if (File.Exists(csvFilePath))
                {
                    using (var reader = new StreamReader(csvFilePath))
                    using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
                    {
                        csv.Read();
                        csv.ReadHeader();

                        var locationManager = new LocationManager();

                        var rows = new List<CsvRow>();

                        while (csv.Read())
                        {
                            try
                            {
                                string partitionKey = csv.GetField(0);
                                string rowKey = csv.GetField(1);
                                string timestamp = csv.GetField(2);
                                string cleanedName = csv.GetField(3);
                                string latitude = csv.GetField(4);
                                string longitude = csv.GetField(5);
                                string timezone = csv.GetField(6);
                                string source = csv.GetField(7);
                                string searchedName = csv.GetField(8);

                                rows.Add(new CsvRow
                                {
                                    PartitionKey = partitionKey,
                                    RowKey = rowKey,
                                    Timestamp = timestamp,
                                    CleanedName = cleanedName,
                                    Latitude = latitude,
                                    Longitude = longitude,
                                    Timezone = timezone,
                                    Source = source,
                                    SearchedName = searchedName
                                });
                            }
                            catch (Exception e)
                            {
                                Console.WriteLine(e);
                            }
                        }

                        await Parallel.ForEachAsync(rows, new ParallelOptions { MaxDegreeOfParallelism = 10 }, async (row, token) =>
                        {
                            try
                            {
                                // Do something with the data
                                Console.WriteLine($"PartitionKey: {row.PartitionKey}");
                                Console.WriteLine($"RowKey: {row.RowKey}"); //ticks
                                Console.WriteLine($"Timestamp: {row.Timestamp}");
                                Console.WriteLine($"CleanedName: {row.CleanedName}");
                                Console.WriteLine($"Latitude: {row.Latitude}");
                                Console.WriteLine($"Longitude: {row.Longitude}");
                                Console.WriteLine($"Timezone: {row.Timezone}");
                                Console.WriteLine($"Source: {row.Source}");
                                Console.WriteLine($"SearchedName: {row.SearchedName}");
                                Console.WriteLine("------------------------");

                                //get time of offset (UTC) as in DB
                                var timeOfOffset = new DateTimeOffset(Int64.Parse(row.RowKey), TimeSpan.Zero);
                                var geoLocation = new GeoLocation("", double.Parse(row.Longitude), double.Parse(row.Latitude));

                                //if empty than pass
                                var timezoneIs0 = row.Timezone == "+00:00";
                                var timezoneIsEmpty = string.IsNullOrEmpty(row.Timezone);
                                if (timezoneIs0 || timezoneIsEmpty) { return; }

                                //check if DB already has record
                                var foundTimezoneRecord = await locationManager.GeoLocationToTimezone_Vedastro(geoLocation, timeOfOffset);
                                string timezoneStr = foundTimezoneRecord.MainRow.TimezoneText;
                                var noRecord = string.IsNullOrEmpty(timezoneStr);

                                //add new record to DB if not found
                                if (noRecord)
                                {
                                    var geoLocationTimezoneEntity = new GeoLocationTimezoneEntity();
                                    geoLocationTimezoneEntity.PartitionKey = geoLocation.ToPartitionKey();
                                    geoLocationTimezoneEntity.RowKey = timeOfOffset.ToRowKey();
                                    geoLocationTimezoneEntity.TimezoneText = row.Timezone;
                                    geoLocationTimezoneEntity.MetadataHash = "FromOldDB";
                                    locationManager.AddToTimezoneTable(geoLocationTimezoneEntity);
                                }
                            }
                            catch (Exception e)
                            {
                                Console.WriteLine(e);
                            }
                        });
                    }
                }
                else
                {
                    Console.WriteLine("The file does not exist.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("An error occurred: " + ex.Message);
            }
        }
    }

    public class CsvRow
    {
        public string PartitionKey { get; set; }
        public string RowKey { get; set; }
        public string Timestamp { get; set; }
        public string CleanedName { get; set; }
        public string Latitude { get; set; }
        public string Longitude { get; set; }
        public string Timezone { get; set; }
        public string Source { get; set; }
        public string SearchedName { get; set; }
    }
}
