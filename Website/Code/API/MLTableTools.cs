using OfficeOpenXml;
using System.Text;
using System.Xml.Linq;
using Newtonsoft.Json.Linq;
using VedAstro.Library;
using Microsoft.AspNetCore.Components.Forms;


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

    /// <summary>
    /// Send time list column as Json to API to make HTML table
    /// </summary>
    public async Task<T> GenerateMLTable<T>(List<Time> timeList, List<OpenAPIMetadata> columnNameList, string selectedFormat)
    {
        //prepare to send data to API
        var payloadJson = new JObject();
        payloadJson["TimeList"] = Tools.ListToJson(timeList);
        payloadJson["ColumnData"] = Tools.ListToJson(columnNameList);


        //send to api and get results 
        //.../GenerateMLTable/HTML or CSV or EXCEL
        var url = _api.URL.GenerateMLTable + $"/{selectedFormat}";

        if (typeof(T) == typeof(string))
        {
            var xListJson = await Tools.WriteServer<JObject, JObject>(HttpMethod.Post, url, payloadJson);

            var htmlTable = xListJson["Payload"]?[selectedFormat].Value<string>();
            return (T)(object)htmlTable;
        }
        else if (typeof(T) == typeof(byte[]))
        {
            var xListJson = await Tools.WriteServer<byte[], JObject>(HttpMethod.Post, url, payloadJson);

            return (T)(object)xListJson;
        }


        throw new Exception("END OF LINE!");
    }


}