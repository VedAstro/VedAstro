using Newtonsoft.Json.Linq;
using VedAstro.Library;
using Microsoft.AspNetCore.Components.Forms;


namespace Library.API;


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

        //prepare url to call
        var url = $"{_api.URL.GetMLTimeListFromExcel}/UserId/{_api.UserId}/VisitorId/{_api.VisitorID}";

        //make call
        var results = await _api.GetList(url, Time.FromJsonList);

        return results;
    }

    public async Task<byte[]> GenerateMLTableExcel(List<Time> timeList, List<OpenAPIMetadata> columnNameList)
    {
        throw new NotImplementedException();
    }
}