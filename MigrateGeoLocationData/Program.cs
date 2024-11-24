using System;
using System.Formats.Asn1;
using System.Globalization;
using System.IO;
using System.Linq;
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

                                // Do something with the data
                                Console.WriteLine($"PartitionKey: {partitionKey}");
                                Console.WriteLine($"RowKey: {rowKey}"); //ticks
                                Console.WriteLine($"Timestamp: {timestamp}");
                                Console.WriteLine($"CleanedName: {cleanedName}");
                                Console.WriteLine($"Latitude: {latitude}");
                                Console.WriteLine($"Longitude: {longitude}");
                                Console.WriteLine($"Timezone: {timezone}");
                                Console.WriteLine($"Source: {source}");
                                Console.WriteLine($"SearchedName: {searchedName}");
                                Console.WriteLine("------------------------");

                                //get time of offset (UTC) as in DB
                                var timeOfOffset = new DateTimeOffset(Int64.Parse(rowKey), TimeSpan.Zero);
                                var geoLocation = new GeoLocation("", double.Parse(longitude), double.Parse(latitude));

                                //if empty than pass
                                var timezoneIs0 = timezone == "+00:00";
                                var timezoneIsEmpty = string.IsNullOrEmpty(timezone);
                                if (timezoneIs0 || timezoneIsEmpty) { continue; }

                                //check if DB already has record
                                var locationManager = new LocationManager();
                                var foundTimezoneRecord = await locationManager.GeoLocationToTimezone_Vedastro(geoLocation, timeOfOffset);
                                string timezoneStr = foundTimezoneRecord.MainRow.TimezoneText;
                                var noRecord = string.IsNullOrEmpty(timezoneStr);

                                //add new record to DB if not found
                                if (noRecord)
                                {
                                    var geoLocationTimezoneEntity = new GeoLocationTimezoneEntity();
                                    geoLocationTimezoneEntity.PartitionKey = geoLocation.ToPartitionKey();
                                    geoLocationTimezoneEntity.RowKey = timeOfOffset.ToRowKey();
                                    geoLocationTimezoneEntity.TimezoneText = timezone;
                                    geoLocationTimezoneEntity.MetadataHash = "FromOldDB";
                                    locationManager.AddToTimezoneTable(geoLocationTimezoneEntity);
                                }
                            }
                            catch (Exception e)
                            {
                                Console.WriteLine(e);
                                //continue
                            }
                        }
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
}
