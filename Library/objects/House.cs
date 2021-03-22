using System;
using System.Collections.Generic;

namespace Genso.Astrology.Library
{
    //TODO CAN BE MOVED OUT
    public enum HouseName
    {
        //Note: maintain int relation for code that is still using int
        House1 = 1,
        House2 = 2,
        House3 = 3,
        House4 = 4,
        House5 = 5,
        House6 = 6,
        House7 = 7,
        House8 = 8,
        House9 = 9,
        House10 = 10,
        House11 = 11,
        House12 = 12,
    }

    /// <summary>
    /// A simple data structure to contain an instance of a house in time
    /// </summary>
    [Serializable()]
    public struct House
    {
        //DATA FIELDS
        private Angle _middleLongitude;
        private Angle _beginLongitude;
        private Angle _endLongitude;
        private int _houseNumber;



        //CONST FIELDS
        /// <summary>
        /// Gets a list of all house names (for looping)
        /// </summary>
        public static readonly List<HouseName> AllHouses = new List<HouseName>()
        {
            HouseName.House1,
            HouseName.House2,
            HouseName.House3,
            HouseName.House4,
            HouseName.House5,
            HouseName.House6,
            HouseName.House7,
            HouseName.House8,
            HouseName.House9,
            HouseName.House10,
            HouseName.House11,
            HouseName.House12
        };




        //CTOR
        public House(int houseNumber, Angle beginLongitude, Angle middleLongitude, Angle endLongitude)
        {
            _middleLongitude = middleLongitude;
            _beginLongitude = beginLongitude;
            _endLongitude = endLongitude;
            _houseNumber = houseNumber;
        }


        //PUBLIC METHODS
        public int GetHouseNumber() => _houseNumber;
        public Angle GetMiddleLongitude() => _middleLongitude;

        public bool IsLongitudeInHouseRange(Angle longitude)
        {
            //if end longitude is less than begin longitude, past 360 degrees
            if (_endLongitude < _beginLongitude)
            {
                //before 360
                if (longitude >= _beginLongitude && longitude <= Angle.Degrees360)
                {
                    //longitude is within house
                    return true;
                }
                //after 360
                if (longitude >= Angle.Zero && longitude < _endLongitude)
                {
                    //longitude is within house
                    return true;
                }
            }
            else
            //else end longitude is more than begin, normal house range
            {
                //if longitude is between house begin and end range
                if (longitude >= _beginLongitude && longitude < _endLongitude)
                {
                    //longitude is within house
                    return true;
                }

            }


            //if not in range, not in house range
            return false;
        }



        //OVERRIDES METHODS
        public override bool Equals(object value)
        {

            if (value.GetType() == typeof(House))
            {
                //cast to type
                var parsedValue = (House)value;

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
            return $"{_houseNumber} - {_beginLongitude} - {_middleLongitude} - {_endLongitude}";
        }

        public override int GetHashCode()
        {
            //get hash of all the fields & combine them
            var hash1 = _houseNumber.GetHashCode();
            var hash2 = _beginLongitude.GetHashCode();
            var hash3 = _middleLongitude.GetHashCode();
            var hash4 = _endLongitude.GetHashCode();

            return hash1 + hash2 + hash3 + hash4;
        }

    }
}