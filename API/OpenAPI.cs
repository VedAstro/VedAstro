using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs;
using System.Net.Http;
using System.Threading.Tasks;
using Genso.Astrology.Library;

namespace API
{
    internal class OpenAPI
    {
        /// <summary>
        /// https://www.vedastro.org/api/Location/Singapore/Time/23:59/31/12/2000/+08:00/Sun/Sign/
        /// </summary>
        [FunctionName("Location")]
        public static async Task<IActionResult> Main(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "Location/{locationName}/Time/{hhmmStr}/{dateStr}/{monthStr}/{yearStr}/{offsetStr}/Planet/{planetNameStr}/{propertyName}")]
            HttpRequestMessage incomingRequest, string locationName, string hhmmStr, string dateStr, string monthStr, string yearStr, string offsetStr, string planetNameStr, string propertyName)
        {
            PlanetName planetName;
            var planetNameResult = PlanetName.TryParse(planetNameStr, out planetName);
            var geoLocationResult = await Tools.AddressToGeoLocation(locationName);
            var geoLocation = geoLocationResult.Payload;

            //check result 1st before parsing
            if (planetNameResult! || geoLocationResult.IsPass!) { return new ContentResult { Content = "Please check your input, it failed to parse.", ContentType = "text/html" }; }

            //clean time text
            var timeStr = $"{hhmmStr} {dateStr}/{monthStr}/{yearStr} {offsetStr}";
            var parsedTime = new Time(timeStr, geoLocation);

            var zodicaSign = AstronomicalCalculator.GetPlanetRasiSign(planetName, parsedTime);

            return new ContentResult { Content = zodicaSign.ToString(), ContentType = "text/html" };
        }
    }
}
