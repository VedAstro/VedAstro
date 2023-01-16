using System;
using System.Diagnostics;
using System.Net.Http;
using System.Threading.Tasks;
using System.Xml.Linq;
using Genso.Astrology.Library;

namespace API;

/// <summary>
/// Custom logger for API, not initialization needed auto log to AppLog.xml file
/// </summary>
public static class Log
{
    private const string AppLogXml = "AppLog.xml";
    private const string ContainerName = "vedastro-site-data";



    //PUBLIC FUNCTIONS

    /// <summary>
    /// Logs an error directly to AppLog.xml
    /// note: request can be null
    /// </summary>
    public static async Task Error(Exception exception, HttpRequestMessage req)
    {
        //get data out of exception
        var errorXml = ExtractDataFromException(exception);

        //get caller data for more debug info
        var ipAddress = req?.GetCallerIp()?.ToString() ?? "no ip";

        errorXml.Add(new XElement("CallerIP", ipAddress));
        errorXml.Add(new XElement("Url", req?.RequestUri));
        errorXml.Add(new XElement("RequestBody"), APITools.RequestToXmlString(req));

        //add error data to main app log file
        await APITools.AddXElementToXDocumentAzure(errorXml, AppLogXml, ContainerName);

    }



    //PRIVATE FUNCTIONS


    private static XElement ExtractDataFromException(Exception e)
    {

        //place to store the exception data
        string fileName;
        string methodName;
        int line;
        int columnNumber;
        string message;
        string source;

        //get the data from the exception
        //this fills the fields declared above
        ExtractDataFromException();

        //put together the new error record
        var newRecord = new XElement("Error",
            new XElement("Message", message),
            new XElement("Source", source),
            new XElement("FileName", fileName),
            new XElement("SourceLineNumber", line),
            new XElement("SourceColNumber", columnNumber),
            new XElement("MethodName", methodName),
            new XElement("Time", GetNow())
        );

        //place new record into the log list
        return newRecord;

        //LOCAL FUNCTION
        void ExtractDataFromException()
        {

            //get the exception that started it all
            var originalException = e.GetBaseException();

            //extract the data from the error
            StackTrace st = new StackTrace(e, true);

            //Get the first stack frame
            StackFrame frame = st.GetFrame(st.FrameCount - 1);

            //Get the file name
            fileName = frame.GetFileName();

            //Get the method name
            methodName = frame.GetMethod().Name;

            //Get the line number from the stack frame
            line = frame.GetFileLineNumber();

            //Get the column number
            columnNumber = frame.GetFileColumnNumber();

            message = originalException.ToString();

            source = originalException.Source;
        }
    }

    /// <summary>
    /// Gets now time in UTC +8:00
    /// Because server time is uncertain, all change to UTC8
    /// </summary>
    private static string GetNow()
    {
        //create utc 8
        var utc8 = new TimeSpan(8, 0, 0);
        //get now time in utc 0
        var nowTime = DateTimeOffset.Now.ToUniversalTime();
        //convert time utc 0 to utc 8
        var utc8Time = nowTime.ToOffset(utc8);

        //return converted time to caller
        return utc8Time.ToString(Time.DateTimeFormatSeconds);
    }


}