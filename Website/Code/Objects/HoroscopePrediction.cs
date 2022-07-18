using System.Text.RegularExpressions;
using Genso.Astrology.Library;

namespace Website
{

    /// <summary>
    /// Simple class to encapsulate a HoroscopePrediction (data)
    /// </summary>
    public class HoroscopePrediction : IHasName
    {
        //FIELDS
        private readonly EventName _name;
        private readonly string _description;
        private readonly string _info;


        //CTOR
        public HoroscopePrediction(EventName name, string description, string info)
        {
            //initialize fields
            _name = name;
            _description = description;
            _info = info;
        }



        //PROPERTIES
        //Note: Created mainly for ease of use with WPF binding
        public EventName Name => _name;
        public string Description => _description;
        public string Info => _info;
        public string FormattedName => Format.FormatName(this);




        //PUBLIC METHODS
        public EventName GetName() => _name;


        //PRIVATE METHODS
        private static string SplitCamelCase(string str)
        {
            return Regex.Replace(
                Regex.Replace(
                    str,
                    @"(\P{Ll})(\P{Ll}\p{Ll})",
                    "$1 $2"
                ),
                @"(\p{Ll})(\P{Ll})",
                "$1 $2"
            );
        }


        //METHOD OVERRIDES
        public override bool Equals(object value)
        {

            if (value.GetType() == typeof(HoroscopePrediction))
            {
                //cast to type
                var parsedValue = (HoroscopePrediction)value;

                //check equality
                bool returnValue = (this.GetHashCode() == parsedValue.GetHashCode());

                return returnValue;
            }
            else
            {
                //Return false if value is null
                return false;
            }


        }

        public override int GetHashCode()
        {
            //get hash of all the fields & combine them
            var hash1 = _name.GetHashCode();
            var hash2 = Tools.GetStringHashCode(_description);

            return hash1 + hash2;
        }

        public override string ToString()
        {
            return $"{FormattedName} - {Description} - {Info}";
        }



        /// <summary>
        /// Searches all text in prediction for input
        /// </summary>
        public bool Contains(string searchText)
        {
            //place all text together
            var compiledText = $"{FormattedName} {Description} {Info}";

            //change all to small caps
            compiledText = compiledText.ToLower();

            //do the searching
            string pattern = @"\b" + Regex.Escape(searchText) + @"\b"; //searches only words
            var searchResult = Regex.Match(compiledText, pattern, RegexOptions.IgnoreCase).Success;
            return searchResult;

        }
    }
}