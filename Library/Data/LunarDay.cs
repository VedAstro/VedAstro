using System;
using System.Data;
using Newtonsoft.Json.Linq;

namespace VedAstro.Library
{
    [Serializable()]
    public struct LunarDay : IToJson
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




        //PUBLIC METHODS
        /// <summary>
        /// Gets the lunar day number 1 to 15
        /// NOTE:
        /// - NEW MOON = 1
        /// - FULL MOON = 15
        /// </summary>
        public int GetLunarDayNumber() => _lunarDayNumber;

        /// <summary>
        /// Gets the lunar day number 1 to 30
        /// </summary>
        public int GetLunarDateNumber() => _lunarDateNumber;

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

        public string GetPaksha()
        {
            //get phase and convert accordingly
            var moonPhase = GetMoonPhase();
            switch (moonPhase)
            {
                case MoonPhase.BrightHalf:
                    return "Sukla";
                case MoonPhase.DarkHalf:
                    return "Krishna";
            }

            throw new Exception("END OF LINE!");
        }

        public string GetTithiName()
        {
            switch (_lunarDateNumber)
            {
                case 1: return "Padyami";
                case 2: return "Vidiya";
                case 3: return "Tadiya";
                case 4: return "Chavithi";
                case 5: return "Panchimi";
                case 6: return "Sashti";
                case 7: return "Saptami";
                case 8: return "Ashtami";
                case 9: return "Navami";
                case 10: return "Dasimi";
                case 11: return "Ekadasi";
                case 12: return "Dwadasi";
                case 13: return "Triodasi";
                case 14: return "Chaturdasi";
                case 15: return "Poornima";
                case 16: return "Padyami";
                case 17: return "Vidiya";
                case 18: return "Tadiya";
                case 19: return "Chavithi";
                case 20: return "Panchimi";
                case 21: return "Sashti";
                case 22: return "Saptami";
                case 23: return "Ashtami";
                case 24: return "Navami";
                case 25: return "Dasimi";
                case 26: return "Ekadasi";
                case 27: return "Dwadasi";
                case 28: return "Triodasi";
                case 29: return "Chaturdasi";
                case 30: return "Amavasya";
                default: throw new Exception("END OF THE LINE!");
            }
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



        //OVERRIDE METHODS
        public override string ToString()
        {
            //prepare string
            var returnString = $"Date:{this._lunarDateNumber}/30 Day:{this._lunarDayNumber}/15";

            //return string to caller
            return returnString;
        }


        public JObject ToJson()
        {

            var returnVal = new JObject();
            returnVal["Name"] = this.GetTithiName();
            returnVal["Paksha"] = this.GetPaksha();
            returnVal["Date"] = $"{_lunarDateNumber}/30";
            returnVal["Day"] = $"{_lunarDayNumber}/15";
            returnVal["Phase"] = this.GetMoonPhase().ToString();

            return returnVal;

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