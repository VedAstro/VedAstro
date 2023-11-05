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


		/// <summary>
		/// Special function to catch async exceptions, but has to be called correctly
		/// Note:
		/// - If caught here overwrites default blazor error handling 
		/// - Not all await calls need this only the top level needs this,
		/// example use inside OnClick or OnInitialized will do.
		/// example: await InvokeAsync(async () => await DeletePerson()).HandleErrors();
		/// </summary>
		public static async Task Try(this Task invocation, IJSRuntime jsRuntime)
		{
			//counts of failure before force refresh page
			const int failureThreshold = 3;

			try
			{
				//try to make call normally
				await invocation;
			}
			catch (Exception e)
			{

				//based on error show the appropriate message
				switch (e)
				{
					//no internet just, just show dialog box and do nothing
					case NoInternetError:
						await jsRuntime.ShowAlert("error", AlertText.NoInternet, true);
						break;

					//here we have internet but somehow failed when talking to API server
					//possible cause:
					// - code mismatch between client & server
					// - slow or unstable internet connection
					//best choice is to redirect 
					case ApiCommunicationFailed:
						await jsRuntime.ShowAlert("error", AlertText.ServerConnectionProblem(), true);
						break;

					//failure here can't be recovered, so best choice is to refresh page to home
					//redirect with reload to clear memory & restart app
					default:
						//note : for unknown reason, when app starts multiple failures occur, for now
						if (AppData.FailureCount > failureThreshold)
						{
							await jsRuntime.ShowAlert("warning", AlertText.SorryNeedRefreshToHome, true);
							await jsRuntime.LoadPage(PageRoute.Home);
						}
						else
						{
							AppData.FailureCount++;
							Console.WriteLine($"BLZ: Failure Count: {AppData.FailureCount}");
						}
						break;
				}

#if DEBUG
				//if running locally, print error to console
				Console.WriteLine(e.ToString());
#else
                //if Release log error & end silently
                WebLogger.Error(e, "Error from WebsiteTools.Try()");
#endif

				//note exception will not go past this point,
				//even calling throw will do nothing
				//throw;
			}
		}

	}
}
