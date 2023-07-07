using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VedAstro.Library
{
    public class Secrets
    {
        public static string? AutoEmailerConnectString => Environment.GetEnvironmentVariable("AutoEmailerConnectString"); //vedastro-api-data
        public static string? API_STORAGE => Environment.GetEnvironmentVariable("API_STORAGE");
        public static string? BING_IMAGE_SEARCH => Environment.GetEnvironmentVariable("BING_IMAGE_SEARCH");
        public static string? WEB_STORAGE => Environment.GetEnvironmentVariable("WEB_STORAGE");
        public static string? EnableCache => Environment.GetEnvironmentVariable("EnableCache");
        public static string? SLACK_EMAIL_WEBHOOK => Environment.GetEnvironmentVariable("SLACK_EMAIL_WEBHOOK");
    }
}
