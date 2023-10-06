using VedAstro.Library;


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

    public async Task<string> GenerateMLTableCsv(List<Time> timeList, List<OpenAPIMetadata> columnNameList)
    {
        throw new NotImplementedException();
    }

    public async Task<string?> GenerateMLTableHtml(List<Time> timeList, List<OpenAPIMetadata> columnNameList)
    {

        return @"

<table>
  <tr>
    <th>Header 1</th>
    <th>Header 2</th>
  </tr>
  <tr>
    <td>Row 1 Data 1</td>
    <td>Row 1 Data 2</td>
  </tr>
  <tr>
    <td>Row 2 Data 1</td>
    <td>Row 2 Data 2</td>
  </tr>
</table>

";



    }
}