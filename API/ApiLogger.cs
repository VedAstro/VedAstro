using System;
using System.Diagnostics;
using System.Net.Http;
using System.Threading.Tasks;
using System.Xml.Linq;
using Genso.Astrology.Library;

namespace API;

/// <summary>
/// Custom simple logger for API, auto log to AppLog.xml file
/// </summary>
public static class ApiLogger
{
    private const string AppLogXml = "AppLog.xml";
    private const string VisitorLogXml = "VisitorLog.xml";
    private const string ContainerName = "vedastro-site-data";



    //PUBLIC FUNCTIONS

    /// <summary>
    /// Logs an error directly to AppLog.xml
    /// note: request can be null
    /// </summary>
    public static async Task Error(Exception exception, HttpRequestMessage req)
    {
        //get data out of exception
        var errorXml = Tools.ExtractDataFromException(exception);

        //get caller data for more debug info
        var ipAddress = req?.GetCallerIp()?.ToString() ?? "no ip";

        errorXml.Add(Tools.TimeStampServerXml);
        errorXml.Add(new XElement("CallerIP", ipAddress));
        errorXml.Add(new XElement("Url", req?.RequestUri));
        errorXml.Add(new XElement("RequestBody"), APITools.RequestToXmlString(req));
        

        //add error data to main app log file
        await APITools.AddXElementToXDocumentAzure(errorXml, AppLogXml, ContainerName);

    }

    public static async Task Visitor(HttpRequestMessage req)
    {
        //get data out of exception
        //var errorXml = Tools.ExtractDataFromException(exception);

        //get caller data for more debug info
        var ipAddress = req?.GetCallerIp()?.ToString() ?? "no ip";

        var visitorXml = new XElement("Visitor");

        visitorXml.Add(Tools.TimeStampServerXml);
        visitorXml.Add(new XElement("CallerIP", ipAddress));
        visitorXml.Add(new XElement("Url", req?.RequestUri));
        visitorXml.Add(new XElement("Data"), APITools.RequestToXmlString(req));
        visitorXml.Add(Tools.TimeStampServerXml);

        //add error data to main app log file
        await APITools.AddXElementToXDocumentAzure(visitorXml, VisitorLogXml, ContainerName);

    }



    //PRIVATE FUNCTIONS



}