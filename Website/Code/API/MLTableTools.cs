using OfficeOpenXml;
using System.Text;
using System.Xml.Linq;
using Newtonsoft.Json.Linq;
using VedAstro.Library;


namespace Website;


/// <summary>
/// Code to generate ML Table via Open API from a web/desktop client
/// </summary>
public class MLTableTools
{

    private readonly VedAstroAPI _api;


    //PUBLIC

    public MLTableTools(VedAstroAPI vedAstroApi) => _api = vedAstroApi;


    /// <summary>
    /// Given an excel file uploaded by user, send to server to process and extract out a Time list
    /// </summary>
    public async Task<List<Time>> GetTimeListFromExcel(byte[] inputedFile)
    {
        //make call, send file for processing
        var results = await _api.GetListNoPolling(_api.URL.GetMLTimeListFromExcel, inputedFile, Time.FromJsonList);

        return results;
    }

    public async Task<byte[]> GenerateMLTableExcel(List<Time> timeList, List<OpenAPIMetadata> columnNameList)
    {
        throw new NotImplementedException();
    }

    public async Task<string> GenerateMLTableCsv(List<Time> timeList, List<OpenAPIMetadata> columnNameList)
    {
        throw new NotImplementedException();
    }

    /// <summary>
    /// Send time list column as Json to API to make HTML table
    /// </summary>
    public async Task<string?> GenerateMLTableHtml(List<Time> timeList, List<OpenAPIMetadata> columnNameList)
    {
        //prepare to send data to API
        var temp = MLTable.FromData(timeList, columnNameList);
        var payloadJson = temp.ToJson();

        //send to api and get results
        var resultsRaw = await WriteToServerXmlReply(AppData.URL.GetEventsApi, root);

        //parse raw results
        List<Event> resultsParsed = Event.FromXml(resultsRaw);


        //make call, send file for processing
        var results = await _api.GetListNoPolling(_api.URL.GetMLTimeListFromExcel, inputedFile, Time.FromJsonList);

        return results;
    }

    public static async Task<WebResult<JToken>> WriteToServerJsonReply(string apiUrl, string stringData, int timeout = 60)
    {

        TryAgain:

        //ACT 1:
        //send data to URL, using JS for reliability & speed
        //also if call does not respond in time, we replay the call over & over
        string receivedData;
        try { receivedData = await VedAstro.Library.Tools.TaskWithTimeoutAndException(Post(apiUrl, stringData), TimeSpan.FromSeconds(timeout)); }

        //if fail replay and log it
        catch (Exception e)
        {
            var debugInfo = $"Call to \"{apiUrl}\" timeout at : {timeout}s";

           // WebLogger.Data(debugInfo);
#if DEBUG
            Console.WriteLine(debugInfo);
#endif
            goto TryAgain;
        }

        //ACT 2:
        //check raw data 
        if (string.IsNullOrEmpty(receivedData))
        {
            //log it
            //await WebLogger.Error($"BLZ > Call returned empty\n To:{apiUrl} with payload:\n{stringData}");

            //send failed empty data to caller, it should know what to do with it
            return new WebResult<JToken>(false, new JObject("CallEmptyError"));
        }

        //ACT 3:
        //return data as Json
        var writeToServerXmlReply = JObject.Parse(receivedData);
        var returnVal = WebResult<XElement>.FromJson(writeToServerXmlReply);

        //ACT 4:
        return returnVal;
    }

    public static async Task<string> Post(string apiUrl, string stringData)
    {
        //this call will take you to NetworkThread.js
        var rawPayload = await AppData.JsRuntime.InvokeAsync<string>(JS.postWrapper, apiUrl, stringData);

        //todo proper checking of status needed
        return rawPayload;
    }


}