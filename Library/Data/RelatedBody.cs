using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Xml.Linq;

namespace VedAstro.Library
{

    /// <summary>
    /// Simple class to wrap all related bodies (Planets, Houses & Signs)
    /// to Calculator Result
    /// </summary>
    public class RelatedBody : IToXml, IToJson
    {
        public static readonly RelatedBody Empty = new RelatedBody();


        /// <summary>
        /// List of planets related to this calculation result
        /// </summary>
        public List<PlanetName> RelatedPlanets { get; private set; } = new List<PlanetName>();

        /// <summary>
        /// List of Zodiac Signs related to this calculation result
        /// </summary>
        public List<ZodiacName> RelatedZodiac { get; private set; } = new List<ZodiacName>();

        /// <summary>
        /// List of houses related to this calculation result
        /// </summary>
        public List<HouseName> RelatedHouses { get; private set; } = new List<HouseName>();

        /// <summary>
        /// Checks if given planet is in List
        /// </summary>
        public bool Contains(PlanetName planetName) => RelatedPlanets.Contains(planetName);
        public bool Contains(ZodiacName zodiacName) => RelatedZodiac.Contains(zodiacName);
        public bool Contains(HouseName houseName) => RelatedHouses.Contains(houseName);

        /// <summary>
        /// Converts all values to string separated by comma
        /// Note: used when searching
        /// </summary>
        public override string ToString()
        {
            var compiledText = "";

            //this assumed that underlying type implements its own to string
            compiledText += Tools.ListToString(this.RelatedPlanets);
            compiledText += Tools.ListToString(this.RelatedHouses);
            compiledText += Tools.ListToString(this.RelatedZodiac);

            return compiledText;
        }

        public dynamic FromXml<T>(XElement relatedBodyXml) where T : IToXml => RelatedBody.FromXml(relatedBodyXml);

        public static RelatedBody FromXml(XElement relatedBodyXml)
        {
            var parsed = new RelatedBody();
            var relatedPlanetsXml = relatedBodyXml?.Element("PlanetNameList") ?? new XElement("PlanetNameList");
            parsed.RelatedPlanets = PlanetName.FromXmlList(relatedPlanetsXml);
            var relatedHousesXml = relatedBodyXml?.Element("HouseNameList") ?? new XElement("HouseNameList");
            parsed.RelatedHouses = HouseNameExtensions.FromXmlList(relatedHousesXml);
            var relatedZodiacXml = relatedBodyXml?.Element("ZodiacNameList") ?? new XElement("ZodiacNameList");
            parsed.RelatedZodiac = ZodiacNameExtensions.FromXmlList(relatedZodiacXml);

            return parsed;

        }

        public XElement ToXml()
        {
            var relatedPlanetsXml = PlanetName.ToXmlList(this.RelatedPlanets);
            var relatedHousesXml = HouseNameExtensions.ToXmlList(this.RelatedHouses);
            var relatedSignsXml = ZodiacNameExtensions.ToXmlList(this.RelatedZodiac);

            var returnXml = new XElement("RelatedBody", relatedPlanetsXml, relatedHousesXml, relatedSignsXml);
            return returnXml;

        }


        #region JSON SUPPORT

        JObject IToJson.ToJson() => (JObject)this.ToJson();

        public JToken ToJson()
        {
            var temp = new JObject();
            temp["Planets"] = PlanetName.ToJsonList(this.RelatedPlanets);
            temp["Houses"] = HouseNameExtensions.ToJsonList(this.RelatedHouses);
            temp["Zodiacs"] = ZodiacNameExtensions.ToJsonList(this.RelatedZodiac);

            return temp;
        }

        /// <summary>
        /// Given a json list of person will convert to instance
        /// used for transferring between server & client
        /// </summary>
        public static List<RelatedBody> FromJsonList(JToken personList)
        {
            //if null empty list please
            if (personList == null) { return new List<RelatedBody>(); }

            var returnList = new List<RelatedBody>();

            foreach (var personJson in personList)
            {
                returnList.Add(RelatedBody.FromJson(personJson));
            }

            return returnList;
        }

        public static RelatedBody FromJson(JToken horoscopeInput)
        {
            //if null return empty, end here
            if (horoscopeInput == null) { return RelatedBody.Empty; }

            try
            {
                var relatedPlanets = PlanetName.FromJsonList(horoscopeInput["Planets"]);
                var relatedHouses = HouseNameExtensions.FromJsonList(horoscopeInput["Houses"]);
                var relatedZodiacs = ZodiacNameExtensions.FromJsonList(horoscopeInput["Zodiacs"]);

                var parsedHoroscope = new RelatedBody()
                {
                    RelatedPlanets = relatedPlanets,
                    RelatedHouses = relatedHouses,
                    RelatedZodiac = relatedZodiacs
                };

                return parsedHoroscope;
            }
            catch (Exception e)
            {
                LibLogger.Debug($"Failed to parse Person:\n{horoscopeInput.ToString()}");

                return RelatedBody.Empty;
            }

        }


        #endregion

    }
}
