using System.Xml.Linq;
using Newtonsoft.Json.Linq;

namespace VedAstro.Library
{
    /// <summary>
    /// convertible to JSON
    /// </summary>
    public interface IToJpeg
    {

        /// <summary>
        /// Converts Type to XML version of it
        /// </summary>
        /// <returns></returns>
        byte[] ToJpeg();

        
    }
}
