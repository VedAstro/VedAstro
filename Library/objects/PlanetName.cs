using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Genso.Astrology.Library
{
    [Serializable()]
    public class PlanetName
    {
        //NESTED TYPES
        private enum PlanetNameEnum
        {
            Sun = 1,
            Moon,
            Mars,
            Mercury,
            Jupiter,
            Venus,
            Saturn,
            Rahu,
            Ketu
        }

        //CONST FIELDS
        public static readonly PlanetName Sun = new PlanetName(PlanetNameEnum.Sun);
        public static readonly PlanetName Moon = new PlanetName(PlanetNameEnum.Moon);
        public static readonly PlanetName Mars = new PlanetName(PlanetNameEnum.Mars);
        public static readonly PlanetName Mercury = new PlanetName(PlanetNameEnum.Mercury);
        public static readonly PlanetName Jupiter = new PlanetName(PlanetNameEnum.Jupiter);
        public static readonly PlanetName Venus = new PlanetName(PlanetNameEnum.Venus);
        public static readonly PlanetName Saturn = new PlanetName(PlanetNameEnum.Saturn);
        public static readonly PlanetName Ketu = new PlanetName(PlanetNameEnum.Ketu);
        public static readonly PlanetName Rahu = new PlanetName(PlanetNameEnum.Rahu);

        /// <summary>
        /// Gets a list of planet excluding rahu & ketu, used for looping through planets
        /// </summary>
        public static readonly List<PlanetName> AllPlanets = new List<PlanetName>()
        {
            PlanetName.Sun, PlanetName.Moon,
            PlanetName.Mars, PlanetName.Mercury,
            PlanetName.Jupiter, PlanetName.Venus,
            PlanetName.Saturn
        };


        //DATA FIELDS
        private readonly PlanetNameEnum _planetName;


        //CTOR
        private PlanetName(PlanetNameEnum planetName)
        {
            _planetName = planetName;
        }


        //METHODS
        public static PlanetName Parse(string name)
        {


            //Convert string to PlanetName type
            switch (name)
            {
                case "Sun":
                    return PlanetName.Sun;
                case "Moon":
                    return PlanetName.Moon;
                case "Mars":
                    return PlanetName.Mars;
                case "Mercury":
                    return PlanetName.Mercury;
                case "Jupiter":
                    return PlanetName.Jupiter;
                case "Venus":
                    return PlanetName.Venus;
                case "Saturn":
                    return PlanetName.Saturn;
                case "Ketu":
                    return PlanetName.Ketu;
                case "Rahu":
                    return PlanetName.Rahu;
            }


            //if could not parse assert error and return null
            Debug.Assert(false, "Could not parse planet string name!");


            return null;

        }



        //OPERATOR OVERRIDES
        public static bool operator ==(PlanetName left, PlanetName right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(PlanetName left, PlanetName right)
        {
            return !(left == right);
        }



        //METHOD OVERRIDES
        public override string ToString()
        {
            return this._planetName.ToString();
        }

        public override bool Equals(object obj)
        {

            if (obj.GetType() == typeof(PlanetName))
            {
                //cast to planet name
                var value = (PlanetName)obj;

                //Check equality
                bool returnValue = (this.GetHashCode() == value.GetHashCode());

                return returnValue;
            }
            else
            {
                //Return false if value is null
                return false;
            }

        }

        /// <summary>
        /// Gets a unique value representing the data (NOT instance)
        /// </summary>
        public override int GetHashCode()
        {
            //combine all the hash of the fields
            var hash1 = _planetName.GetHashCode();

            return hash1;
        }

    }

}