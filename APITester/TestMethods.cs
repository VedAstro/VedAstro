using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json.Linq;

namespace APITester
{
    public class TestMethods
    {
        private string LocalAPIServer { get; set; }

        //save data here to be used during all test
        public TestMethods(string localApiServer)
        {
            LocalAPIServer = localApiServer;
        }



        /// <summary>
        /// Simple test for over calls
        /// .../CoordinatesToGeoLocation/Location/SydneyNSW,Australia/Time/14:02/09/11/1977/+00:00
        /// </summary>
        public async Task<bool> AbuseFirewallTest()
        {
            var url =
                $"{LocalAPIServer}Calculate/AllPlanetData/PlanetName/All/Location/Hetauda/Time/09:30/08/07/2023/+05:45";
            var json = JObject.Parse(await new HttpClient().GetStringAsync(url));

            Console.WriteLine(json); //print everything

            //check some key values
            Assert.IsTrue(json["Status"]?.Value<string>() == "Pass");
            Assert.IsTrue(json["Payload"]["AllPlanetData"].HasValues);
            Assert.IsTrue(json["Payload"]["AllPlanetData"][0]["Sun"].HasValues);

            //control comes here once all pass
            return true; //todo default pass

        }


        #region GEO_LOCATION

        /// <summary>
        /// .../AddressToGeoLocation/Gaithersburg, MD, USA
        /// </summary>
        public async Task<dynamic> AddressToGeoLocationTest()
        {
            //.../Calculate/AddressToGeoLocation/London
            var url = $"{LocalAPIServer}Calculate/AddressToGeoLocation/Address/London";
            var json = JObject.Parse(await new HttpClient().GetStringAsync(url));

            //send data back to caller
            return new { URL = url, OUTPUT = json };
        }

        /// <summary>
        /// .../CoordinatesToGeoLocation/Location/SydneyNSW,Australia/Time/14:02/09/11/1977/+00:00
        /// </summary>
        public async Task<dynamic> CoordinatesToGeoLocationTest()
        {
            //.../Calculate/AddressToGeoLocation/London
            var url = $"{LocalAPIServer}Calculate/CoordinatesToGeoLocation/Latitude/35.6764/Longitude/139.6500";
            var json = JObject.Parse(await new HttpClient().GetStringAsync(url));

            //send data back to caller
            return new { URL = url, OUTPUT = json };

        }

        /// <summary>
        /// .../IpAddressToGeoLocation/
        /// </summary>
        public async Task<dynamic> IpAddressToGeoLocationTest()
        {
            var url = $"{LocalAPIServer}Calculate/IpAddressToGeoLocation/IpAddress/180.75.241.81";
            var json = JObject.Parse(await new HttpClient().GetStringAsync(url));

            //send data back to caller
            return new { URL = url, OUTPUT = json };
        }

        /// <summary>
        /// .../Calculate/GeoLocationToTimezone/Location/Tokyo, Japan/Coordinates/35.65,139.83/Time/14:02/09/11/1977/+00:00
        /// </summary>
        public async Task<dynamic> GeoLocationToTimezoneTest()
        {
            //That’s the last time we changed timezones, but not the first. We’ve actually changed zone 6 times before that in our history. 
            // 
            // Prior to 1901, GMT+6.46
            // After 1901, GMT+6.55 (Set to Singapore Mean Time by Colonial Powers) 
            // After 1905, GMT+7.00  (Rounded off the time to the Standard Zone Time) 
            // After 1933, GMT+7.20 (Adjusted for Daylight Savings) 
            // After 1941, GMT+7.30 (Further Adjusted for Daylight Savings) 
            // After 1942, GMT+9.00 (Adjusted to Tokyo Standard Time during Japanese Occupation) 
            // After 1945, GMT+7.30 (Return to Daylight Savings Time) 
            // After 1982, GMT+8.00 (Malaysia Standard Time) 

            var url = $"{LocalAPIServer}Calculate/GeoLocationToTimezone/Location/Tokyo, Japan/Coordinates/35.65,139.83/Time/14:02/09/11/1977/+00:00";
            var json = JObject.Parse(await new HttpClient().GetStringAsync(url));


            //send data back to caller
            return new {URL = url, OUTPUT=json};
        }

        #endregion

        /// <summary>
        /// .../Calculate/AllPlanetData/PlanetName/All/Location/Hetauda/Time/09:30/08/07/2023/+05:45
        /// </summary>
        public async Task<dynamic> AllPlanetDataTest()
        {
            //var json = JObject.Parse(await new HttpClient().GetStringAsync($"{LocalAPIServer}Calculate/AllPlanetData/PlanetName/All/Location/Hetauda/Time/09:30/08/07/2023/+05:45"));

            var url = $"{LocalAPIServer}Calculate/AllPlanetData/PlanetName/All/Location/Hetauda/Time/09:30/08/07/2023/+05:45";
            var json = JObject.Parse(await new HttpClient().GetStringAsync(url));

            //send data back to caller
            return new { URL = url, OUTPUT = json };
        }

        /// <summary>
        /// .../Calculate/AllHouseData/HouseName/All/Location/Hetauda/Time/09:30/08/07/2023/+05:45
        /// </summary>
        public async Task<dynamic> AllHouseData()
        {
            var url = $"{LocalAPIServer}Calculate/AllHouseData/HouseName/All/Location/Hetauda/Time/09:30/08/07/2023/+05:45";
            var json = JObject.Parse(await new HttpClient().GetStringAsync(url));

            //send data back to caller
            return new { URL = url, OUTPUT = json };
        }

        /// <summary>
        /// .../Calculate/DasaAtRange/Location/Hetauda/Time/09:3[…]/+05:45/Location/Hetauda/Time/09:30/08/07/2093/+05:45
        /// </summary>
        public async Task<dynamic> DasaAtRangeTest()
        {

            var url = $"{LocalAPIServer}Calculate/DasaAtRange/Location/Punjab, India/Time/22:10/02/08/1995/+05:30/Location/Punjab, India/Time/22:10/02/08/1995/+05:30/Location/Punjab, India/Time/22:10/02/08/2065/+05:30";
            var json = JObject.Parse(await new HttpClient().GetStringAsync(url));

            //send data back to caller
            return new { URL = url, OUTPUT = json };


        }

        /// <summary>
        /// .../Calculate/BhinnashtakavargaChart/Location/Hetauda/Time/09:30/08/07/2023/+05:45
        /// </summary>
        public async Task BhinnashtakavargaChartTest()
        {
            var json = JObject.Parse(await new HttpClient().GetStringAsync($"{LocalAPIServer}Calculate/AllPlanetData/PlanetName/All/Location/Hetauda/Time/09:30/08/07/2023/+05:45"));

            Assert.IsTrue(json["Status"]?.Value<string>() == "Pass");
            Assert.IsTrue(json["Payload"]["AllPlanetData"].HasValues);
            Assert.IsTrue(json["Payload"]["AllPlanetData"][0]["Sun"].HasValues);
        }



    }
}
