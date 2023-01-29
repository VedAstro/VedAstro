using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs;
using System.Net.Http;
using System.Threading.Tasks;
using Genso.Astrology.Library;

namespace API
{
    public static class OpenAPI
    {
        /// <summary>
        /// https://www.vedastro.org/api/Location/Singapore/Time/23:59/31/12/2000/+08:00/Planet/Sun/Sign/
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
            if (!planetNameResult || !geoLocationResult.IsPass)
            {
                return new ContentResult
                { Content = "Please check your input, it failed to parse.", ContentType = "text/html" };
            }

            //clean time text
            var timeStr = $"{hhmmStr} {dateStr}/{monthStr}/{yearStr} {offsetStr}";
            var parsedTime = new Time(timeStr, geoLocation);

            //based on property call the method
            var returnVal = "";
            switch (propertyName)
            {
                case "Sign": returnVal = AstronomicalCalculator.GetPlanetRasiSign(planetName, parsedTime).ToString(); break;
                case "Navamsa": returnVal = AstronomicalCalculator.GetPlanetNavamsaSign(planetName, parsedTime).ToString(); break;
                case "Constellation": returnVal = AstronomicalCalculator.GetPlanetConstellation(parsedTime, planetName).ToString(); break;
                case "Declination": returnVal = AstronomicalCalculator.GetPlanetDeclination(planetName, parsedTime).ToString(); break;
                case "AspectingPlanets": returnVal = AstronomicalCalculator.GetPlanetsAspectingPlanet(parsedTime, planetName).ToString(); break;
                case "Motion": returnVal = AstronomicalCalculator.GetPlanetMotionName(planetName, parsedTime).ToString(); break;
            }


            return new ContentResult { Content = returnVal, ContentType = "text/html" };
        }

    }
}
