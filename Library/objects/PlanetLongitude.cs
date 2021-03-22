namespace Genso.Astrology.Library
{
    public struct PlanetLongitude
    {
        //FIELDS
        private PlanetName _planetName;
        private Angle _planetLongitude;



        //CTOR
        public PlanetLongitude(PlanetName planetName, Angle planetLongitude)
        {
            _planetName = planetName;
            _planetLongitude = planetLongitude;
        }



        //PUBLIC METHODS
        public PlanetName GetPlanetName() => _planetName;

        public Angle GetPlanetLongitude() => _planetLongitude;



        //METHOD OVERRIDES
        public override bool Equals(object value)
        {

            if (value.GetType() == typeof(PlanetLongitude))
            {
                //cast to type
                var parsedValue = (PlanetLongitude)value;

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
            var hash1 = _planetName.GetHashCode();
            var hash2 = _planetLongitude.GetHashCode();

            return hash1 + hash2;
        }

        public override string ToString()
        {
            return $"{_planetName} - {_planetLongitude}";
        }

    }
}