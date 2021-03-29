using System;

namespace Genso.Astrology.Library
{
    public interface IGeoLocation
    {
        string GetName();
        double GetLongitude();
        double GetLatitude();
    }


    //IMMUTABLE CLASS
    [Serializable()]
    public struct GeoLocation : IGeoLocation
    {
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

        public override string ToString()
        {
            //return location name
            return _name;
        }

        public override int GetHashCode()
        {
            //get hash of all the fields & combine them
            var hash1 = _name?.GetHashCode() ?? 0;
            var hash2 = _longitude.GetHashCode();
            var hash3 = _latitude.GetHashCode();

            return hash1 + hash2 + hash3;
        }
    }
}