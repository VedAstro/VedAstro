using System;

namespace Genso.Astrology.Library
{
    /// <summary>
    /// The constellation which is behind a planet
    /// </summary>
    [Serializable()]
    public struct PlanetConstellation
    {
        //CONST FIELDS
        private const int QuarterMax = 4;
        private const int QuarterMin = 1;


        //DATA FIELDS
        private readonly ConstellationName _name;
        private readonly int _quarter;
        //private readonly double _bodyPosition;


        //CTOR
        public PlanetConstellation(int constellationNumber, int quarter)
        {
            //convert constellation number to constellation name
            _name = (ConstellationName)constellationNumber;
            //save quarter
            _quarter = quarter;
        }



        //PUBLIC METHODS
        public int GetConstellationNumber()
        {
            //convert constellation name to its number
            return (int)_name;
        }

        public ConstellationName GetConstellationName()
        {
            return _name;
        }

        public int GetQuater()
        {
            return _quarter;
        }



        //OPERATORS OVERRIDES
        public static bool operator ==(PlanetConstellation left, PlanetConstellation right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(PlanetConstellation left, PlanetConstellation right)
        {
            return !(left == right);
        }



        //METHOD OVERRIDES
        public override bool Equals(object value)
        {

            if (value.GetType() == typeof(PlanetConstellation))
            {
                //cast to constellation
                var parsedValue = (PlanetConstellation)value;

                //Check equality
                bool returnValue = (this.GetHashCode() == parsedValue.GetHashCode());

                return returnValue;
            }
            else
            {
                //Return false if value is null
                return false;
            }


        }

        public override int GetHashCode()
        {
            //get hash of all the fields & combine them
            var hash1 = _name.GetHashCode();
            var hash2 = _quarter.GetHashCode();

            return hash1 + hash2;
        }

        public override string ToString()
        {
            return $"{_name} - {_quarter}";
        }

    }
}
