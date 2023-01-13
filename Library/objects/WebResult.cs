using System;
using System.Collections.Generic;
using System.Linq;
using System.Resources;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Genso.Astrology.Library
{

    /// <summary>
    /// Simple type to encapsulate results coming from HTTP/API calls
    /// This is high level data, not needed but for better code quality
    /// </summary>
    public class WebResult<T>
    {

        /// <summary>
        /// If true result is PASS, if false is FAIL
        /// </summary>
        public bool Result { get; set; }

        public T Payload { get; set; }


        public WebResult() { }

        public WebResult(bool result, T payload)
        {
            Result = result;
            Payload = payload;
        }

        public static implicit operator T(WebResult<T> value)
        {
            return value.Payload;
        }

        public static implicit operator WebResult<T>(T value)
        {
            return new WebResult<T> { Payload = value };
        }

        /// <summary>
        /// Checks if result xml sent from api to client has status pass
        /// else will return false, will also false if any error in result
        /// </summary>
        public static bool IsResultPass(XElement result)
        {
            try
            {
                return result.Element("Status")?.Value == "Pass";
            }
            catch (Exception) { return false; }
        }

        public static WebResult<XElement> FromXml(XElement inputXml)
        {
            var result = new WebResult<XElement>();

            //get data out of the xml
            result.Result = inputXml.Element("Status")?.Value == "Pass";
            result.Payload = inputXml.Element("Payload")?.Elements().FirstOrDefault();

            return result;
        }

    }
}
