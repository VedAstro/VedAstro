using API;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APITests
{
    [TestClass()]
    public class CalculateTests
    {

        [TestMethod()]
        public async Task AllPlanetDataTest()
        {
            var json = JObject.Parse(await new HttpClient().GetStringAsync("https://vedicastrogpt.azurewebsites.net/api/Calculate/AllPlanetData/PlanetName/All/Location/Hetauda/Time/09:30/08/07/2023/+05:45"));

            Assert.IsTrue(json["Status"]?.Value<string>() == "Pass");
            Assert.IsTrue(json["Payload"]["AllPlanetData"].HasValues);
            Assert.IsTrue(json["Payload"]["AllPlanetData"][0]["Sun"].HasValues);
        }

    }
}
