using System.Text.Json.Nodes;
using System.Xml;
using System.Xml.Linq;
using Genso.Astrology.Library;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using Newtonsoft.Json;

namespace Website
{
    public delegate Task AsyncEventHandler();
    /// <summary>
    /// Simple class holding general functions used in project
    /// </summary>
    public static class WebsiteTools
    {
        public static async Task<dynamic> GetAddressLocation(string address)
        {
            //create the request url for Google API
            var url = $"https://maps.googleapis.com/maps/api/geocode/xml?key={ServerManager.GoogleGeoLocationAPIKey}&address={Uri.EscapeDataString(address)}&sensor=false";

            //get location data from GoogleAPI
            var rawReplyXml = await ServerManager.ReadFromServer(url);

            //extract out the longitude & latitude
            var locationData = new XDocument(rawReplyXml);
            var result = locationData.Element("GeocodeResponse").Element("result");
            var locationElement = result.Element("geometry").Element("location");
            var lat = Double.Parse(locationElement.Element("lat").Value);
            var lng = Double.Parse(locationElement.Element("lng").Value);

            //round coordinates to 3 decimal places
            lat = Math.Round(lat, 3);
            lng = Math.Round(lng, 3);

            //get full name with country & state
            var fullName = result.Element("formatted_address").Value;

            return new { FullName = fullName, Latitude = lat, Longitude = lng };

        }


        /// <summary>
        /// gets the name of the place given th coordinates, uses Google API
        /// </summary>
        public static async Task<string> CoordinateToAddress(decimal longitude, decimal latitude)
        {
            //create the request url for Google API
            var url = string.Format($"https://maps.googleapis.com/maps/api/geocode/xml?latlng={latitude},{longitude}&key={ServerManager.GoogleGeoLocationAPIKey}");


            //get location data from GoogleAPI
            var rawReplyXml = await ServerManager.ReadFromServer(url);

            //extract out the longitude & latitude
            var locationData = new XDocument(rawReplyXml);
            var localityResult = locationData.Element("GeocodeResponse")?.Elements("result").FirstOrDefault(result => result.Element("type")?.Value == "locality");
            var locationName = localityResult?.Element("formatted_address")?.Value;


            return locationName;

        }

        /// <summary>
        /// Tries to ID the user, and sends a log of the visit to API server
        /// Called from MainLayout everytime page is loaded
        /// </summary>
        public static async Task LogVisitor(IJSRuntime _jsRuntime)
        {
            //get url user is on
            var urlXml = new XElement("Url", await _jsRuntime.InvokeAsync<string>("getUrl"));

            //find out if new visitor just arriving or old one browsing
            var uniqueId = await GetVisitorIdFromCookie();
            var isNewVisitor = uniqueId == null;

            //based on visitor write the log
            //this is done to minimize excessive logging
            if (isNewVisitor) { await NewVisitor(); }
            else { await OldVisitor(); }


            //-------------- FUNCTIONS -------------------------

            //all possible details are logged
            async Task NewVisitor()
            {

                //get visitor data & format it nicely for storage
                var browserDataXml = await GetBrowserDataXml();
                var timeStampXml = new XElement("TimeStamp", Tools.GetNowSystemTimeText());
                var visitorId = Tools.GenerateId();
                var uniqueIdXml = new XElement("UniqueId", visitorId);
                var locationXml = await ServerManager.ReadFromServer(ServerManager.GetGeoLocation, "Location");
                var visitorElement = new XElement("Visitor");
                visitorElement.Add(uniqueIdXml, urlXml, timeStampXml, locationXml, browserDataXml);

                //send to API for save keeping
                var result = await ServerManager.WriteToServer(ServerManager.AddVisitorAPI, visitorElement);

                //mark visitor with id inside cookie
                await SetNewVisitorIdInCookie(visitorId);

                //todo do something with result
                Console.WriteLine(result);
            }

            //only needed details are logged
            async Task OldVisitor()
            {

                //get visitor data & format it nicely for storage
                var visitorElement = new XElement("Visitor");
                var timeStampXml = new XElement("TimeStamp", Tools.GetNowSystemTimeText());
                var uniqueIdXml = new XElement("UniqueId", uniqueId); //use id generated above
                visitorElement.Add(uniqueIdXml, urlXml, timeStampXml);

                //send to API for save keeping
                var result = await ServerManager.WriteToServer(ServerManager.AddVisitorAPI, visitorElement);

                //todo do something with result
                Console.WriteLine(result);
            }

            //returns null if no id found
            async Task<string> GetVisitorIdFromCookie() => await _jsRuntime.InvokeAsync<string>("getCookiesWrapper", "uniqueId");

            async Task SetNewVisitorIdInCookie(string id) => await _jsRuntime.InvokeVoidAsync("setCookiesWrapper", "uniqueId", id);

            //calls js library to get browser data, converts it to xml
            async Task<XElement> GetBrowserDataXml()
            {
                var dataJson = await _jsRuntime.InvokeAsync<JsonNode>("getVisitorData");
                var rawXml = JsonConvert.DeserializeXmlNode(dataJson.ToString(), "BrowserData");
                return XElement.Parse(rawXml.InnerXml);
            }

        }


        /// <summary>
        /// Event fired just after user has signed in
        /// </summary>
        public static event AsyncEventHandler OnUserSignIn;

        /// <summary>
        /// Event fired just after user has signed out
        /// </summary>
        public static event AsyncEventHandler OnUserSignOut;

        /// <summary>
        /// This method is called from JS when user signs in
        /// </summary>
        [JSInvokable]
        public static void InvokeOnUserSignIn() => OnUserSignIn?.Invoke();

        /// <summary>
        /// This method is called from JS when user signs out
        /// </summary>
        [JSInvokable]
        public static void InvokeOnUserSignOut() => OnUserSignOut?.Invoke();
    }
}
