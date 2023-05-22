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


        //FIELDS
        private readonly string _name;
        private readonly double _longitude;
        private readonly double _latitude;

        //CTOR
        /// <summary>
        /// Note : Eastern longitudes are positive, western ones negative.
        /// </summary>
        public GeoLocation(string name, double longitude, double latitude)
        {
            _name = name;
            _longitude = longitude;
            _latitude = latitude;
        }

        //PUBLIC METHODS
        public string GetName() => _name;

        /// <summary>
        /// Eastern longitudes are positive, western ones negative.
        /// Range : -180 to 180
        /// </summary>
        public double GetLongitude() => _longitude;

        /// <summary>
        /// Range : -90 to 90
        /// </summary>
        public double GetLatitude() => _latitude;



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
            temp["Name"] = this.GetName();
            temp["Longitude"] = this.GetLongitude();
            temp["Latitude"] = this.GetLatitude();

            return temp;
        }

        public XElement ToXml()
        {
            var locationHolder = new XElement("Location");
            var name = new XElement("Name", this.GetName());
            var longitude = new XElement("Longitude", this.GetLongitude());
            var latitude = new XElement("Latitude", this.GetLatitude());

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

            //if fail, raise alarm
            if (!results.IsPass) { throw new Exception("Location failed to find!"); }

            //make the new Geo Location and return it
            return results.Payload;
        }

        public static async Task<GeoLocation> FromIpAddress()
        {
            try
            {
                var apiKey = "AIzaSyDqBWCqzU1BJenneravNabDUGIHotMBsgE";
                var url = $"https://www.googleapis.com/geolocation/v1/geolocate?key={apiKey}";
                var resultString = await Tools.WriteServer(HttpMethod.Post, url);
                //var parsed = JObject.Parse(resultString);
                var lat = resultString["location"]["lat"].Value<double>();
                var lng = resultString["location"]["lng"].Value<double>();

                //get name from lat and long
                var urlReverse = $"https://maps.googleapis.com/maps/api/geocode/json?latlng={lat},{lng}&key={apiKey}";
                var resultString2 = await Tools.WriteServer(HttpMethod.Post, urlReverse);
                //var parsed2 = JObject.Parse(resultString2);
                var resultsJson = resultString2["results"][0];

                var locationNameLong = resultsJson["formatted_address"].Value<string>();
                var splitted = locationNameLong.Split(',');
                //keep only the last parts, country, state...
                var newLocationName = $"{splitted[splitted.Length - 3]},{splitted[splitted.Length - 2]},{splitted[splitted.Length-1]}";

                var fromIpAddress = new GeoLocation(newLocationName, lng, lat);

                Console.WriteLine($"Client Location:{fromIpAddress}");

                //new geo location from the depths
                return fromIpAddress;
            }
            catch (Exception e)
            {
                //log it
                Console.WriteLine($"Client Location: FAILED!!!");
                await LibLogger.Error(e);

                //return empty to avert meltdown
                return GeoLocation.Empty;

            }
        }
    }
}