using System;
using System.Collections.Generic;
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
        public static LifeEvent Empty = new LifeEvent("Empty", "Empty", Time.Empty, "New Life Event",
            "Event Description", "Neutral", "Normal");

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
        public string Name
        {
            get => HttpUtility.HtmlDecode(_name);
            set => _name = HttpUtility.HtmlEncode(value);
        }

        /// <summary>
        /// Description of life event
        /// HTML safety included
        /// </summary>
        public string Description
        {
            get => HttpUtility.HtmlDecode(_description);
            set => _description = HttpUtility.HtmlEncode(value);
        }

        public Time StartTime { get; set; }

        /// <summary>
        /// must be Good, Neutral or Bad only
        /// </summary>
        public string Nature { get; set; }

        /// <summary>
        /// must be Major, Normal and Minor
        /// </summary>
        public string Weight { get; set; }

        /// <summary>
        /// unique ID stamped at creation
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Id of person it is connected to
        /// </summary>
        public string PersonId { get; set; }

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


        //CTOR
        public LifeEvent(string personId, string uniqueId, Time startTime, string name, string description, string nature, string weight)
        {
            PersonId = personId;
            Id = uniqueId;
            Name = name;
            StartTime = startTime;
            Description = description;
            Nature = nature;
            Weight = weight;
        }


        public JObject ToJson()
        {
            var temp = new JObject();
            temp["PersonId"] = this.PersonId;
            temp["Id"] = this.Id;
            temp["Name"] = this.Name;
            temp["StartTime"] = this.StartTime.ToJson();
            temp["Description"] = this.Description;
            temp["Nature"] = this.Nature;
            temp["Weight"] = this.Weight;

            //compile into an JSON array
            return temp;

        }

        public LifeEventRow ToAzureRow()
        {
            //make the cache row to be added
            var newRow = new LifeEventRow()
            {
                //can have many IP as partition key
                PartitionKey = this.PersonId, //person linked to
                RowKey = Id, //time of creation
                StartTime = this.StartTime.ToJson().ToString(),
                Name = this.Name,
                Description = this.Description,
                Nature = this.Nature,
                Weight = this.Weight,
            };

            return newRow;
        }

        public static LifeEvent FromAzureRow(LifeEventRow rowData)
        {
            var startTime = Time.FromJson(JToken.Parse(rowData.StartTime));
            var newPerson = new LifeEvent(
                personId: rowData.PartitionKey,
                uniqueId: rowData.RowKey,
                startTime: startTime,
                name: rowData.Name,
                description: rowData.Description,
                nature: rowData.Nature,
                weight: rowData.Weight);

            return newPerson;
        }

        public XElement ToXml()
        {
            var lifeEventXml = new XElement("LifeEvent");
            var idXml = new XElement("Id", this.Id);
            var personIdXml = new XElement("PersonId", this.PersonId);
            var nameXml = new XElement("Name", this.Name);
            var startTimeXml = new XElement("StartTime", this.StartTime.ToXml());
            var descriptionXml = new XElement("Description", this.Description);
            var natureXml = new XElement("Nature", this.Nature);
            var weightXml = new XElement("Weight", this?.Weight ?? "Normal");

            lifeEventXml.Add(personIdXml, idXml, nameXml, startTimeXml, descriptionXml, natureXml, weightXml);

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

            //try get data from xml else use empty string
            var id = !string.IsNullOrEmpty(lifeEventXml.Element("Id")?.Value) ? lifeEventXml?.Element("Id")?.Value : ""; //leave empty to detect
            var personId = !string.IsNullOrEmpty(lifeEventXml.Element("PersonId")?.Value) ? lifeEventXml?.Element("PersonIdId")?.Value : ""; //leave empty to detect
            var name = !string.IsNullOrEmpty(lifeEventXml.Element("Name")?.Value) ? lifeEventXml?.Element("Name")?.Value : "";
            var description = !string.IsNullOrEmpty(lifeEventXml.Element("Description")?.Value) ? lifeEventXml?.Element("Description")?.Value : "";
            var nature = !string.IsNullOrEmpty(lifeEventXml.Element("Nature")?.Value) ? lifeEventXml?.Element("Nature")?.Value : "";
            var weight = !string.IsNullOrEmpty(lifeEventXml.Element("Weight")?.Value) ? lifeEventXml?.Element("Weight")?.Value : "Normal";
            var startTime = Time.FromXml(lifeEventXml.Element("StartTime")?.Element("Time"));

            var lifeEventParsed = new LifeEvent(personId, id, startTime, name, description, nature, weight);

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
                returnList.Add(LifeEvent.FromJson(lifeEvent));
            }

            return returnList;
        }

        public static LifeEvent FromJson(JToken lifeEvent)
        {
            var personId = lifeEvent["PersonId"].Value<string>();
            var id = lifeEvent["Id"].Value<string>();
            var name = lifeEvent["Name"].Value<string>();
            var startTime = Time.FromJson(lifeEvent["StartTime"]); ;
            var description = lifeEvent["Description"].Value<string>();
            var nature = lifeEvent["Nature"].Value<string>();
            var weight = lifeEvent["Weight"]?.Value<string>() ?? "Normal";

            var parsed = new LifeEvent(personId, id, startTime, name, description, nature, weight);

            return parsed;
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
