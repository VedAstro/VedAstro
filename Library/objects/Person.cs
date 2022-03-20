using System;

namespace Genso.Astrology.Library
{
    /// <summary>
    /// Simple data type to contain a person's details
    /// </summary>
    public struct Person
    {
        private readonly string _name;
        private readonly Gender _gender;
        private readonly Time _birthTime;

        //CTOR
        public Person(string name, Time birthTime, Gender gender)
        {
            _name = name;
            _birthTime = birthTime;
            _gender = gender;
        }

        
        
        //PUBLIC PROPERTIES
        public Time GetBirthDateTime() => _birthTime;
        /// <summary>
        /// Get the place of birth
        /// Note: uses the location stored in birth "Time"
        /// </summary>
        public GeoLocation GetBirthLocation() => _birthTime.GetGeoLocation();
        public string GetName() => _name;
        public Gender GetGender() => _gender;



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
            var hash1 = _name?.GetHashCode() ?? 0;
            var hash2 = _birthTime.GetHashCode();

            return hash1 + hash2;
        }

    }
}