using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using System.Web;
using System.Xml.Linq;
using Newtonsoft.Json.Linq;

namespace VedAstro.Library
{
    /// <summary>
    /// Simple data type to hold info on life events of a person
    /// This event is to mark important moments in a persons life
    /// This is used later by calculators like Dasa to show against astrological predictions
    /// </summary>
    public class LifeEvent : IToXml
    {
        private string _name;
        private string _description;



        //░█▀▀▄ ─█▀▀█ ▀▀█▀▀ ─█▀▀█ 
        //░█─░█ ░█▄▄█ ─░█── ░█▄▄█ 
        //░█▄▄▀ ░█─░█ ─░█── ░█─░█
        //NOTE :
        //JsonPropertyName used when auto converting
        //type to Json when interoperating with JS

        /// <summary>
        /// Custom name of the event auto formatted to be HTML save
        /// Note: since all view of this in HTML, special charters
        /// like "&" will cause errors otherwise 
        /// </summary>
        [JsonPropertyName("Name")]
        public string Name
        {
            get => HttpUtility.HtmlDecode(_name);
            set => _name = HttpUtility.HtmlEncode(value);
        }

        /// <summary>
        /// Description of life event
        /// HTML safety included
        /// </summary>
        [JsonPropertyName("Description")]
        public string Description
        {
            get => HttpUtility.HtmlDecode(_description);
            set => _description = HttpUtility.HtmlEncode(value);
        }

        /// <summary>
        /// Format : 10:10 10/10/2010
        /// Note the absence of offset, compensated by location
        /// </summary>
        [JsonPropertyName("StartTime")]
        public string StartTime { get; set; }

        /// <summary>
        /// Must follow standard time formatting 
        /// </summary>
        [JsonPropertyName("Location")]
        public string Location { get; set; }

        /// <summary>
        /// must be Good, Neutral or Bad only
        /// </summary>
        [JsonPropertyName("Nature")]
        public string Nature { get; set; }

        /// <summary>
        /// Auto set by code when not available using Google API
        /// </summary>
        [JsonPropertyName("Timezone")]
        public string Timezone { get; set; } = ""; //detect empty string later

        ///// </summary>
        //[JsonPropertyName("Timezone")]
        //public string Timezone
        //{
        //    get
        //    {
        //        //get from API if no saved 
        //        var savedExists = !string.IsNullOrEmpty(_timezone);
        //        if (!savedExists)
        //        {
        //            //get timezone from Google API
        //            var lifeEvtTimeNoTimezone = DateTime.ParseExact(this.StartTime, Time.DateTimeFormatNoTimezone, null);
        //            _timezone = Tools.GetTimezoneOffsetString(this.Location, lifeEvtTimeNoTimezone, "AIzaSyDqBWCqzU1BJenneravNabDUGIHotMBsgE");
        //        }

        //        return _timezone;
        //    }
        //    set => _timezone = value;
        //}


        //░█▀▄▀█ ░█▀▀▀ ▀▀█▀▀ ░█─░█ ░█▀▀▀█ ░█▀▀▄ ░█▀▀▀█ 
        //░█░█░█ ░█▀▀▀ ─░█── ░█▀▀█ ░█──░█ ░█─░█ ─▀▀▀▄▄ 
        //░█──░█ ░█▄▄▄ ─░█── ░█─░█ ░█▄▄▄█ ░█▄▄▀ ░█▄▄▄█


        public JObject ToJson()
        {
            var temp = new JObject();
            temp["Name"] = this.Name;
            temp["StartTime"] = this.StartTime;
            temp["Timezone"] = this.Timezone;
            temp["Location"] = this.Location;
            temp["Description"] = this.Description;
            temp["Nature"] = this.Nature;

            //compile into an JSON array
            return temp;

        }


        public XElement ToXml()
        {
            var lifeEventXml = new XElement("LifeEvent");
            var nameXml = new XElement("Name", this.Name);
            var startTimeXml = new XElement("StartTime", this.StartTime);
            var timezoneXml = new XElement("Timezone", this.Timezone);
            var locationXml = new XElement("Location", this.Location);
            var descriptionXml = new XElement("Description", this.Description);
            var natureXml = new XElement("Nature", this.Nature);

            lifeEventXml.Add(nameXml, startTimeXml, timezoneXml, locationXml, descriptionXml, natureXml);

            return lifeEventXml;
        }

        /// <summary>
        /// The root element is expected to be LifeEvent
        /// Note: Special method done to implement IToXml
        /// </summary>
        public dynamic FromXml<T>(XElement xml) where T : IToXml => FromXml(xml);

        /// <summary>
        /// The root element is expected to be Person
        /// </summary>
        public static LifeEvent FromXml(XElement lifeEventXml)
        {
            var lifeEventParsed = new LifeEvent();

            //try get data from xml else use empty string
            lifeEventParsed.Name = !string.IsNullOrEmpty(lifeEventXml.Element("Name")?.Value) ? lifeEventXml?.Element("Name")?.Value : "";
            lifeEventParsed.StartTime = !string.IsNullOrEmpty(lifeEventXml.Element("StartTime")?.Value) ? lifeEventXml?.Element("StartTime")?.Value : "";
            lifeEventParsed.Timezone = !string.IsNullOrEmpty(lifeEventXml.Element("Timezone")?.Value) ? lifeEventXml?.Element("Timezone")?.Value : ""; //keep it empty so that can be detected and filled in later
            const string defaultLocation = "Singapore"; //backup location for error free operation
            lifeEventParsed.Location = !string.IsNullOrEmpty(lifeEventXml.Element("Location")?.Value) ? lifeEventXml?.Element("Location")?.Value : defaultLocation;
            lifeEventParsed.Description = !string.IsNullOrEmpty(lifeEventXml.Element("Description")?.Value) ? lifeEventXml?.Element("Description")?.Value : "";
            lifeEventParsed.Nature = !string.IsNullOrEmpty(lifeEventXml.Element("Nature")?.Value) ? lifeEventXml?.Element("Nature")?.Value : "";


            return lifeEventParsed;

        }

        /// <summary>
        /// input is json array
        /// </summary>
        public static List<LifeEvent> FromJsonList(JToken lifeEventList)
        {
            var returnList = new List<LifeEvent>();

            foreach (var lifeEvent in lifeEventList)
            {
                var temp = new LifeEvent();

                temp.Name = lifeEvent["Name"].Value<string>();
                temp.StartTime = lifeEvent["StartTime"].Value<string>();
                temp.Timezone = lifeEvent["Timezone"].Value<string>();
                temp.Location = lifeEvent["Location"].Value<string>();
                temp.Description = lifeEvent["Description"].Value<string>();
                temp.Nature = lifeEvent["Nature"].Value<string>();

                returnList.Add(temp);
            }

            return returnList;
        }





        //░█▀▀▀█ ░█──░█ ░█▀▀▀ ░█▀▀█ ░█▀▀█ ▀█▀ ░█▀▀▄ ░█▀▀▀ 　 ░█▀▄▀█ ░█▀▀▀ ▀▀█▀▀ ░█─░█ ░█▀▀▀█ ░█▀▀▄ ░█▀▀▀█ 
        //░█──░█ ─░█░█─ ░█▀▀▀ ░█▄▄▀ ░█▄▄▀ ░█─ ░█─░█ ░█▀▀▀ 　 ░█░█░█ ░█▀▀▀ ─░█── ░█▀▀█ ░█──░█ ░█─░█ ─▀▀▀▄▄ 
        //░█▄▄▄█ ──▀▄▀─ ░█▄▄▄ ░█─░█ ░█─░█ ▄█▄ ░█▄▄▀ ░█▄▄▄ 　 ░█──░█ ░█▄▄▄ ─░█── ░█─░█ ░█▄▄▄█ ░█▄▄▀ ░█▄▄▄█


        public override bool Equals(object value)
        {

            if (value.GetType() == typeof(LifeEvent))
            {
                //cast to type
                var parsedValue = (LifeEvent)value;

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
            var returnString = $"{this.Name} - {this.Nature} - {this.StartTime} - {this.Location} - {this.Description}";

            //return string to caller
            return returnString;
        }

        /// <summary>
        /// Name & Birth Time are used to generate Hash
        /// </summary>
        public override int GetHashCode()
        {
            //get hash of all the fields & combine them
            var hash1 = Tools.GetStringHashCode(this.Name);
            var hash2 = Tools.GetStringHashCode(this.StartTime);
            var hash3 = Tools.GetStringHashCode(this.Timezone);
            var hash4 = Tools.GetStringHashCode(this.Location);
            var hash5 = Tools.GetStringHashCode(this.Description);
            var hash6 = Tools.GetStringHashCode(this.Nature);

            //take out negative before returning
            return Math.Abs(hash1 + hash2 + hash3 + hash4 + hash5 + hash6);
        }


        /// <summary>
        /// TODO all events should have offset data on entry, as such google API here is a hack
        /// TODO MARKED FOR DELETION use non async version
        /// Gets the exact time with offset at the place where this event happened
        /// Parses a event start time into DateTimeOffset
        /// uses GOOGLE API
        /// NOTE:
        /// - standard time (STD) at the location at that time
        /// </summary>
        public async Task<DateTimeOffset> GetDateTimeOffsetAsync()
        {
            //get timezone from api and save it to local instance so that it can saved later
            //only use API if timezone data not yet set, to save unnecessary slow calls to Google
            this.Timezone = string.IsNullOrEmpty(this.Timezone)
                ? await Tools.GetTimezoneOffsetString(this.Location, this.StartTime)
                : this.Timezone;

            //get start time of life event and find the position of it in slices (same as now line)
            //so that this life event line can be placed exactly on the report where it happened
            var lifeEvtTimeStr = $"{this.StartTime} {this.Timezone}"; //add offset 0 only for parsing, not used by API to get timezone
            var lifeEvtTime = DateTimeOffset.ParseExact(lifeEvtTimeStr, Time.DateTimeFormat, null);

            return lifeEvtTime;
        }

        public DateTimeOffset GetDateTimeOffset()
        {
            //get timezone from api and save it to local instance so that it can saved later
            //only use API if timezone data not yet set, to save unnecessary slow calls to Google
            this.Timezone = string.IsNullOrEmpty(this.Timezone)
                ? throw new Exception($"Timezone data for event \"{this.Name}\" missing!")
                : this.Timezone;

            //get start time of life event and find the position of it in slices (same as now line)
            //so that this life event line can be placed exactly on the report where it happened
            var lifeEvtTimeStr = $"{this.StartTime} {this.Timezone}"; //add offset 0 only for parsing, not used by API to get timezone
            var lifeEvtTime = DateTimeOffset.ParseExact(lifeEvtTimeStr, Time.DateTimeFormat, null);

            return lifeEvtTime;
        }


        /// <summary>
        /// Gets exact time event occurred without API
        /// note: if timezone not filled, time zone set to +00:00
        /// </summary>
        /// <returns></returns>
        public DateTimeOffset GetDateTimeOffsetLocal()
        {
            //used saved time zone or use the birth time zone as quick backup
            this.Timezone = string.IsNullOrEmpty(this.Timezone)
                ? "+00:00"
                : this.Timezone;

            //get start time of life event and find the position of it in slices (same as now line)
            //so that this life event line can be placed exactly on the report where it happened
            var lifeEvtTimeStr = $"{this.StartTime} {this.Timezone}"; //add offset 0 only for parsing, not used by API to get timezone
            var lifeEvtTime = DateTimeOffset.ParseExact(lifeEvtTimeStr, Time.DateTimeFormat, null);

            return lifeEvtTime;
        }


        /// <summary>
        /// Note this call uses Google API everytime
        /// </summary>
        /// <returns></returns>
        public async Task<Time> GetTime()
        {
            var stdTimeLocation = await this.GetDateTimeOffsetAsync();
            var location = await this.GetGeoLocation();
            var newTime = new Time(stdTimeLocation, location);
            return newTime;
        }

        /// <summary>
        /// Gets geo location instance for this event, uses Google API
        /// </summary>
        /// <returns></returns>
        public async Task<GeoLocation> GetGeoLocation() => await GeoLocation.FromName(this.Location);

        /// <summary>
        /// compare logic to sort according to time
        /// greater = -1
        /// equal = 0
        /// lesser = 1
        /// </summary>
        public int CompareTo(LifeEvent lifeEvent)
        {
            var inputTime = lifeEvent.GetDateTimeOffsetLocal();

            var currentTime = this.GetDateTimeOffsetLocal();

            //compare with time
            return currentTime.CompareTo(inputTime);
        }


    }
}
