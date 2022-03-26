using System;
using System.Xml.Linq;

namespace Genso.Astrology.Library.Compatibility
{
    /// <summary>
    /// simple data type to contain info on a kuta prediction
    /// Note : properties can only be set once,
    /// so that doesn't accidentally get changed
    /// </summary>
    public class CompatibilityPrediction
    {
        //DATA FIELDS
        private string _info = "";
        private string _maleInfo = "";
        private string _femaleInfo = "";
        private string _description = "";


        //PUBLIC PROPERTIES
        public Name Name { get; set; }

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


        //PRIVATE METHODS
        public XElement ToXML()
        {
            //create root tag to hold data
            var predictionXml = new XElement("Prediction");
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
