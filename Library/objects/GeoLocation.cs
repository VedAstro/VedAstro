using System;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Genso.Astrology.Library
{


    //IMMUTABLE CLASS
    [Serializable()]
    public struct GeoLocation : IToXml
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
        /// </summary>
        public double GetLongitude() => _longitude;

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

        public static GeoLocation FromXml(XElement root)
        {
            try
            {
                var name = root.Element("Name")?.Value;
                var longitude = Double.Parse(root.Element("Longitude")?.Value);
                var latitude = Double.Parse(root.Element("Latitude")?.Value);


                return new GeoLocation(name, longitude, latitude);

            }
            catch (Exception e)
            {
                throw new Exception($"BLZ:GeoLocation.FromXml() Failed : {root}");
            }
        }

        /// <summary>
        /// Given a place's name, will get fully initialized GeoLocation.
        /// Using Google API
        /// </summary>
        public static async Task<GeoLocation> FromName(string locationName)
        {
            var DefaultLocationCountry = "Singapore";

            TryAgain:
            //if location not set, default to preset location
            locationName = locationName is "" or null ? DefaultLocationCountry : locationName;

            //sometimes location can't be found, causes critical failure
            //so if fail here continue with default location
            (string FullName, double Latitude, double Longitude) coordinates;
            try
            {
                //get longitude & latitude for location from API
                coordinates = await Tools.AddressToCoordinate(locationName);
            }
            catch (Exception e)
            {
                //if fail set to default location and continue
                locationName = DefaultLocationCountry;
                goto TryAgain;
            }

            //set new coordinates into view
            double longitude = coordinates.Longitude;
            double latitude = coordinates.Latitude;

            //update user typed location name to proper formatted one
            var formattedLocationName = coordinates.FullName;

            //make the new Geo Location and return it
            var newGeoLocation = new GeoLocation(formattedLocationName, longitude, latitude);
            return newGeoLocation;
        }
    }
}