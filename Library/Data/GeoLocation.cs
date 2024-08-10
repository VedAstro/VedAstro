using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Xml.Linq;
using Newtonsoft.Json.Linq;

//HOW TO BE WISE?
//THINK OF ALL THE STUPID THINGS
//...AND DON'T DO IT

//YOUTH, 

namespace VedAstro.Library
{

    //IMMUTABLE CLASS
    [Serializable()]
    //TODO CANDIDATE FOR RECORD STRUCT
    public class GeoLocation : IToXml, IToJpeg, IToJson, IToDataTable, IFromUrl
    {
        /// <summary>
        /// The number of pieces the URL version of this instance needs to be cut for processing
        /// used to segregate out the combined URL with other data
        /// EXP -> ../Location/Tokyo, Japan/Coordinates/35.65,139.83/ == 4 PIECES
        ///      
        /// </summary>
        public static int OpenAPILength = 4;


        /// <summary>
        /// Returns an Empty Time instance meant to be used as null/void filler
        /// for debugging and generating empty dasa svg lines
        /// </summary>
        public static GeoLocation Empty = new("Empty", 101, 4.59); //ipoh

        /// <summary>
        /// Accurate AI typed ready-made locations
        /// </summary>
        public static GeoLocation Tokyo = new GeoLocation("Tokyo, Japan", 139.83, 35.65);
        public static GeoLocation Bangkok = new GeoLocation("Bangkok, Thailand", 100.50, 13.75);
        public static GeoLocation Bangalore = new GeoLocation("Bangalore, India", 77.5946, 12.9716);
        public static GeoLocation Ipoh = new GeoLocation("Ipoh, Malaysia", 101.0758, 4.6005);
        public static GeoLocation LosAngeles = new GeoLocation("Los Angeles, CA, USA", -118.243, 34.055);
        public static GeoLocation Ahmedabad = new GeoLocation("Ahmedabad, India", 72.5714, 23.0225);
        public static GeoLocation Ujjain = new GeoLocation("Ujjain, India", 75.7167, 23.1667);
        public static GeoLocation WashingtonDC = new GeoLocation("Washington D.C., USA", -77.0369, 38.9072);
        public static GeoLocation London = new GeoLocation("London, UK", -0.1276, 51.5074);
        public static GeoLocation Paris = new GeoLocation("Paris, France", 2.3522, 48.8566);
        public static GeoLocation Berlin = new GeoLocation("Berlin, Germany", 13.4050, 52.5200);
        public static GeoLocation Canberra = new GeoLocation("Canberra, Australia", 149.1300, -35.2809);
        public static GeoLocation Ottawa = new GeoLocation("Ottawa, Canada", -75.6972, 45.4215);
        public static GeoLocation Brasilia = new GeoLocation("Brasilia, Brazil", -47.8825, -15.7942);
        public static GeoLocation Moscow = new GeoLocation("Moscow, Russia", 37.6176, 55.7558);
        public static GeoLocation Beijing = new GeoLocation("Beijing, China", 116.4074, 39.9042);
        public static GeoLocation Mumbai = new GeoLocation("Mumbai, India", 72.8775, 19.0761);
        public static GeoLocation Singapore = new GeoLocation("Singapore, Singapore", 103.8198, 1.3521);
        public static GeoLocation Chicago = new GeoLocation("Chicago, USA", -87.6298, 47.8781);
        public static GeoLocation TestLocB = new GeoLocation("TestLocB, TestLocationB", 80.25, 13.0667);
        public static GeoLocation Barrington = new GeoLocation("Barrington, USA", -88.13611, 42.1538);

        //FIELDS
        private readonly string _name;
        private readonly double _longitude;
        private readonly double _latitude;

        //CTOR
        /// <summary>
        /// Auto checks and corrects of wrong coordinates decimal placing
        /// Note : Eastern longitudes are positive, western ones negative.
        /// </summary>
        public GeoLocation(string name, double longitude, double latitude)
        {
            _name = name;

            //coordinates have been known to be inputed with misplaced decimal (from api)
            //this will check and try correct if possible
            bool isValid = IsValidLatitudeLongitude(longitude, latitude);
            if (isValid) //normal operation
            {
                _longitude = longitude;
                _latitude = latitude;
            }
            else //abnormal input, auto correct decimal place as most likely fault (heavy computation use only when sure fail)
            {
                _longitude = CorrectDecimalPoint(longitude);
                _latitude = CorrectDecimalPoint(latitude);
            }
        }

        //PUBLIC METHODS
        public string Name() => _name;

        /// <summary>
        /// Eastern longitudes are positive, western ones negative.
        /// Range : -180 to 180
        /// </summary>
        public double Longitude() => _longitude;

        /// <summary>
        /// Range : -90 to 90
        /// </summary>
        public double Latitude() => _latitude;



        //OVERRIDES METHODS
        public override bool Equals(object value)
        {

            if (value.GetType() == typeof(GeoLocation))
            {
                //cast to type
                var parsedValue = (GeoLocation)value;

                //check equality
                bool returnValue = (this.GetHashCode() == parsedValue.GetHashCode());

                return returnValue;
            }
            else
            {
                //return false if value is null
                return false;
            }
        }

        /// <summary>
        /// Prints name of location with coordinates
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            //return location name
            return $"{_name} {_longitude} {_latitude}";
        }

        /// <summary>
        /// .../Location/Tokyo, Japan/Coordinates/35.65,139.83/
        /// </summary>
        public static async Task<dynamic> FromUrl(string url)
        {
            //              0           1           2           3
            // INPUT -> "/Location/Tokyo, Japan/Coordinates/35.65,139.83"
            string[] parts = url.Split(new char[] { '/' }, StringSplitOptions.RemoveEmptyEntries); //remove empty needed for back handling last slash

            //extract the needed data out
            var locationName = parts[1];
            var coordinatesPart = parts[3].Split(',');
            var latitude = double.Parse(coordinatesPart[0]);
            var longitude = double.Parse(coordinatesPart[1]);

            var parsedGeoLocation = new GeoLocation(locationName, longitude, latitude);

            return parsedGeoLocation;

        }

        /// <summary>
        /// Convert current instance of geolocation for use in OpenAPI url
        /// EXP: .../Location/Tokyo, Japan/Coordinates/35.65,139.83
        /// </summary>
        public string ToUrl()
        {
            //out .../Location/Tokyo, Japan/Coordinates/35.65,139.83
            var url = $"/Location/{this.Name()}/Coordinates/{this.Latitude()},{this.Longitude()}";

            return url;
        }

        public override int GetHashCode()
        {
            //get hash of all the fields & combine them
            var hash1 = Tools.GetStringHashCode(this._name);
            var hash2 = _longitude.GetHashCode();
            var hash3 = _latitude.GetHashCode();

            return hash1 + hash2 + hash3;
        }

        public byte[] ToJpeg() { var table = this.ToDataTable(); return Tools.DataTableToJpeg(table); }

        public DataTable ToDataTable()
        {
            // Create a new DataTable.
            DataTable table = new DataTable("GeoLocation");

            // Define columns.
            table.Columns.Add("Name", typeof(string));
            table.Columns.Add("Value", typeof(string));

            // fill rows
            table.Rows.Add("Name", this.Name());
            table.Rows.Add("Longitude", this.Longitude());
            table.Rows.Add("Latitude", this.Latitude());

            return table;
        }

        public JToken ToJson()
        {
            var temp = new JObject();
            temp["Name"] = this.Name();
            temp["Longitude"] = this.Longitude();
            temp["Latitude"] = this.Latitude();

            return temp;
        }

        JObject IToJson.ToJson() => (JObject)this.ToJson();

        public XElement ToXml()
        {
            var locationHolder = new XElement("Location");
            var name = new XElement("Name", this.Name());
            var longitude = new XElement("Longitude", this.Longitude());
            var latitude = new XElement("Latitude", this.Latitude());

            locationHolder.Add(name, longitude, latitude);

            return locationHolder;
        }

        /// <summary>
        /// The root element is expected to be name of Type
        /// Note: Special method done to implement IToXml
        /// </summary>
        public dynamic FromXml<T>(XElement xml) where T : IToXml => FromXml(xml);

        public static GeoLocation FromXml(XElement locationXml)
        {
            try
            {
                var name = locationXml.Element("Name")?.Value;
                var longitude = Double.Parse(locationXml.Element("Longitude")?.Value ?? "-1"); //-1 so easy to spot
                var latitude = Double.Parse(locationXml.Element("Latitude")?.Value ?? "-1");


                return new GeoLocation(name, longitude, latitude);

            }
            catch (Exception e)
            {
                //log it
                LibLogger.Debug(e, $"GeoLocation.FromXml FAIL! : {locationXml}");

                //instead of giving up return something
                return GeoLocation.Empty;

                throw new Exception($"BLZ:GeoLocation.FromXml() Failed : {locationXml}");
            }
        }

        public static GeoLocation FromJson(JToken rawJson)
        {
            try
            {
                //PROBABILITY 1 :
                //rawJson sample JSON
                //"{
                //   "AddressToGeoLocation": {
                //     "Name": "Ipoh, Perak, Malaysia",
                //     "Longitude": 101.077,
                //     "Latitude": 4.599
                //   }
                // }

                //auto dig 1 level into package
                var locationJson = ((JObject)rawJson).Properties().FirstOrDefault()?.Value;

                var name = locationJson["Name"].Value<string>();
                var longitude = locationJson["Longitude"].Value<double>();
                var latitude = locationJson["Latitude"].Value<double>();

                return new GeoLocation(name, longitude, latitude);
                
            }
            catch (Exception e)
            {

                try
                {
                    //TODO AS BACKUP BECAUSE ANOTHER WAY OF SAVING IN DB, MINOR SCHEMA CHANGE
                    //PROBABILITY 2 :
                    var name = rawJson["Name"].Value<string>();
                    var longitude = rawJson["Longitude"].Value<double>();
                    var latitude = rawJson["Latitude"].Value<double>();

                    return new GeoLocation(name, longitude, latitude);


                }
                catch (Exception exception)
                {
                    Console.WriteLine(exception);
                    return GeoLocation.Empty;
                }
            }

        }

        /// <summary>
        /// Given a place's name, will get fully initialized GeoLocation.
        /// Using Google API
        /// </summary>
        public static async Task<GeoLocation> FromName(string locationName)
        {

            //try get location from name using google API
            var results = await Tools.AddressToGeoLocation(locationName);

            //meant to fail, so return Empty
            if (!results.IsPass)
            {
                return Empty;
            }
            else
            {
                //make the new Geo Location and return it
                return results.Payload;
            }
        }

        /// <summary>
        /// Tries to get location from IP address if fail uses sample location
        /// important is to always have some location for app to use
        /// </summary>
        /// <returns></returns>
        public static async Task<GeoLocation> FromIpAddress()
        {
            try
            {
                //get only coordinates 1st
                var fromIpAddress = await SystemIpAddressToGeoLocation();

                //get name from coordinates
                //var fromIpAddress = await CoordinatesToGeoLocation(coordinates.Latitude(), coordinates.Longitude());

                //new geolocation from the depths
                return fromIpAddress;
            }
            catch (Exception e)
            {
                //log it
                LibLogger.Debug(e, $"Client Location: FAILED!!!");

                //return some location to avert meltdown
                return Empty;
            }
        }

        /// <summary>
        /// Gets coordinates with VedAstro API
        /// </summary>
        public static async Task<GeoLocation> CoordinatesToGeoLocation(double latitude, double longitude)
        {
            //round the coordinates, to match cache better and also because 3 decimal places is enough
            var latitudeRound = Math.Round(latitude, 3);
            var longitudeRound = Math.Round(longitude, 3);

            //get from API
            var allUrls = new URL(ThisAssembly.BranchName.Contains("beta")); //todo clean up
            var url = allUrls.CoordinatesToGeoLocationAPI + $"/Latitude/{latitudeRound}/Longitude/{longitudeRound}";
            var webResult = await Tools.ReadFromServerJsonReplyVedAstro(url);

            //convert
            var parsed = GeoLocation.FromJson(webResult.Payload);

            return parsed;
        }

        /// <summary>
        /// Gets coordinates with VedAstro API
        /// IP address is auto-detected by API, so need to supply 
        /// </summary>
        public static async Task<GeoLocation> SystemIpAddressToGeoLocation()
        {
            //get from API
            var allUrls = new URL(ThisAssembly.BranchName.Contains("beta")); //todo clean up
            var ipAddress = await Tools.GetIPAddress();
            var url = allUrls.IpAddressToGeoLocationAPI + $"/IpAddress/{ipAddress}";
            var webResult = await Tools.ReadFromServerJsonReplyVedAstro(url);

            //convert
            var parsed = GeoLocation.FromJson(webResult.Payload);

            return parsed;
        }





        /// <summary>
        /// In geographical coordinates, the maximum latitude is 90 degrees and the minimum latitude is -90 degrees.
        /// The maximum longitude is 180 degrees and the minimum longitude is -180 degrees.
        /// Here is a simple C# method to check if a given latitude and longitude are within these ranges:
        /// </summary>
        public bool IsValidLatitudeLongitude(double longitude, double latitude)
        {
            if (latitude < -90 || latitude > 90)
            {
                Console.WriteLine($"Invalid Latitude! {latitude}");
                return false;
            }
            if (longitude < -180 || longitude > 180)
            {
                Console.WriteLine($"Invalid Longitude! {longitude}");
                return false;
            }

            //if control reaches here than is valid
            return true;
        }


        /// <summary>
        /// Converts "-466395571" to "-46.6395571", also maintains "34.333" to ""34.333"
        /// Used for auto correcting bad input data, heavy computation use only when sure fail
        /// </summary>
        public double CorrectDecimalPoint(double input)
        {
            // Convert the double to a string
            string inputStr = input.ToString();
            // Check if the input is negative
            bool isNegative = inputStr.StartsWith("-");
            // Remove the negative sign if it exists
            if (isNegative)
            {
                inputStr = inputStr.Substring(1);
            }
            // Calculate the position to insert the decimal point
            int insertPosition = inputStr.Length > 7 ? inputStr.Length - 7 : 0;
            // Insert the decimal point at the correct position
            inputStr = inputStr.Insert(insertPosition, ".");
            // Convert the string back to a double
            double output = double.Parse(inputStr);
            // If the input was negative, make the output negative
            if (isNegative)
            {
                output = -output;
            }
            return output;
        }


        /// <summary>
        /// given a string value as name, will try to parse it using API,
        /// NOTE: data is cached!
        /// </summary>
        public static async Task<(bool, GeoLocation)> TryParse(string cellValue)
        {
            //CACHE MECHANISM
            return await CacheManager.GetCache(new CacheKey("GeoLocation_TryParse", cellValue), _tryParse);

            async Task<(bool, GeoLocation)> _tryParse()
            {
                try
                {

                    //should return empty 
                    var tryParsed = await GeoLocation.FromName(cellValue);

                    //if empty than parse failed
                    var isParsed = !(tryParsed.Equals(Empty));

                    return (isParsed, tryParsed);

                }
                //if fail for any reason, than empty so caller can know
                //NOTE: avoid exception or logging here, since failure is expected pattern
                catch
                {
                    return (false, Empty);
                }

            }
        }

        /// <summary>
        /// Check if an inputed STD time string is valid,
        /// returns default time if not parseable
        /// </summary>
        public static bool TryParseStd(string stdDateTimeText, out DateTimeOffset parsed)
        {
            try
            {
                parsed = DateTimeOffset.ParseExact(stdDateTimeText, Time.DateTimeFormat, null);
                return true;
            }
            catch (Exception)
            {
                //failure for any reason, return false
                parsed = new DateTimeOffset();
                return false;
            }
        }

        /// <summary>
        /// Returns lat long in google format
        /// EXP: -3.9571599,103.8723379
        /// </summary>
        /// <returns></returns>
        public string GetPartitionKey()
        {
            var roundedLong1DeciPlaces11Km = Math.Round(this.Longitude(), 1).ToString();
            var roundedLat1DeciPlaces11Km = Math.Round(this.Latitude(), 1).ToString();

            return $"{roundedLat1DeciPlaces11Km},{roundedLong1DeciPlaces11Km}";
        }

        public static JArray ToJsonList(List<GeoLocation> geolocationList)
        {
            var jsonList = new JArray();

            foreach (var eventInstance in geolocationList)
            {
                jsonList.Add(eventInstance.ToJson());
            }

            return jsonList;
        }


    }
}