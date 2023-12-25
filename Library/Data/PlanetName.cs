using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace VedAstro.Library
{
    /// <summary>
    /// A list of planet names, with string parsing & comparison
    /// </summary>
    [TypeConverter(typeof(PlanetNameTypeConverter))]
    public class PlanetName : IToXml, IFromUrl, IToJson
    {
        /// <summary>
        /// The number of pieces the URL version of this instance needs to be cut for processing
        /// used to segregate out the combined URL with other data
        /// EXP -> /PlanetName/Sun/ == 2 PIECES
        /// </summary>
        public static int OpenAPILength = 2;


        //NESTED TYPES
        public enum PlanetNameEnum
        {
            Empty = 0,
            Sun = 1,
            Moon,
            Mars,
            Mercury,
            Jupiter,
            Venus,
            Saturn,
            /// <summary>
            /// the true node, which is the point where the Moon's orbit crosses the ecliptic plane
            /// </summary>
            Rahu,
            /// <summary>
            ///  true osculating apogee, which is the point where the Moon is farthest from the Earth in its elliptical orbit.
            /// </summary>
            Ketu,
            Earth
        }

        //CONST FIELDS
        public static readonly PlanetName Sun = new PlanetName(PlanetNameEnum.Sun);
        public static readonly PlanetName Moon = new PlanetName(PlanetNameEnum.Moon);
        public static readonly PlanetName Mars = new PlanetName(PlanetNameEnum.Mars);
        public static readonly PlanetName Mercury = new PlanetName(PlanetNameEnum.Mercury);
        public static readonly PlanetName Jupiter = new PlanetName(PlanetNameEnum.Jupiter);
        public static readonly PlanetName Venus = new PlanetName(PlanetNameEnum.Venus);
        public static readonly PlanetName Saturn = new PlanetName(PlanetNameEnum.Saturn);
        public static readonly PlanetName Earth = new PlanetName(PlanetNameEnum.Earth);

        /// <summary>
        /// the true node, which is the point where the Moon's orbit crosses the ecliptic plane
        /// </summary>
        public static readonly PlanetName Rahu = new PlanetName(PlanetNameEnum.Rahu);
        /// <summary>
        /// true osculating apogee, which is the point where the Moon is farthest from the Earth in its elliptical orbit.
        /// </summary>
        public static readonly PlanetName Ketu = new PlanetName(PlanetNameEnum.Ketu);


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

        public static PlanetName Empty = new PlanetName(PlanetNameEnum.Empty);


        //CTOR
        private PlanetName(PlanetNameEnum planetName)
        {
            Name = planetName;
        }


        //METHODS

        /// <summary>
        /// Given a planet name in string will give back an instance, caps auto corrected
        /// </summary>
        public static PlanetName Parse(string name)
        {
            //if null or empty return empty
            if (string.IsNullOrEmpty(name)) { return Empty; }

            //Convert string to PlanetName type
            switch (name.ToLower())
            {
                case "sun":
                    return Sun;
                case "moon":
                    return Moon;
                case "mars":
                    return Mars;
                case "mercury":
                    return Mercury;
                case "jupiter":
                    return Jupiter;
                case "venus":
                    return Venus;
                case "saturn":
                    return Saturn;
                case "ketu":
                    return Ketu;
                case "rahu":
                    return Rahu;
            }

            //could not parse, return empty
            return Empty;
        }

        /// <summary>
        /// Tries to parse planet from name, caps not important
        /// </summary>
        public static bool TryParse(string possiblePlanetName, out PlanetName parsed)
        {
            switch (possiblePlanetName.ToLower())
            {
                case "sun": { parsed = PlanetName.Sun; return true; }
                case "moon": { parsed = PlanetName.Moon; return true; }
                case "mars": { parsed = PlanetName.Mars; return true; }
                case "mercury": { parsed = PlanetName.Mercury; return true; }
                case "jupiter": { parsed = PlanetName.Jupiter; return true; }
                case "venus": { parsed = PlanetName.Venus; return true; }
                case "saturn": { parsed = PlanetName.Saturn; return true; }
                case "rahu": { parsed = PlanetName.Rahu; return true; }
                case "ketu": { parsed = PlanetName.Ketu; return true; }
            }

            //could not parse
            parsed = null;
            return false;
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
        /// Given PlanetName instance in URL form will convert to instance
        /// /PlanetName/Mercury
        /// </summary>
        public static async Task<dynamic> FromUrl(string url)
        {
            // INPUT -> "/PlanetName/Mercury/"
            string[] parts = url.Split(new char[] { '/' }, StringSplitOptions.RemoveEmptyEntries);

            var parsed = PlanetName.Parse(parts[1]);

            return parsed;
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


        #region JSON SUPPORT

        JObject IToJson.ToJson() => (JObject)this.ToJson();

        public JToken ToJson()
        {
            var temp = new JObject();
            temp["Name"] = this.Name.ToString();

            return temp;
        }

        /// <summary>
        /// Given a json list of person will convert to instance
        /// used for transferring between server & client
        /// </summary>
        public static List<PlanetName> FromJsonList(JToken personList)
        {
            //if null empty list please
            if (personList == null) { return new List<PlanetName>(); }

            var returnList = new List<PlanetName>();

            foreach (var personJson in personList)
            {
                returnList.Add(PlanetName.FromJson(personJson));
            }

            return returnList;
        }

        public static JArray ToJsonList(List<PlanetName> planetNameList)
        {
            var jsonList = new JArray();

            foreach (var eventTag in planetNameList)
            {
                jsonList.Add(eventTag.ToString());
            }

            return jsonList;
        }

        public static PlanetName FromJson(JToken planetInput)
        {
            //if null return empty, end here
            if (planetInput == null) { return PlanetName.Empty; }

            try
            {
                var nameStr = planetInput["Name"].Value<string>();
                var name = (PlanetNameEnum)Enum.Parse(typeof(PlanetNameEnum), nameStr);

                var parsedHoroscope = new PlanetName(name);

                return parsedHoroscope;
            }
            catch (Exception e)
            {
                LibLogger.Error($"Failed to parse:\n{planetInput.ToString()}");

                return PlanetName.Empty;
            }

        }

        #endregion



        //OPERATOR OVERRIDES
        public static bool operator ==(PlanetName left, PlanetName right)
        {
            //check for nulls
            if (ReferenceEquals(left, null))
            {
                return ReferenceEquals(right, null);
            }

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
            //nulls is false
            if (ReferenceEquals(null, obj)) return false;

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



    }


    /// <summary>
    /// Special class to allow direct use Planet Name in blazor
    /// </summary>
    public class PlanetNameTypeConverter : TypeConverter
    {
        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
        {
            return sourceType == typeof(string) || base.CanConvertFrom(context, sourceType);
        }

        /// <summary>
        /// Tells blazor how to auto convert string to Planet Name
        /// </summary>
        public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
        {
            if (value is string stringValue)
            {
                return PlanetName.Parse(stringValue);
            }
            return base.ConvertFrom(context, culture, value);
        }
    }
}