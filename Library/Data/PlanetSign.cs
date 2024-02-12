using System;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;

namespace VedAstro.Library
{
    /// <summary>
    /// The zodiac sign which is at a longitude
    /// </summary>
    [Serializable()]
    public struct ZodiacSign : IToJson
    {

        /// <summary>
        /// List of zodiac sign in order
        /// </summary>
        public static readonly List<ZodiacName> All12ZodiacNames = new List<ZodiacName>
        {
            ZodiacName.Aries ,
            ZodiacName.Taurus,
            ZodiacName.Gemini,
            ZodiacName.Cancer,
            ZodiacName.Leo,
            ZodiacName.Virgo,
            ZodiacName.Libra,
            ZodiacName.Scorpio,
            ZodiacName.Sagittarius,
            ZodiacName.Capricorn,
            ZodiacName.Aquarius,
            ZodiacName.Pisces,
        };


        private readonly ZodiacName _signName;
        private readonly Angle _degreesInSign;

        public static ZodiacSign Empty = new ZodiacSign(ZodiacName.Empty, Angle.Zero);

        //CTOR
        public ZodiacSign(ZodiacName signName, Angle degreesInSign)
        {
            this._signName = signName;
            this._degreesInSign = degreesInSign;
        }


        //METHODS

        public ZodiacName GetSignName() => _signName;

        public Angle GetDegreesInSign() => _degreesInSign;



        //METHOD OVERRIDES
        public override bool Equals(object value)
        {

            if (value.GetType() == typeof(ZodiacSign))
            {
                //cast to type
                var parsedValue = (ZodiacSign)value;

                //check equality
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
            var hash1 = _signName.GetHashCode();
            var hash2 = _degreesInSign.GetHashCode();

            return hash1 + hash2;
        }

        public override string ToString()
        {
            //break down into common view format
            var dmg = $"{_degreesInSign?.Degrees ?? 0}° {Math.Abs(_degreesInSign?.Minutes ?? 0)}' {Math.Abs(_degreesInSign?.Seconds ?? 0)}";

            return $"{_signName} : {dmg}";
        }

        public JObject ToJson()
        {
            var temp = new JObject();
            temp["Name"] = _signName.ToString();
            temp["DegreesIn"] = _degreesInSign?.ToJson() ?? new JObject();
            return temp;
        }


    }
}