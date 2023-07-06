using System.Xml.Linq;
using Newtonsoft.Json.Linq;

namespace VedAstro.Library
{
    /// <summary>
    /// To implement type convertible to and from XML
    /// Used when sending types over internet
    /// </summary>
    public interface IToXml
    {
        /// <summary>
        /// Converts Type to XML version of it
        /// </summary>
        /// <returns></returns>
        XElement ToXml();

        /// <summary>
        /// Converts XML to its instance type
        /// </summary>
        dynamic FromXml<T>(XElement personXml) where T : IToXml;
    }

    /// <summary>
    /// convertible to JSON
    /// </summary>
    public interface IToJson
    {

        /// <summary>
        /// Converts Type to XML version of it
        /// </summary>
        /// <returns></returns>
        JObject ToJson();

        /// <summary>
        /// Converts XML to its instance type
        /// </summary>
    }
}
