using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using System.Web;
using System.Xml.Linq;

namespace Genso.Astrology.Library
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
            get => HttpUtility.HtmlEncode(_name);//in-case was edited outside code
            set => _name = HttpUtility.HtmlEncode(value);
        }

        /// <summary>
        /// Description of life event
        /// HTML safety included
        /// </summary>
        [JsonPropertyName("Description")]
        public string Description
        {
            get => HttpUtility.HtmlEncode(_description); //in-case was edited outside code
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
        /// Gets the extact time with offset at the place where this event happened
        /// Parses a event start time into DateTimeOffset
        /// uses google api
        /// NOTE:
        /// - updates the local instance with timezone data, to be saved and used later
        /// </summary>
        public async Task<DateTimeOffset> GetTime()
        {
            //get timezone from api and save it to local instance so that it can saved later
            //only use API if timezone data not yet set, to save unnecessary calls to Google
            this.Timezone = string.IsNullOrEmpty(this.Timezone)
                ? await Tools.GetTimezoneOffsetString(this.Location,
                    this.StartTime)
                : this.Timezone;

            //get start time of life event and find the position of it in slices (same as now line)
            //so that this life event line can be placed exactly on the report where it happened
            var lifeEvtTimeStr = $"{this.StartTime} {this.Timezone}"; //add offset 0 only for parsing, not used by API to get timezone
            var lifeEvtTime = DateTimeOffset.ParseExact(lifeEvtTimeStr, Time.DateTimeFormat, null);

            return lifeEvtTime;
        }

    }
}
