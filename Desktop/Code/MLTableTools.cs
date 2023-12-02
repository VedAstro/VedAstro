using OfficeOpenXml;
using System.Text;
using System.Xml.Linq;
using Newtonsoft.Json.Linq;
using VedAstro.Library;
using Microsoft.AspNetCore.Components.Forms;


namespace Desktop;


/// <summary>
/// Code to generate ML Table via Open API from a web/desktop client
/// </summary>
public class MLTableTools
{


    //PUBLIC


    /// <summary>
    /// Given an excel file uploaded by user, send to server to process and extract out a Time list
    /// </summary>
    public static async Task<List<Time>> GetTimeListFromExcel(Stream inputedFile)
    {
        var foundRawTimeList = await Tools.ExtractTimeColumnFromExcel(inputedFile);
        var foundGeoLocationList = await Tools.ExtractLocationColumnFromExcel(inputedFile);

        //3 : COMBINE DATA
        var returnList = foundRawTimeList.Select(dateTimeOffset => new Time(dateTimeOffset, foundGeoLocationList[foundRawTimeList.IndexOf(dateTimeOffset)])).ToList();

        return returnList;
    }

    /// <summary>
    /// Send time list column as Json to API to make HTML table
    /// </summary>
    public static async Task<T> GenerateMLTable<T>(List<Time> timeList, List<OpenAPIMetadata> columnNameList, string selectedFormat)
    {

        var newMLTable = MLTable.FromData(timeList, columnNameList);


        switch (selectedFormat)
        {
            case "HTML":
                {
                    return (T)(object)newMLTable.ToHtml();
                }
            case "CSV":
                {
                    return (T)(object)newMLTable.ToCSV();
                }
            case "JSON":
                {
                    return (T)(object)newMLTable.ToJson();
                }
            case "EXCEL":
                {
                    return (T)(object)newMLTable.ToExcel();
                }

            default: throw new Exception("END OF LINE!");

        }



        ////prepare to send data to API
        //var payloadJson = new JObject();
        //payloadJson["TimeList"] = Tools.ListToJson(timeList);
        //payloadJson["ColumnData"] = Tools.ListToJson(columnNameList);


        ////send to api and get results 
        ////.../GenerateMLTable/HTML or CSV or EXCEL
        //var url = _api.URL.GenerateMLTable + $"/{selectedFormat}";

        //if (typeof(T) == typeof(string))
        //{
        //    var xListJson = await Tools.WriteServer<JObject, JObject>(HttpMethod.Post, url, payloadJson);

        //    var htmlTable = xListJson["Payload"]?[selectedFormat].Value<string>();
        //    return (T)(object)htmlTable;
        //}
        //else if (typeof(T) == typeof(byte[]))
        //{
        //    var xListJson = await Tools.WriteServer<byte[], JObject>(HttpMethod.Post, url, payloadJson);

        //    return (T)(object)xListJson;
        //}


        //throw new Exception("END OF LINE!");
    }


}