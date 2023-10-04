using VedAstro.Library;

namespace Library.API;

public class MatchTools
{
    private readonly VedAstroAPI _api;


    public MatchTools(VedAstroAPI vedAstroApi)
    {
        _api = vedAstroApi;
    }

    public async Task<List<PersonKutaScore>> GetList(string personId)
    {
        //CHECK CACHE
        //cache will be cleared when update is needed
        //if (CachedPersonKutaScore.Any()) { return CachedPersonKutaScore; }

        //prepare url to call
        var url = $"{_api.URL.FindMatch}/PersonId/{personId}";
        var personKutaScore = await _api.GetList(url, PersonKutaScore.FromJsonList);

        return personKutaScore;

    }
}