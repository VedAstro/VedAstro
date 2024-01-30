using System;
using System.Collections.Generic;
using System.Linq;

namespace VedAstro.Library
{
    /// <summary>
    /// Holds all the info of a specific point in the constellation
    /// Usually a point (longitude) behind a planet.
    /// </summary>
    [Serializable()]
    public struct Constellation
    {
        //CONST FIELDS
        private const int QuarterMax = 4;
        private const int QuarterMin = 1;

        public static Constellation Empty = new Constellation(1, 1, Angle.FromDegrees(0));

        /// <summary>
        /// Gets all 27 constellation
        /// NOTE:
        /// - empty artificially removed
        /// - also used by API for ALL call
        /// </summary>
        public static List<ConstellationName> AllConstellation
        {
            get
            {
                return new List<ConstellationName>(((ConstellationName[])Enum.GetValues(typeof(ConstellationName))).Where(e => e != ConstellationName.Empty));
            }
        }

        //DATA FIELDS
        private readonly ConstellationName _name;
        private readonly int _quarter;
        private readonly Angle _degreeInConstellation;


        //CTOR
        public Constellation(int constellationNumber, int quarter, Angle degreeInConstellation)
        {
            //convert constellation number to constellation name
            _name = (ConstellationName)constellationNumber;
            _quarter = quarter;
            _degreeInConstellation = degreeInConstellation;

            //if degrees in constellation not within range raise alarm
            var min = Angle.Zero;
            var max = new Angle(0, 800, 0);
            if (_degreeInConstellation < min || _degreeInConstellation > max) { throw new Exception("Degrees in constellation not valid!"); }
        }



        //PUBLIC METHODS


        /// <summary>
        ///  Gets the constellation name as a number in the preset order
        /// </summary>
        public int GetConstellationNumber() => (int)_name; //convert constellation name to its number

        /// <summary>
        /// Gets the name of the constellation
        /// </summary>
        public ConstellationName GetConstellationName() => _name;

        /// <summary>
        /// Gets the quarter (subdivision) of a constellation, 1 to 4
        /// A rougher form of "degrees in constellation"
        /// </summary>
        public int GetQuarter() => _quarter;

        /// <summary>
        /// Gets the degrees in the constellation, 0 to 800' (13° 20')
        /// An accurate form of "quarter"
        /// </summary>
        public Angle GetDegreesInConstellation() => _degreeInConstellation;


        //OPERATORS OVERRIDES
        public static bool operator ==(Constellation left, Constellation right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(Constellation left, Constellation right)
        {
            return !(left == right);
        }



        //METHOD OVERRIDES
        public override bool Equals(object value)
        {

            if (value.GetType() == typeof(Constellation))
            {
                //cast to constellation
                var parsedValue = (Constellation)value;

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
            //todo can add degrees in constellation if needed
            return $"{_name} - {_quarter}";
        }

    }
}
