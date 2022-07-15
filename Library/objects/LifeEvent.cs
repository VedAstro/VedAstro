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
            get => HttpUtility.HtmlEncode(_name);
            set => _name = value;
        }

        /// <summary>
        /// Description of life event
        /// HTML safety included
        /// </summary>
        [JsonPropertyName("Description")]
        public string Description
        {
            get => HttpUtility.HtmlEncode(_description);
            set => _description = value;
        }

        /// <summary>
        /// Must follow standard time formatting 
        /// </summary>
        [JsonPropertyName("StartTime")]
        public string StartTime { get; set; }

        /// <summary>
        /// must be Good, Neutral or Bad only
        /// </summary>
        [JsonPropertyName("Nature")]
        public string Nature { get; set; }





        //░█▀▄▀█ ░█▀▀▀ ▀▀█▀▀ ░█─░█ ░█▀▀▀█ ░█▀▀▄ ░█▀▀▀█ 
        //░█░█░█ ░█▀▀▀ ─░█── ░█▀▀█ ░█──░█ ░█─░█ ─▀▀▀▄▄ 
        //░█──░█ ░█▄▄▄ ─░█── ░█─░█ ░█▄▄▄█ ░█▄▄▀ ░█▄▄▄█


        public XElement ToXml()
        {
            var lifeEventXml = new XElement("LifeEvent");
            var nameXml = new XElement("Name", this.Name);
            var startTimeXml = new XElement("StartTime", this.StartTime);
            var descriptionXml = new XElement("Description", this.Description);
            var natureXml = new XElement("Nature", this.Nature);

            lifeEventXml.Add(nameXml, startTimeXml, descriptionXml, natureXml);

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
            lifeEventParsed.Name = !string.IsNullOrEmpty(lifeEventXml.Element("Name")?.Value) ? lifeEventXml.Element("Name").Value : "";
            lifeEventParsed.StartTime = !string.IsNullOrEmpty(lifeEventXml.Element("StartTime")?.Value) ? lifeEventXml.Element("StartTime").Value : "";
            lifeEventParsed.Description = !string.IsNullOrEmpty(lifeEventXml.Element("Description")?.Value) ? lifeEventXml.Element("Description").Value : "";
            lifeEventParsed.Nature = !string.IsNullOrEmpty(lifeEventXml.Element("Nature")?.Value) ? lifeEventXml.Element("Nature").Value : "";


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
            var returnString = $"{this.Name} - {this.Nature} - {this.StartTime} - {this.Description}";

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
            var hash3 = Tools.GetStringHashCode(this.Description);
            var hash4 = Tools.GetStringHashCode(this.Nature);

            //take out negative before returning
            return Math.Abs(hash1 + hash2 + hash3 + hash4);
        }



    }
}
