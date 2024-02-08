using Microsoft.VisualStudio.TestTools.UnitTesting;
using API;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API.Tests
{
    [TestClass()]
    public class GeoLocationAPITests
    {
        [TestMethod()]
        public async Task AddressToGeoLocation_GoogleTest()
        {

            var xxx = await GeoLocationAPI.AddressToGeoLocation_Google("Tupelo, Mississippi");

            Assert.Fail();
        }
    }
}