using VedAstro.Library;

namespace Website;

public class MatchTools
{
    private readonly VedAstroAPI _api;


    public MatchTools(VedAstroAPI vedAstroApi)
    {
        _api = vedAstroApi;
    }

    public async Task<List<PersonKutaScore>> GetList(string personId)
    {
        //prepare url to call
        var url = $"{_api.URL.FindMatch}/PersonId/{personId}";
        var personKutaScore = await _api.GetList(url, PersonKutaScore.FromJsonList);

        return personKutaScore;

    }
}