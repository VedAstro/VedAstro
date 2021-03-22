using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Genso.Astrology.Library
{
    /// <summary>
    /// Simple data type to encapsulate 
    /// </summary>
    public struct Shashtiamsa
    {
        //CONST FIELDS
        public static readonly Shashtiamsa Zero = new Shashtiamsa(0);


        //DATA FIELDS
        private double _shashtiamsaAsDouble;



        //CTOR
        public Shashtiamsa(double shashtiamsa)
        {
            _shashtiamsaAsDouble = shashtiamsa;
        }


        //METHODS
        public double ToDouble() => _shashtiamsaAsDouble;

        //This divided by 60 will give shashtiamsa in rupas
        public double ToRupa() => _shashtiamsaAsDouble / 60;


        //OPERATOR OVERRIDES
        public static Shashtiamsa operator +(Shashtiamsa left, Shashtiamsa right)
        {
            var totalShashtiamsaAsDouble = left._shashtiamsaAsDouble + right._shashtiamsaAsDouble;

            return new Shashtiamsa(totalShashtiamsaAsDouble);
        }

        public static Shashtiamsa operator -(Shashtiamsa left, Shashtiamsa right)
        {
            var totalShashtiamsaAsDouble = left._shashtiamsaAsDouble - right._shashtiamsaAsDouble;

            return new Shashtiamsa(totalShashtiamsaAsDouble);
        }




        //METHOD OVERRIDES
        public override bool Equals(object value)
        {

            if (value.GetType() == typeof(Shashtiamsa))
            {
                //cast to type
                var parsedValue = (Shashtiamsa)value;

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
            var hash1 = _shashtiamsaAsDouble.GetHashCode();

            return hash1;
        }

        public override string ToString()
        {
            return $"{_shashtiamsaAsDouble}";
        }

    }

}
