using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;


//█▄█ █▀█ █░█ ▀ █▀█ █▀▀   ▄▀█ █░░ █░█░█ ▄▀█ █▄█ █▀   █░█░█ █ ▀█▀ █░█   █▄█ █▀█ █░█ █▀█   █▀ █░█░█ █▀▀ █▀▀ ▀█▀
//░█░ █▄█ █▄█ ░ █▀▄ ██▄   █▀█ █▄▄ ▀▄▀▄▀ █▀█ ░█░ ▄█   ▀▄▀▄▀ █ ░█░ █▀█   ░█░ █▄█ █▄█ █▀▄   ▄█ ▀▄▀▄▀ ██▄ ██▄ ░█░

//█░█ █▀▀ ▄▀█ █▀█ ▀█▀ ░   ░░█ █░█ █▀ ▀█▀   █▀▀ ▄▀█ █▄░█ ▀ ▀█▀   █▀ █▀▀ █▀▀   █ ▀█▀
//█▀█ ██▄ █▀█ █▀▄ ░█░ █   █▄█ █▄█ ▄█ ░█░   █▄▄ █▀█ █░▀█ ░ ░█░   ▄█ ██▄ ██▄   █ ░█░


namespace VedAstro.Library
{
    /// <summary>
    /// A logical wrapper around Event class to make specific to Dasas
    /// </summary>
    public class DasaEvent(Event sourceEvent)
    {

        /// <summary>
        /// based on PD tag will get dasa level number
        /// </summary>
        public int DasaLevel
        {
            get
            {
                //based on PD tag set the name of sub dasa
                //Dasa > Bhukti > Antaram > Sukshma > Prana > Avi Prana > Viprana
                if (sourceEvent.EventTags.Contains(EventTag.PD1)) { return 1; }
                if (sourceEvent.EventTags.Contains(EventTag.PD2)) { return 2; }
                if (sourceEvent.EventTags.Contains(EventTag.PD3)) { return 3; }
                if (sourceEvent.EventTags.Contains(EventTag.PD4)) { return 4; }
                if (sourceEvent.EventTags.Contains(EventTag.PD5)) { return 5; }
                if (sourceEvent.EventTags.Contains(EventTag.PD6)) { return 6; }
                if (sourceEvent.EventTags.Contains(EventTag.PD7)) { return 7; }

                throw new Exception("Dasa level not found");
            }
        }

        /// <summary>
        /// based on PD tag will get dasa level name
        /// </summary>
        public string DasaName
        {
            get
            {
                //based on PD tag set the name of sub dasa
                //Dasa > Bhukti > Antaram > Sukshma > Prana > Avi Prana > Viprana
                if (sourceEvent.EventTags.Contains(EventTag.PD1)) { return "Dasa"; }
                if (sourceEvent.EventTags.Contains(EventTag.PD2)) { return "Bhukti"; }
                if (sourceEvent.EventTags.Contains(EventTag.PD3)) { return "Antaram"; }
                if (sourceEvent.EventTags.Contains(EventTag.PD4)) { return "Sukshma"; }
                if (sourceEvent.EventTags.Contains(EventTag.PD5)) { return "Prana"; }
                if (sourceEvent.EventTags.Contains(EventTag.PD6)) { return "Avi Prana"; }
                if (sourceEvent.EventTags.Contains(EventTag.PD7)) { return "Viprana"; }

                throw new Exception("Dasa level not found");
            }
        }

        /// <summary>
        /// Duration in hours
        /// </summary>
        public double Duration => sourceEvent.DurationHour;

        /// <summary>
        /// Description of the event
        /// </summary>
        public string Description => sourceEvent.Description;
        public Time StartTime => sourceEvent.StartTime;
        public Time EndTime => sourceEvent.EndTime;

        public EventNature Nature => sourceEvent.Nature;

        /// <summary>
        /// Name of the planet lord of the sub dasa/dasa
        /// </summary>
        public PlanetName Lord
        {
            get
            {
                //1st word is Parent, 2nd word is Current
                //try get 2nd word as planet name
                var stringName = sourceEvent.Name.ToString();
                var planetName = Tools.GetCamelCaseWord(stringName, 1);

                //try parse planet name, if fail will return empty Planet
                var parsed = PlanetName.Parse(planetName);

                //not found, return empty
                return parsed;

            }
        }

        /// <summary>
        /// Planet lord of the parent sub dasa/dasa
        /// </summary>
        public PlanetName ParentLord
        {
            get
            {
                //try get 2nd word as planet name, else get second word
                var stringName = sourceEvent.Name.ToString();
                var planetName = Tools.GetCamelCaseWord(stringName, 0);

                //try parse planet name, if fail will return empty Planet
                var parsed = PlanetName.Parse(planetName);

                //not found, return empty
                return parsed;
            }
        }

        public Event SourceEvent => sourceEvent;


        #region JSON SUPPORT

        //TODO MARKED FOR DEPRECATION
        public JToken ToJson()
        {
            var temp = new JObject();

            //NOTE: dasa name has to be intelligently converted to classic dasa name
            //exp: "JupiterSunPD3" -> "Jupiter Antaram" 
            temp["Name"] = ConvertToClassicDasaName(sourceEvent);
            temp["FullName"] = sourceEvent.Name.ToString();
            temp["StartTime"] = sourceEvent.StartTime.GetStdDateTimeOffsetText();
            temp["EndTime"] = sourceEvent.EndTime.GetStdDateTimeOffsetText();
            temp["DurationHour"] = sourceEvent.DurationHour;

            return temp;
        }

        /// <summary>
        /// dasa name has to be intelligently converted to classic dasa name
        /// exp: "JupiterSunPD3" -> "Jupiter Antaram" 
        /// </summary>
        private string ConvertToClassicDasaName(Event inputEvent)
        {
            var newName = "";

            //extract out planet for sub dasa period
            //exp: "JupiterSunPD3" --> "Jupiter"
            var dasaLord = Tools.GetCamelCaseWord(inputEvent.Name.ToString());
            newName += dasaLord;

            //add space between
            newName += " ";

            //based on PD tag set the name of sub dasa
            //Dasa > Bhukti > Antaram > Sukshma > Prana > Avi Prana > Viprana
            newName += this.DasaName;

            return newName;
        }



        /// <summary>
        /// Given a json list of person will convert to instance
        /// used for transferring between server & client
        /// </summary>
        public static List<DasaEvent> FromJsonList(JToken personList)
        {
            //if null empty list please
            if (personList == null) { return new List<DasaEvent>(); }

            var returnList = new List<DasaEvent>();

            foreach (var personJson in personList)
            {
                returnList.Add(DasaEvent.FromJson(personJson));
            }

            return returnList;
        }

        public static JArray ToJsonList(List<DasaEvent> eventList)
        {
            var jsonList = new JArray();

            foreach (var eventInstance in eventList)
            {
                jsonList.Add(eventInstance.ToJson());
            }

            return jsonList;
        }

        public static DasaEvent FromJson(JToken planetInput)
        {

            throw new NotImplementedException();

        }

        #endregion

    }
}