using System.Text.RegularExpressions;
using Genso.Astrology.Library;

namespace Genso.Astrology.Library
{
    /// <summary>
    /// Class to handle formatting
    /// </summary>
    public static class Format
    {
        //PUBLIC METHODS
        /// <summary>
        /// Gets human readable event name
        /// </summary>
        public static string FormatName(IHasName obj)
        {
            //get enum name of object
            var name = obj.GetName();

            //convert enum name to string
            var nameWithoutSpaces = name.ToString();

            //add spaces in between camel case letters
            var nameWithSpace = SplitCamelCase(nameWithoutSpaces);

            return nameWithSpace;
        }

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

    }
}