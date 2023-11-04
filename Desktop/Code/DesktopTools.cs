using System.Xml.Linq;
using VedAstro.Library;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System.Threading;
using Desktop.Pages;
using System.Reflection;
using Newtonsoft.Json.Linq;
using System.Text.Json;

namespace Desktop
{

    /// <summary>
    /// Simple class holding general functions used in project
    /// </summary>
    public static class DesktopTools
    {

        //░█▀▀▀ ▀█▀ ░█▀▀▀ ░█─── ░█▀▀▄ ░█▀▀▀█ 　 ─█▀▀█ ░█▄─░█ ░█▀▀▄ 　 ░█▀▀█ ░█▀▀█ ░█▀▀▀█ ░█▀▀█ ░█▀▀▀█ 
        //░█▀▀▀ ░█─ ░█▀▀▀ ░█─── ░█─░█ ─▀▀▀▄▄ 　 ░█▄▄█ ░█░█░█ ░█─░█ 　 ░█▄▄█ ░█▄▄▀ ░█──░█ ░█▄▄█ ─▀▀▀▄▄ 
        //░█─── ▄█▄ ░█▄▄▄ ░█▄▄█ ░█▄▄▀ ░█▄▄▄█ 　 ░█─░█ ░█──▀█ ░█▄▄▀ 　 ░█─── ░█─░█ ░█▄▄▄█ ░█─── ░█▄▄▄█



        //░█▀▄▀█ ░█▀▀▀ ▀▀█▀▀ ░█─░█ ░█▀▀▀█ ░█▀▀▄ ░█▀▀▀█ 
        //░█░█░█ ░█▀▀▀ ─░█── ░█▀▀█ ░█──░█ ░█─░█ ─▀▀▀▄▄ 
        //░█──░█ ░█▄▄▄ ─░█── ░█─░█ ░█▄▄▄█ ░█▄▄▀ ░█▄▄▄█




        /// <summary>
        /// tries to get location from local storage, if not found
        /// gets from API and saves a copy for future
        /// </summary>
        /// <returns></returns>
        public static async Task<GeoLocation> GetClientLocation(string apiKey)
        {
            //try get from browser storage
            var clientLocXml = await AppData.JsRuntime.GetProperty("ClientLocation");
            var isFound = clientLocXml != null;
            //if got cache, then just parse that and return (1 http call saved)
            GeoLocation parsedLocation;
            if (isFound)
            {
                var locationXml = XElement.Parse(clientLocXml);
                parsedLocation = GeoLocation.FromXml(locationXml);
            }
            //no cache, call Google API with IP
            else
            {
                parsedLocation = await GeoLocation.FromIpAddress(apiKey);
                //save for future use
                await AppData.JsRuntime.SetProperty("ClientLocation", parsedLocation.ToXml().ToString());
            }

            Console.WriteLine($"Client Location:{parsedLocation.Name()}");

            return parsedLocation;
        }


        /// <summary>
        /// Adds a message to API server records from user
        /// </summary>
        public static async Task SendMailToAPI(string? title, string? description)
        {
            //package message data to be sent
            var textXml = new XElement("Title", title);
            var emailXml = new XElement("Text", description);
            var userIdXml = new XElement("UserId", AppData.CurrentUser?.Id);
            var visitorIdXml = new XElement("VisitorId", AppData.VisitorId);
            var messageXml = new XElement("Message", userIdXml, visitorIdXml, emailXml, Tools.TimeStampSystemXml, Tools.TimeStampServerXml, textXml);

            //send message to API server
            await ServerManager.WriteToServerXmlReply(AppData.URL.AddMessageApi, messageXml);
        }

    }
}
