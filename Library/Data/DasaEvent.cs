using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Runtime.Intrinsics.X86;
using System.Security.Cryptography;
using System.Text.RegularExpressions;
using System.Web;
using System.Xml.Linq;


//█▄█ █▀█ █░█ ▀ █▀█ █▀▀   ▄▀█ █░░ █░█░█ ▄▀█ █▄█ █▀   █░█░█ █ ▀█▀ █░█   █▄█ █▀█ █░█ █▀█   █▀ █░█░█ █▀▀ █▀▀ ▀█▀
//░█░ █▄█ █▄█ ░ █▀▄ ██▄   █▀█ █▄▄ ▀▄▀▄▀ █▀█ ░█░ ▄█   ▀▄▀▄▀ █ ░█░ █▀█   ░█░ █▄█ █▄█ █▀▄   ▄█ ▀▄▀▄▀ ██▄ ██▄ ░█░

//█░█ █▀▀ ▄▀█ █▀█ ▀█▀ ░   ░░█ █░█ █▀ ▀█▀   █▀▀ ▄▀█ █▄░█ ▀ ▀█▀   █▀ █▀▀ █▀▀   █ ▀█▀
//█▀█ ██▄ █▀█ █▀▄ ░█░ █   █▄█ █▄█ ▄█ ░█░   █▄▄ █▀█ █░▀█ ░ ░█░   ▄█ ██▄ ██▄   █ ░█░


namespace VedAstro.Library
{
    /// <summary>
    /// Represents a period of time "Event" with start, end time and data related
    /// </summary>
    public class DasaEvent(Event sourceEvent)
    {



        #region JSON SUPPORT

        public JToken ToJson()
        {
            var temp = new JObject();

            //NOTE: dasa name has to be intelligently converted to classic dasa name
            //exp: "JupiterSunPD3" -> "Jupiter Antaram" 
            temp["Name"] = ConvertToClassicDasaName(sourceEvent);
            temp["FullName"] = sourceEvent.Name.ToString();
            temp["StartTime"] = sourceEvent.StartTime.GetStdDateTimeOffsetText();
            temp["EndTime"] = sourceEvent.EndTime.GetStdDateTimeOffsetText();
            temp["Duration"] = sourceEvent.DurationMin;

            return temp;
        }

        /// <summary>
        /// dasa name has to be intelligently converted to classic dasa name
        /// exp: "JupiterSunPD3" -> "Jupiter Antaram" 
        /// </summary>
        private static string ConvertToClassicDasaName(Event inputEvent)
        {
            var newName = "";

            //extract out planet for sub dasa period
            //exp: "JupiterSunPD3" --> "Jupiter"
            var dasaLord = Tools.GetFirstCamelCaseWord(inputEvent.Name.ToString());
            newName += dasaLord;

            //add space between
            newName += " ";

            //based on PD tag set the name of sub dasa
            //Dasa > Bhukti > Antaram > Sukshma > Prana > Avi Prana > Viprana
            if (inputEvent.EventTags.Contains(EventTag.PD1)) { newName += "Dasa"; }
            if (inputEvent.EventTags.Contains(EventTag.PD2)) { newName += "Bhukti"; }
            if (inputEvent.EventTags.Contains(EventTag.PD3)) { newName += "Antaram"; }
            if (inputEvent.EventTags.Contains(EventTag.PD4)) { newName += "Sukshma"; }
            if (inputEvent.EventTags.Contains(EventTag.PD5)) { newName += "Prana"; }
            if (inputEvent.EventTags.Contains(EventTag.PD6)) { newName += "Avi Prana"; }
            if (inputEvent.EventTags.Contains(EventTag.PD7)) { newName += "Viprana"; }

            return newName;
        }



        /// <summary>
        /// Given a json list of person will convert to instance
        /// used for transferring between server & client
        /// </summary>
        public  static List<DasaEvent> FromJsonList(JToken personList)
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

        public  static JArray ToJsonList(List<DasaEvent> eventList)
        {
            var jsonList = new JArray();

            foreach (var eventInstance in eventList)
            {
                jsonList.Add(eventInstance.ToJson());
            }

            return jsonList;
        }

        public  static DasaEvent FromJson(JToken planetInput)
        {

            throw new NotImplementedException();

        }

        #endregion


    }
}