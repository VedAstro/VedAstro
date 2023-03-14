using System;
using System.Threading.Tasks;
using System.Xml.Linq;
using Genso.Astrology.Library;
using Microsoft.Azure.Functions.Worker.Http;

namespace API;

/// <summary>
/// Custom simple logger for API, auto log to AppLog.xml file
/// </summary>
public static class APILogger
{
    private const string AppLogXml = "AppLog.xml";
    private const string VisitorLogXml = "VisitorLog.xml";
    private const string ContainerName = "vedastro-site-data";

    private static readonly XElement SourceXml = new("Source", "APILogger");



    //PUBLIC FUNCTIONS

    /// <summary>
    /// Logs an error directly to AppLog.xml
    /// note: request can be null
    /// </summary>
    public static async Task Error(Exception exception, HttpRequestData req)
    {

        //get data out of exception
        var errorXml = Tools.ExtractDataFromException(exception);

        //get data out of request
        var requestDataXml = await APITools.RequestToXml(req);

        //add error data to main app log file
        var visitorXml = new XElement("Visitor", requestDataXml, errorXml);

        //stamp it!
        visitorXml.Add(Tools.BranchXml, SourceXml);
        visitorXml.Add(Tools.TimeStampSystemXml);
        visitorXml.Add(Tools.TimeStampServerXml);

        await APITools.AddXElementToXDocumentAzure(visitorXml, AppLogXml, ContainerName);

    }
   

    public static async Task Visitor(HttpRequestData req)
    {
        //get caller data for more debug info
        var ipAddress = req?.GetCallerIp()?.ToString() ?? "no ip";

        var visitorXml = new XElement("Visitor");

        visitorXml.Add(Tools.BranchXml, SourceXml);
        visitorXml.Add(await APITools.RequestToXml(req));
        visitorXml.Add(Tools.TimeStampSystemXml);
        visitorXml.Add(Tools.TimeStampServerXml);

        //add error data to main app log file
        await APITools.AddXElementToXDocumentAzure(visitorXml, VisitorLogXml, ContainerName);

    }
    public static async Task Data(string textData, HttpRequestData req)
    {

        var visitorXml = new XElement("Visitor");

        visitorXml.Add(Tools.BranchXml, SourceXml);
        visitorXml.Add(await APITools.RequestToXml(req));
        visitorXml.Add(new XElement("Data"),new XElement("Text", textData));
        visitorXml.Add(Tools.TimeStampSystemXml);
        visitorXml.Add(Tools.TimeStampServerXml);

        //add error data to main app log file
        await APITools.AddXElementToXDocumentAzure(visitorXml, VisitorLogXml, ContainerName);

    }




    //PRIVATE FUNCTIONS



}