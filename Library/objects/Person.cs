using System;

namespace Genso.Astrology.Library
{
    public struct Person
    {
        private readonly string _name;
        private readonly Time _birthTime;

        //CTOR
        public Person(string name, Time birthTime)
        {
            _name = name;
            _birthTime = birthTime;
        }

        //METHODS
        public Time GetBirthDateTime()
        {
            return _birthTime;
        }



        //OVERRIDES METHODS
        public override bool Equals(object value)
        {

            if (value.GetType() == typeof(Person))
            {
                //cast to type
                var parsedValue = (Person)value;

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
            //prepare string
            var returnString = $"{this._name}";

            //return string to caller
            return returnString;
        }

        public override int GetHashCode()
        {
            //get hash of all the fields & combine them
            var hash1 = _name.GetHashCode();
            var hash2 = _birthTime.GetHashCode();

            return hash1 + hash2;
        }

    }
}