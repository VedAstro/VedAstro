using System;
using Newtonsoft.Json.Linq;

namespace VedAstro.Library
{
    /// <summary>
    /// The zodiac sign which is at a longitude
    /// </summary>
    [Serializable()]
    public struct ZodiacSign : IToJson
    {
        private readonly ZodiacName _signName;
        private readonly Angle _degreesInSign;

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
            return $"{_signName} : {_degreesInSign.Rounded}";
        }

        public JObject ToJson()
        {
            var temp = new JObject();
            temp["Name"] = _signName.ToString();
            temp["DegreesIn"] = _degreesInSign.ToJson();
            return temp;

        }

    }
}