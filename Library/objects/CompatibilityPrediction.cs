using System;
using System.Xml.Linq;

namespace VedAstro.Library.Compatibility
{
    /// <summary>
    /// simple data type to contain info on a kuta prediction
    /// Note : properties can only be set once,
    /// so that doesn't accidentally get changed
    /// </summary>
    public class CompatibilityPrediction : IToXml
    {
        public static CompatibilityPrediction Empty = new CompatibilityPrediction(MatchPredictionName.Empty, EventNature.Empty, "Empty", "Empty","Empty", "Empty");

        //DATA FIELDS
        private string _info = "";
        private string _maleInfo = "";
        private string _femaleInfo = "";
        private string _description = "";

        public CompatibilityPrediction(MatchPredictionName name, EventNature nature, string maleInfo, string femaleInfo, string info, string description)
        {
            Name = name;
            Nature = nature;
            MaleInfo = maleInfo;
            FemaleInfo = femaleInfo;
            Info = info;
            Description = description;
        }

        public CompatibilityPrediction() { }


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

        public CompatibilityPrediction FromXml(XElement xml)
        {
            var name = Enum.Parse<MatchPredictionName>(xml.Element("Name")?.Value);
            var nature = Enum.Parse<EventNature>(xml.Element("Nature")?.Value);
            var maleInfo = xml.Element("MaleInfo")?.Value;
            var femaleInfo = xml.Element("FemaleInfo")?.Value;
            var info = xml.Element("Info")?.Value;
            var description = xml.Element("Description")?.Value;

            var parsed = new CompatibilityPrediction(name, nature, maleInfo, femaleInfo, info, description);

            return parsed;

        }

        public XElement ToXml()
        {
            //create root tag to hold data
            var predictionXml = new XElement(nameof(CompatibilityPrediction));
            var name = new XElement("Name", this.Name);
            var nature = new XElement("Nature", this.Nature);
            var maleInfo = new XElement("MaleInfo", this.MaleInfo);
            var femaleInfo = new XElement("FemaleInfo", this.FemaleInfo);
            var info = new XElement("Info", this.Info);
            var description = new XElement("Description", this.Description);

            predictionXml.Add(name, nature, maleInfo, femaleInfo, info, description);

            return predictionXml;
        }

    }
}
