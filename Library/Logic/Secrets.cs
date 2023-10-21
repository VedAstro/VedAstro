using System;

namespace VedAstro.Library
{
    public class Secrets
    {
        public static string? GoogleAPIKey => Environment.GetEnvironmentVariable("GoogleAPIKey");
        public static string? AutoEmailerConnectString => Environment.GetEnvironmentVariable("AutoEmailerConnectString"); //vedastro-api-data
        public static string? VedAstroApiStorageKey => Environment.GetEnvironmentVariable("VedAstroApiStorageKey"); //vedastro-api-data
        public static string? VedAstroCentralStorageKey => Environment.GetEnvironmentVariable("VedAstroCentralStorageKey"); //vedastro-api-data
        public static string? API_STORAGE => Environment.GetEnvironmentVariable("API_STORAGE");
        public static string? OpenAPICallDelayMs => Environment.GetEnvironmentVariable("OpenAPICallDelayMs");
        public static string? BING_IMAGE_SEARCH => Environment.GetEnvironmentVariable("BING_IMAGE_SEARCH");
        public static string? WEB_STORAGE => Environment.GetEnvironmentVariable("WEB_STORAGE");
        public static string? EnableCache => Environment.GetEnvironmentVariable("EnableCache");
        public static string? SLACK_EMAIL_WEBHOOK => Environment.GetEnvironmentVariable("SLACK_EMAIL_WEBHOOK");
    }
}
