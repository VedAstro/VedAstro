using System.Threading.Tasks;
using Genso.Astrology.Library;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;

namespace API
{
    public static class OpenAPI
    {
        /// <summary>
        /// https://www.vedastro.org/api/Location/Singapore/Time/23:59/31/12/2000/+08:00/Planet/Sun/Sign/
        /// </summary>
        [Function("Location")]
        public static async Task<HttpResponseData> Main([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "Location/{locationName}/Time/{hhmmStr}/{dateStr}/{monthStr}/{yearStr}/{offsetStr}/Planet/{planetNameStr}/{propertyName}")] HttpRequestData incomingRequest, string locationName, string hhmmStr, string dateStr, string monthStr, string yearStr, string offsetStr, string planetNameStr, string propertyName)
        {
            //log the call
            APILogger.Visitor(incomingRequest);

            PlanetName planetName;
            var planetNameResult = PlanetName.TryParse(planetNameStr, out planetName);
            var geoLocationResult = await Tools.AddressToGeoLocation(locationName);
            var geoLocation = geoLocationResult.Payload;

            //check result 1st before parsing
            if (!planetNameResult || !geoLocationResult.IsPass) { return APITools.FailMessage("Please check your input, it failed to parse.", incomingRequest); }

            //clean time text
            var timeStr = $"{hhmmStr} {dateStr}/{monthStr}/{yearStr} {offsetStr}";
            var parsedTime = new Time(timeStr, geoLocation);

            //based on property call the method
            var returnVal = "";
            switch (propertyName)
            {
                case "Rasi":
                case "Sign": returnVal = AstronomicalCalculator.GetPlanetRasiSign(planetName, parsedTime).ToString(); break;
                case "Navamsa": returnVal = AstronomicalCalculator.GetPlanetNavamsaSign(planetName, parsedTime).ToString(); break;
                case "Dwadasamsa": returnVal = AstronomicalCalculator.GetPlanetDwadasamsaSign(planetName, parsedTime).ToString(); break;
                case "Constellation": returnVal = AstronomicalCalculator.GetPlanetConstellation(parsedTime, planetName).ToString(); break;
                case "Kranti":
                case "Declination": returnVal = AstronomicalCalculator.GetPlanetDeclination(planetName, parsedTime).ToString(); break;
                case "AspectingPlanets": returnVal = AstronomicalCalculator.GetPlanetsAspectingPlanet(parsedTime, planetName).ToString(); break;
                case "Motion": returnVal = AstronomicalCalculator.GetPlanetMotionName(planetName, parsedTime).ToString(); break;
                case "FixedLongitude":
                case "NirayanaLongitude": returnVal = AstronomicalCalculator.GetPlanetNirayanaLongitude(parsedTime, planetName).ToString(); break;
                case "MovableLongitude":
                case "SayanaLongitude": returnVal = AstronomicalCalculator.GetPlanetSayanaLongitude(parsedTime, planetName).ToString(); break;
            }


            return APITools.PassMessage(returnVal, incomingRequest);
        }

    }
}
