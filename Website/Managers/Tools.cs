using System.Xml.Linq;

namespace Website
{
    /// <summary>
    /// Simple class holding general functions used in project
    /// </summary>
    public static class Tools
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


    }
}
