using System;

namespace Genso.Astrology.Library
{
    [Serializable()]
    public struct LunarDay
    {
        //FIELDS
        private readonly int _lunarDateNumber; //number from 1 to 30
        private readonly int _lunarDayNumber; //number from 1 to 15

        //CTOR
        public LunarDay(int lunarDateNumber)
        {
            _lunarDateNumber = lunarDateNumber;

            //get lunar day number
            _lunarDayNumber = ConvertLunarDateNumberToLunarDayNumber(_lunarDateNumber);
        }

        //PRIVATE METHODS
        private static int ConvertLunarDateNumberToLunarDayNumber(int lunarDateNumber)
        {
            //declare lunar day number as 0 first
            int lunarDayNumber = 0;

            //if lunar date number is less than or equal to 15
            if (lunarDateNumber <= 15)
            {
                //lunar date number is same as lunar day number
                lunarDayNumber = lunarDateNumber;
            }
            else
            //if 16 and above
            {
                //minus lunar date number with 15 to get lunar day number
                lunarDayNumber = lunarDateNumber - 15;
            }

            // return lunar day number
            return lunarDayNumber;
        }

        //PUBLIC METHODS
        /// <summary>
        /// Gets the lunar day number 1 to 15
        /// </summary>
        public int GetLunarDayNumber()
        {
            return _lunarDayNumber;
        }
        /// <summary>
        /// Gets the lunar day group, such as Nanda, Bhadra, Jaya
        /// </summary>
        public LunarDayGroup GetLunarDayGroup()
        {

            //Nanda (1st, 6th and 11th lunar days)
            if (_lunarDayNumber == 1 ||
                _lunarDayNumber == 6 ||
                _lunarDayNumber == 11)
            {
                return LunarDayGroup.Nanda;
            }

            //Bhadra (2nd, 7th and 12th lunar days)
            if (_lunarDayNumber == 2 ||
                _lunarDayNumber == 7 ||
                _lunarDayNumber == 12)
            {
                return LunarDayGroup.Bhadra;
            }

            //Jaya (3rd 8th and 13th lunar days)
            if (_lunarDayNumber == 3 ||
                _lunarDayNumber == 8 ||
                _lunarDayNumber == 13)
            {
                return LunarDayGroup.Jaya;
            }

            //Riktha tithi (4th, 9th and 14th lunar days)
            if (_lunarDayNumber == 4 ||
                _lunarDayNumber == 9 ||
                _lunarDayNumber == 14)
            {
                return LunarDayGroup.Rikta;
            }

            //5th, 10th or 15th (Poorna)
            if (_lunarDayNumber == 5 ||
                _lunarDayNumber == 10 ||
                _lunarDayNumber == 15)
            {
                return LunarDayGroup.Purna;
            }

            //if no match raise error
            throw new Exception("No lunar day group found!");

        }

        /// <summary>
        /// Gets only the actual wanning or waxing pahse of the moon
        /// does not corelate with moon's malefic or benefic nature
        /// </summary>
        public MoonPhase GetMoonPhase()
        {
            //lunar date from 1 to 15 is bright half
            if (_lunarDateNumber >= 1 && _lunarDateNumber <= 15)
            {
                return MoonPhase.BrightHalf;
            }

            //lunar date from 16 to 30 is dark half
            if (_lunarDateNumber >= 16 && _lunarDateNumber <= 30)
            {
                return MoonPhase.DarkHalf;
            }

            //throw error if not found
            throw new Exception("Moon phase not found, error!");
        }

        /// <summary>
        /// Gets the lunar day number 1 to 30
        /// </summary>
        public int GetLunarDateNumber()
        {
            return _lunarDateNumber;
        }



        //OVERRIDE METHODS
        public override string ToString()
        {
            //prepare string
            var returnString = $"Date:{this._lunarDateNumber}/30 Day:{this._lunarDayNumber}/15";

            //return string to caller
            return returnString;
        }

        public override bool Equals(object value)
        {

            if (value.GetType() == typeof(LunarDay))
            {
                //cast to type
                var parsedValue = (LunarDay)value;

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
        /// Gets a unique value representing the data (NOT instance)
        /// </summary>
        public override int GetHashCode()
        {
            //combine all the hash of the fields
            var hash1 = _lunarDateNumber.GetHashCode();
            var hash2 = _lunarDayNumber.GetHashCode();

            return hash1 + hash2;
        }


    }
}