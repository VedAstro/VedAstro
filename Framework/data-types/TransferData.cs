using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Xml.Linq;

namespace Genso.Framework
{
    //TODO method names need more clarity
    /// <summary>
    /// The base class for Request & Response data types
    /// </summary>
    public class TransferData
    {
        //data has the top level root element
        private XElement _data = new XElement("Root");
        private HttpStatusCode _status;


        /** PUBLIC METHODS **/

        /// <summary>
        /// When sending
        /// Adds data pieces into this data holder, each piece has a name & value
        ///  here original data NOT overwritten
        /// </summary>
        public void addData(string name, object value)
        {
            //format the raw data into a structured form
            var structured = new XElement(name, value);

            //add the inputted data piece into the main xml data
            _data.Add(structured);
        }

        /// <summary>
        /// When receiving
        /// Extracts the raw data coming from the server, and extract what is needed
        /// here original data is overwritten
        /// </summary>
        public void addData(HttpResponseMessage rawData)
        {
            //extract needed data from reply
            _status = rawData.StatusCode;

            //if the raw data is NOT ok, stop here
            if (_status != HttpStatusCode.OK) { return; }

            //extract the content of the reply data
            var rawMessage = rawData.Content.ReadAsStringAsync().Result;

            //and save it for later use
            _data = toXml(rawMessage);

        }

        /// <summary>
        /// Gets the value of a data element in side main data object (by its element name)
        /// TODO name of gate type can be passed in as var for improved separation of concerns
        /// </summary>
        public T getChildData<T>(string elementName)
        {
            //get elements with matching name
            var foundElements =
                from record in _data.Elements()
                where record.Name.ToString() == elementName
                select record;

            //if no element found, return null
            if (!foundElements.Any())
            {
                return default(T);
                //throw new Exception("Element doesn't exist!");
            }

            //if more than 1 element found throw error
            if (foundElements.Count() > 1) { throw new Exception("More than 1 element found!"); }

            //get value of the data
            var value = foundElements.FirstOrDefault().Value;

            //if trying to convert to Gate Name, then use this converting method
            if (typeof(T).Name == "Gate") { return (T)Enum.Parse(typeof(T), value); }

            //for other types use this converting method
            return (T)Convert.ChangeType(value, typeof(T));

        }

        /// <summary>
        /// When receiving
        /// Extracts the raw data coming from the server, and extract what is needed
        ///  here original data is overwritten
        /// </summary>
        public void addData(HttpRequestMessage rawData)
        {
            //get request body
            string stringBody = rawData.Content.ReadAsStringAsync().Result;

            //add to main xml data
            _data = toXml(stringBody);

            //extract needed data from reply
            //_status = rawRequest.StatusCode;

            //_message = parseMessage(rawRequest);

        }

        /// <summary>
        /// Packages the data into ready form for the HTTP client to use in final sending stage
        /// </summary>
        public StringContent toStringContent()
        {
            //gets the main XML data as a string
            var data = xmlToString(_data);

            //specify the data encoding
            var encoding = Encoding.UTF8;

            //specify the type of the data sent
            //plain text, stops auto formatting
            var mediaType = "plain/text";

            //return packaged data to caller
            return new StringContent(data, encoding, mediaType);
        }

        /// <summary>
        /// Returns the whole contents of the response as XML element
        /// </summary>
        public XElement getDataAsXml() => _data;

        /// <summary>
        /// Returns the whole contents of the response as XML element in string form
        /// </summary>
        public string getDataAsString() => xmlToString(_data);

        public HttpStatusCode getStatus() => _status;

        /// <summary>
        /// Converts/parses xml string into data instance
        /// Returns null if unable to convert
        /// </summary>
        public static TransferData fromXml(string xmlString)
        {
            //convert raw xml data into a recognized data (parse)
            var xElement = toXml(xmlString);

            //if could not parse, end here & return null
            if (xElement == null) { return null; }

            //place the data into an object 
            var instance = new TransferData { _data = xElement };

            //return the object to the caller
            return instance;
        }

        /// <summary>
        /// Converts xml element instance to string properly
        /// TODO needs to be a seperate class utils
        /// </summary>
        public static string xmlToString(XElement xml)
        {
            //remove all formatting, for clean xml as string
            return xml.ToString(SaveOptions.DisableFormatting);
        }



        /** PRIVATE METHODS **/

        /// <summary>
        /// Converts/parses string into XML instance, with cleaning
        /// if unable to parse returns null
        /// </summary>
        private static XElement toXml(string rawXml)
        {
            try
            {
                //clean string of extra characters, that cause parsing errors
                var cleaned = rawXml.Replace("\"", "");

                //parse into XML & return
                return XElement.Parse(cleaned);
            }
            //if any failure occurs when parsing, return null
            catch (Exception) { return null; }
        }



        /** ARCHIVED CODE **/


        ////converts current data instance into XML string
        //public string toXml()
        //{
        //    
        //    return _data.ToString();

        //}
    }


}