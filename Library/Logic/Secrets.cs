using System;

namespace VedAstro.Library
{

    /// <summary>
    /// Central place access API keys at runtime, other security related instance data
    /// This data is picked up from "local.settings.json"
    /// </summary>
    public class Secrets
    {
        public static string Password => Environment.GetEnvironmentVariable("Password") ?? throw new Exception("One of the API keys missing sweetheart!");
        public static string AzureGeoLocationStorageKey => Environment.GetEnvironmentVariable("AzureGeoLocationStorageKey") ?? throw new Exception("One of the API keys missing sweetheart!");
        public static string GoogleAPIKey => Environment.GetEnvironmentVariable("GoogleAPIKey") ?? throw new Exception("One of the API keys missing sweetheart!");
        public static string AzureMapsAPIKey => Environment.GetEnvironmentVariable("AzureMapsAPIKey") ?? throw new Exception("One of the API keys missing sweetheart!");
        public static string IpDataAPIKey => Environment.GetEnvironmentVariable("IpDataAPIKey") ?? throw new Exception("One of the API keys missing sweetheart!");
        public static string AutoEmailerConnectString => Environment.GetEnvironmentVariable("AutoEmailerConnectString") ?? throw new Exception("One of the API keys missing sweetheart!"); //vedastro-api-data
        public static string VedAstroApiStorageKey => Environment.GetEnvironmentVariable("VedAstroApiStorageKey") ?? throw new Exception("One of the API keys missing sweetheart!"); //vedastro-api-data
        public static string VedAstroCentralStorageKey => Environment.GetEnvironmentVariable("VedAstroCentralStorageKey") ?? throw new Exception("One of the API keys missing sweetheart!"); //vedastro-api-data
        public static string API_STORAGE => Environment.GetEnvironmentVariable("API_STORAGE") ?? throw new Exception("One of the API keys missing sweetheart!");
        public static string OpenAPICallDelayMs => Environment.GetEnvironmentVariable("OpenAPICallDelayMs") ?? throw new Exception("One of the API keys missing sweetheart!");
        public static string BING_IMAGE_SEARCH => Environment.GetEnvironmentVariable("BING_IMAGE_SEARCH") ?? throw new Exception("One of the API keys missing sweetheart!");
        public static string WEB_STORAGE => Environment.GetEnvironmentVariable("WEB_STORAGE") ?? throw new Exception("One of the API keys missing sweetheart!");
        public static string EnableCache => Environment.GetEnvironmentVariable("EnableCache") ?? throw new Exception("One of the API keys missing sweetheart!");
        public static string SLACK_EMAIL_WEBHOOK => Environment.GetEnvironmentVariable("SLACK_EMAIL_WEBHOOK") ?? throw new Exception("One of the API keys missing sweetheart!");
    }
}
