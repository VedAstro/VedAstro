using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Xml.Linq;
using Newtonsoft.Json.Linq;

namespace VedAstro.Library
{

    //IMMUTABLE CLASS
    [Serializable()]
    //TODO CANDIDATE FOR RECORD STRUCT
    public readonly struct GeoLocation : IToXml
    {
        /// <summary>
        /// Returns an Empty Time instance meant to be used as null/void filler
        /// for debugging and generating empty dasa svg lines
        /// </summary>
        public static GeoLocation Empty = new("Empty", 101, 4.59); //ipoh

        /// <summary>
        /// Accurate AI typed ready made locations
        /// </summary>
        public static GeoLocation Tokyo = new GeoLocation("Tokyo, Japan", 139.83, 35.65);
        public static GeoLocation Bangkok = new GeoLocation("Bangkok, Thailand", 100.50, 13.75);
        public static GeoLocation Bangalore = new GeoLocation("Bangalore, India", 77.5946, 12.9716);
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

        public override int GetHashCode()
        {
            //get hash of all the fields & combine them
            var hash1 = Tools.GetStringHashCode(this._name);
            var hash2 = _longitude.GetHashCode();
            var hash3 = _latitude.GetHashCode();

            return hash1 + hash2 + hash3;
        }

        public JToken ToJson()
        {
            var temp = new JObject();
            temp["Name"] = this.Name();
            temp["Longitude"] = this.Longitude();
            temp["Latitude"] = this.Latitude();

            return temp;
        }

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
                LibLogger.Error(e, $"GeoLocation.FromXml FAIL! : {locationXml}");

                //instead of giving up return something
                return GeoLocation.Empty;

                throw new Exception($"BLZ:GeoLocation.FromXml() Failed : {locationXml}");
            }
        }

        public static GeoLocation FromJson(JToken locationJson)
        {
            var name = locationJson["Name"].Value<string>();
            var longitude = locationJson["Longitude"].Value<double>();
            var latitude = locationJson["Latitude"].Value<double>();

            return new GeoLocation(name, longitude, latitude);

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
        public static async Task<GeoLocation> FromIpAddress(string apiKey)
        {
            try
            {
                //TODO switch to using VedAstro API
                //get only coordinates 1st
                var coordinates = await GetCoordinatesFromIpAddressGoogle(apiKey);

                //get name from coordinates
                var fromIpAddress = await CoordinatesToGeoLocation(coordinates.Latitude(), coordinates.Longitude());

                //new geo location from the depths
                return fromIpAddress;
            }
            catch (Exception e)
            {
                //log it
                Console.WriteLine($"Client Location: FAILED!!!");
                await LibLogger.Error(e);

                //return some location to avert meltdown
                return Tokyo;
            }
        }


        public static async Task<GeoLocation> CoordinatesToGeoLocation(double latitude, double longitude)
        {
            //round the coordinates, to match cache better and also because 3 decimal places is enough
            var latitudeRound = Math.Round(latitude, 3);
            var longitudeRound = Math.Round(longitude, 3);

            //get from API
            var url = URL.CoordinatesToGeoLocationAPIStable + $"/Latitude/{latitudeRound}/Longitude/{longitudeRound}";
            var webResult = await Tools.ReadFromServerXmlReply(url);

            //convert
            var parsed = GeoLocation.FromXml(webResult.Payload);

            return parsed;
        }


        /// <summary>
        /// Will get longitude and latitude from IP using google API
        /// NOTE: The only place so far Google API outside VedAstro API
        /// </summary>
        private static async Task<GeoLocation> GetCoordinatesFromIpAddressGoogle(string apiKey)
        {
            var url = $"https://www.googleapis.com/geolocation/v1/geolocate?key={apiKey}";
            var resultString = await Tools.WriteServer<JObject, object>(HttpMethod.Post, url);

            //get raw value 
            var rawLat = resultString["location"]["lat"].Value<double>();
            var rawLong = resultString["location"]["lng"].Value<double>();

            var result = new GeoLocation("", rawLong, rawLat);

            return result;
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

    }
}