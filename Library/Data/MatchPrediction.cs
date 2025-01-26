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
    public class MatchPrediction : IToJson
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

        JObject IToJson.ToJson() => (JObject)this.ToJson();

        public JToken ToJson()
        {
            var temp = new JObject();
            temp["Name"] = this.Name.ToDisplayString(); //note to show human friendly name for easy print to HTML
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
