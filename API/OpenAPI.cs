using System.Threading.Tasks;
using VedAstro.Library;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;

namespace API
{
    public static class OpenAPI
    {
        /// <summary>
        /// https://api.vedastro.org/Location/Singapore/Time/23:59/31/12/2000/+08:00/Planet/Sun/Sign/
        /// </summary>
        [Function(nameof(OneProperty))]
        public static async Task<HttpResponseData> OneProperty([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "Location/{locationName}/Time/{hhmmStr}/{dateStr}/{monthStr}/{yearStr}/{offsetStr}/Planet/{planetNameStr}/{propertyName}")] HttpRequestData incomingRequest, string locationName, string hhmmStr, string dateStr, string monthStr, string yearStr, string offsetStr, string planetNameStr, string propertyName)
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
            }


            return APITools.PassMessage(returnVal, incomingRequest);
        }

        /// <summary>
        /// Here comes calls that with 2 level properties like planet strength
        /// https://api.vedastro.org/Location/Singapore/Time/23:59/31/12/2000/+08:00/Planet/Sun/Strength/Temporal
        /// </summary>
        [Function(nameof(TwoProperty))]
        public static async Task<HttpResponseData> TwoProperty([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "Location/{locationName}/Time/{hhmmStr}/{dateStr}/{monthStr}/{yearStr}/{offsetStr}/Planet/{planetNameStr}/{propertyName1}/{propertyName2}")] HttpRequestData incomingRequest, string locationName, string hhmmStr, string dateStr, string monthStr, string yearStr, string offsetStr, string planetNameStr, string propertyName1, string propertyName2)
        {
            //log the call
            APILogger.Visitor(incomingRequest);

            PlanetName planetName;
            var planetNameResult = PlanetName.TryParse(planetNameStr, out planetName);
            var geoLocationResult = await Tools.AddressToGeoLocation(locationName);
            var geoLocation = geoLocationResult.Payload;

            //check result 1st before parsing
            if (!planetNameResult || !geoLocationResult.IsPass) { return APITools.FailMessage("Please check your input, it failed to parse. Check the caps/spelling/parameter order!", incomingRequest); }

            //clean time text
            var timeStr = $"{hhmmStr} {dateStr}/{monthStr}/{yearStr} {offsetStr}";
            var parsedTime = new Time(timeStr, geoLocation);

            //based on property call the method
            var returnVal = "";
            switch (propertyName1)
            {

                //LONGITUDE
                case "Longitude":
                    {
                        switch (propertyName2)
                        {
                            case "Fixed":
                            case "Nirayana": returnVal = AstronomicalCalculator.GetPlanetNirayanaLongitude(parsedTime, planetName).ToString(); break;
                            case "Movable":
                            case "Sayana": returnVal = AstronomicalCalculator.GetPlanetSayanaLongitude(parsedTime, planetName).ToString(); break;
                        }
                        break;
                    }
                case "Strength":
                    {
                        switch (propertyName2)
                        {
                            case "Total":
                            case "ShadbalaPinda": returnVal = AstronomicalCalculator.GetPlanetShadbalaPinda(planetName, parsedTime).ToString(); break;

                            case "Positional":
                            case "Sthana": returnVal = AstronomicalCalculator.GetPlanetSthanaBala(planetName, parsedTime).ToString(); break;

                            case "Directional":
                            case "Dig": returnVal = AstronomicalCalculator.GetPlanetDigBala(planetName, parsedTime).ToString(); break;
                            
                            case "Temporal":
                            case "Kala": returnVal = AstronomicalCalculator.GetPlanetKalaBala(planetName, parsedTime).ToString(); break;

                            case "Motional":
                            case "Chesta": returnVal = AstronomicalCalculator.GetPlanetChestaBala(planetName, parsedTime).ToString(); break;

                            case "Natural":
                            case "Naisargika": returnVal = AstronomicalCalculator.GetPlanetNaisargikaBala(planetName, parsedTime).ToString(); break;

                            case "Aspect":
                            case "Drik": returnVal = AstronomicalCalculator.GetPlanetDrikBala(planetName, parsedTime).ToString(); break;
                        }
                        break;
                    }

            }


            return APITools.PassMessage(returnVal, incomingRequest);
        }

    }
}
