using System;

namespace Genso.Astrology.Library
{
    [Serializable()]
    public struct Tarabala
    {
        private readonly TarabalaName _name;
        private readonly int _cycle;

        //CTOR
        public Tarabala(int tarabalaNumber, int cycle)
        {
            //convert number to tarabala name
            _name = (TarabalaName)tarabalaNumber;
            //save cycle
            _cycle = cycle;
        }

        //METHODS
        public TarabalaName GetName()
        {
            return _name;
        }
        public int GetCycle()
        {
            return _cycle;
        }



        //METHOD OVERRIDES
        public override bool Equals(object obj)
        {
            //return false if obj is null
            if (obj == null) return false;

            //check if obj is of type Tarabala
            if (obj.GetType() == typeof(Tarabala))
            {
                //cast to type
                var parsedValue = (Tarabala)obj;

                //Check equality
                bool returnValue = (this.GetHashCode() == parsedValue.GetHashCode());

                return returnValue;
            }

            //if obj is not tarabala type return false
            return false;
        }

        public override string ToString()
        {

            return $"{_name} - Cycle:{_cycle}";
        }

        public override int GetHashCode()
        {
            //get hash of all the fields & combine them
            var hash1 = _name.GetHashCode();
            var hash2 = _cycle.GetHashCode();

            return hash1 + hash2;
        }


        //OPERATOR OVERRIDES
        public static bool operator ==(Tarabala left, Tarabala right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(Tarabala left, Tarabala right)
        {
            return !(left == right);
        }
    }
}