using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Xml.Linq;

namespace Genso.Astrology.Library
{
    /// <summary>
    /// A list of planet names, with string parsing & comparison
    /// </summary>
    [Serializable()]
    public class PlanetName : IToXml
    {
        //NESTED TYPES
        public enum PlanetNameEnum
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
        public static readonly List<PlanetName> All7Planets = new List<PlanetName>()
        {
            Sun, Moon,
            Mars, Mercury,
            Jupiter, Venus,
            Saturn
        };

        /// <summary>
        /// Gets a list of planet WITH rahu & ketu, used for looping through planets
        /// </summary>
        public static readonly List<PlanetName> All9Planets = new List<PlanetName>()
        {
            Sun, Moon,
            Mars, Mercury,
            Jupiter, Venus,
            Saturn, Rahu, Ketu
        };



        //DATA FIELDS
        public PlanetNameEnum Name { get; }


        //CTOR
        private PlanetName(PlanetNameEnum planetName)
        {
            Name = planetName;
        }


        //METHODS
        public static PlanetName Parse(string name)
        {


            //Convert string to PlanetName type
            switch (name)
            {
                case "Sun":
                    return Sun;
                case "Moon":
                    return Moon;
                case "Mars":
                    return Mars;
                case "Mercury":
                    return Mercury;
                case "Jupiter":
                    return Jupiter;
                case "Venus":
                    return Venus;
                case "Saturn":
                    return Saturn;
                case "Ketu":
                    return Ketu;
                case "Rahu":
                    return Rahu;
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
        public override string ToString() => Name.ToString();

        public override bool Equals(object obj)
        {

            if (obj.GetType() == typeof(PlanetName))
            {
                //cast to planet name
                var value = (PlanetName)obj;

                //Check equality
                bool returnValue = (GetHashCode() == value.GetHashCode());

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
            var hash1 = Name.GetHashCode();

            return hash1;
        }

        public XElement ToXml()
        {
            var planetNameHolder = new XElement("PlanetName");
            var nameXml = new XElement("Name", this.Name.ToString());

            planetNameHolder.Add(nameXml);

            return planetNameHolder;

        }

        public dynamic FromXml<T>(XElement planetNameXml) where T : IToXml => FromXml(planetNameXml);

        public static PlanetName FromXml(XElement planetNameXml)
        {
            var nameRaw = planetNameXml?.Element("Name")?.Value ?? "Empty";
            var planetName = Enum.Parse<PlanetNameEnum>(nameRaw);
            return new PlanetName(planetName);
        }

        /// <summary>
        /// Note: Root element must be named EventTagList
        /// </summary>
        public static List<PlanetName> FromXmlList(XElement planetNameListXml)
        {
            var returnList = new List<PlanetName>();
            foreach (var planetNameXml in planetNameListXml.Elements())
            {
                returnList.Add(PlanetName.FromXml(planetNameXml));
            }
            return returnList;
        }

        /// <summary>
        /// Note: Root element must be named PlanetNameList
        /// </summary>
        public static XElement ToXmlList(List<PlanetName> planetNameList)
        {
            var planetNameListXml = new XElement("PlanetNameList");

            foreach (var planetName in planetNameList)
            {
                planetNameListXml.Add(planetName.ToXml());
            }

            return planetNameListXml;
        }

    }

}