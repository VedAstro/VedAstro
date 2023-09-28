using System;
using System.Collections.Generic;
using System.Xml.Linq;
using Newtonsoft.Json.Linq;

namespace VedAstro.Library
{
    /// <summary>
    /// simple data type to contain info on a kuta prediction
    /// Note : properties can only be set once,
    /// so that doesn't accidentally get changed
    /// </summary>
    public class MatchPrediction : IToXml
    {
        public static MatchPrediction Empty = new MatchPrediction(MatchPredictionName.Empty, EventNature.Empty, "Empty", "Empty","Empty", "Empty");

        //DATA FIELDS
        private string _info = "";
        private string _maleInfo = "";
        private string _femaleInfo = "";
        private string _description = "";

        public MatchPrediction(MatchPredictionName name, EventNature nature, string maleInfo, string femaleInfo, string info, string description)
        {
            Name = name;
            Nature = nature;
            MaleInfo = maleInfo;
            FemaleInfo = femaleInfo;
            Info = info;
            Description = description;
        }

        public MatchPrediction() { }


        //PUBLIC PROPERTIES
        public MatchPredictionName Name { get; set; }

        public string Description
        {
            get => _description;
            set
            {
                //if value already set, raise alarm
                if (_description != "") throw new InvalidOperationException("Only set once!");
                _description = value;
            }
        }

        public EventNature Nature { get; set; }

        public string Info
        {
            get => _info;
            set
            {
                //if value already set, raise alarm
                if (_info != "") throw new InvalidOperationException("Only set once!");
                _info = value;
            }
        }

        public string MaleInfo
        {
            get => _maleInfo;
            set
            {
                //if value already set, raise alarm
                if (_maleInfo != "") throw new InvalidOperationException("Only set once!");
                _maleInfo = value;
            }
        }

        public string FemaleInfo
        {
            get => _femaleInfo;
            set
            {
                //if value already set, raise alarm
                if (_femaleInfo != "") throw new InvalidOperationException("Only set once!");
                _femaleInfo = value;
            }
        }

        public string FormattedName => Format.FormatName(this.Name);


        //PUBLIC METHODS

        /// <summary>
        /// The root element is expected to be Person
        /// Note: Special method done to implement IToXml
        /// </summary>
        public dynamic FromXml<T>(XElement xml) where T : IToXml => FromXml(xml);

        public MatchPrediction FromXml(XElement xml)
        {
            var name = Enum.Parse<MatchPredictionName>(xml.Element("Name")?.Value);
            var nature = Enum.Parse<EventNature>(xml.Element("Nature")?.Value);
            var maleInfo = xml.Element("MaleInfo")?.Value;
            var femaleInfo = xml.Element("FemaleInfo")?.Value;
            var info = xml.Element("Info")?.Value;
            var description = xml.Element("Description")?.Value;

            var parsed = new MatchPrediction(name, nature, maleInfo, femaleInfo, info, description);

            return parsed;

        }

        public XElement ToXml()
        {
            //create root tag to hold data
            var predictionXml = new XElement(nameof(MatchPrediction));
            var name = new XElement("Name", this.Name);
            var nature = new XElement("Nature", this.Nature);
            var maleInfo = new XElement("MaleInfo", this.MaleInfo);
            var femaleInfo = new XElement("FemaleInfo", this.FemaleInfo);
            var info = new XElement("Info", this.Info);
            var description = new XElement("Description", this.Description);

            predictionXml.Add(name, nature, maleInfo, femaleInfo, info, description);

            return predictionXml;
        }

        public JToken ToJson()
        {
            var temp = new JObject();
            temp["Name"] = this.Name.ToString();
            temp["Nature"] = this.Nature.ToString();
            temp["MaleInfo"] = this.MaleInfo;
            temp["FemaleInfo"] = this.FemaleInfo;
            temp["Info"] = this.Info;
            temp["Description"] = this.Description;

            return temp;
        }


        public static JArray ToJsonList(List<MatchPrediction> predictionList)
        {
            var jsonList = new JArray();

            foreach (var matchPrediction in predictionList)
            {
                jsonList.Add(matchPrediction.ToJson());
            }

            return jsonList;
        }
    }
}
