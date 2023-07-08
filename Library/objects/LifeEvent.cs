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
        public static LifeEvent Empty = new LifeEvent()
        {
            Name = "New Life Event",
            Description = "Event Description",
            StartTime = Time.Empty,
            Id = Tools.GenerateId(),
            Nature = "Neutral",
            Weight = "Normal"
        };
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

        [JsonPropertyName("StartTime")]
        public Time StartTime { get; set; }

        /// <summary>
        /// must be Good, Neutral or Bad only
        /// </summary>
        [JsonPropertyName("Nature")]
        public string Nature { get; set; }

        /// <summary>
        /// must be Major, Normal and Minor
        /// </summary>
        [JsonPropertyName("Weight")]
        public string Weight { get; set; }

        /// <summary>
        /// unique ID stamped at creation
        /// </summary>
        [JsonPropertyName("Id")]
        public string Id { get; set; }

        /// <summary>
        /// combined all text in life event used for search
        /// </summary>
        public string SearchText
        {
            get
            {
                var compiledText = "";

                //add in all stuff
                compiledText += Name;
                compiledText += Description;
                compiledText += StartTime;
                compiledText += Nature;
                compiledText += Weight;

                return compiledText;
            }
        }



        //░█▀▄▀█ ░█▀▀▀ ▀▀█▀▀ ░█─░█ ░█▀▀▀█ ░█▀▀▄ ░█▀▀▀█ 
        //░█░█░█ ░█▀▀▀ ─░█── ░█▀▀█ ░█──░█ ░█─░█ ─▀▀▀▄▄ 
        //░█──░█ ░█▄▄▄ ─░█── ░█─░█ ░█▄▄▄█ ░█▄▄▀ ░█▄▄▄█


        public JObject ToJson()
        {
            var temp = new JObject();
            temp["Id"] = this.Id;
            temp["Name"] = this.Name;
            temp["StartTime"] = this.StartTime.ToJson();
            temp["Description"] = this.Description;
            temp["Nature"] = this.Nature;
            temp["Weight"] = this.Weight;

            //compile into an JSON array
            return temp;

        }


        public XElement ToXml()
        {
            var lifeEventXml = new XElement("LifeEvent");
            var idXml = new XElement("Id", this.Id);
            var nameXml = new XElement("Name", this.Name);
            var startTimeXml = new XElement("StartTime", this.StartTime.ToXml());
            var descriptionXml = new XElement("Description", this.Description);
            var natureXml = new XElement("Nature", this.Nature);
            var weightXml = new XElement("Weight", this?.Weight ?? "Normal");

            lifeEventXml.Add(idXml, nameXml, startTimeXml, descriptionXml, natureXml, weightXml);

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
            lifeEventParsed.Id = !string.IsNullOrEmpty(lifeEventXml.Element("Id")?.Value) ? lifeEventXml?.Element("Id")?.Value : ""; //leave empty to detect
            lifeEventParsed.Name = !string.IsNullOrEmpty(lifeEventXml.Element("Name")?.Value) ? lifeEventXml?.Element("Name")?.Value : "";
            lifeEventParsed.Description = !string.IsNullOrEmpty(lifeEventXml.Element("Description")?.Value) ? lifeEventXml?.Element("Description")?.Value : "";
            lifeEventParsed.Nature = !string.IsNullOrEmpty(lifeEventXml.Element("Nature")?.Value) ? lifeEventXml?.Element("Nature")?.Value : "";
            lifeEventParsed.Weight = !string.IsNullOrEmpty(lifeEventXml.Element("Weight")?.Value) ? lifeEventXml?.Element("Weight")?.Value : "Normal";
            lifeEventParsed.StartTime = Time.FromXml(lifeEventXml.Element("StartTime")?.Element("Time"));

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

                temp.Id = lifeEvent["Id"].Value<string>();
                temp.Name = lifeEvent["Name"].Value<string>();
                temp.StartTime = Time.FromJson(lifeEvent["StartTime"]); ;
                temp.Description = lifeEvent["Description"].Value<string>();
                temp.Nature = lifeEvent["Nature"].Value<string>();
                temp.Weight = lifeEvent["Weight"]?.Value<string>() ?? "Normal";

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
            var returnString = $"{this.Name} - {this.Nature} - {this.Weight} - {this.StartTime} - {this.Description}";

            //return string to caller
            return returnString;
        }

        /// <summary>
        /// Name & Birth Time are used to generate Hash
        /// </summary>
        public override int GetHashCode()
        {
            //get hash of all the fields & combine them
            var hash0 = Tools.GetStringHashCode(this.Id);
            var hash1 = Tools.GetStringHashCode(this.Name);
            var hash2 = this.StartTime.GetHashCode();
            var hash5 = Tools.GetStringHashCode(this.Description);
            var hash6 = Tools.GetStringHashCode(this.Nature);
            var hash7 = Tools.GetStringHashCode(this.Weight);

            //take out negative before returning
            return Math.Abs(hash0 + hash1 + hash2 + hash5 + hash6 + hash7);
        }


        /// <summary>
        /// compare logic to sort according to time
        /// greater = -1
        /// equal = 0
        /// lesser = 1
        /// </summary>
        public int CompareTo(LifeEvent lifeEvent)
        {
            var inputTime = lifeEvent.StartTime.GetStdDateTimeOffset();

            var currentTime = this.StartTime.GetStdDateTimeOffset();

            //compare with time
            return currentTime.CompareTo(inputTime);
        }


    }
}
