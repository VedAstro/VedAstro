using VedAstro.Library;

namespace Website;

public class EventsChartTools
{
    private readonly VedAstroAPI _api;

    private static List<EventData> CachedEventDataList { get; set; } = new(); //if empty que to get new list

    public EventsChartTools(VedAstroAPI vedAstroApi) => _api = vedAstroApi;

    /// <summary>
    /// Calls API till get events chart
    /// </summary>
    public async Task<string> GetEventsChart(Person person, TimeRange timeRange, List<EventTag> inputedEventTags, double daysPerPixelRaw, ChartOptions options, string ayanamsaName)
    {
        //no person no entry!
        if (Person.Empty.Equals(person)) { throw new InvalidOperationException("NO CHART FOR EMPTY PERSON!"); }

        //generate URL to get chart from API
        var eventsChartApiCallUrl = GetEventsChartApiUrl(person, timeRange, inputedEventTags, daysPerPixelRaw, options, ayanamsaName);

        //make the call to API, NOTE:call is held here
        var chartString = await _api.PollApiTillDataEventsChart(eventsChartApiCallUrl);

        return chartString;
    }

    /// <summary>
    /// Get Events chart api call GET URL that is sent to API,
    /// note: used as a shortcut in website rather going into network tab in F12 
    /// </summary>
    public string GetEventsChartApiUrl(Person person, TimeRange timeRange, List<EventTag> inputedEventTags, double daysPerPixelRaw,
        ChartOptions summaryOptions, string ayanamsaName)
    {
        //put specs to make chart into a URL format
        var chartSpecsUrl = EventsChart.FromData(person, timeRange, inputedEventTags, daysPerPixelRaw, summaryOptions).ToUrl();

        //add in server address & API call name
        var finalUrl = _api.URL.GetEventsChart + chartSpecsUrl + $"/Ayanamsa/{ayanamsaName}";

        return finalUrl;
    }

    /// <summary>
    /// Get Events chart api call GET URL that is sent to API, but with time preset in string like "1week"
    /// </summary>
    //public string GetEventsChartApiUrl(Person person, string timePreset, List<EventTag> inputedEventTags, double daysPerPixelRaw,
    //    ChartOptions summaryOptions, string ayanamsaName)
    //{
    //    //put specs to make chart into a URL format
    //    var chartSpecsUrl = EventsChart.FromData(person, timeRange, inputedEventTags, daysPerPixelRaw, summaryOptions).ToUrl();

    //    //add in server address & API call name
    //    var finalUrl = _api.URL.GetEventsChart + chartSpecsUrl + $"/Ayanamsa/{ayanamsaName}";

    //    return finalUrl;
    //}


    /// <summary>
    /// calls server for a list of EventDataList.xml in nice JSON form
    /// </summary>
    /// <returns></returns>
    //public async Task<List<EventData>> GetEventDataList()
    //{

    //    //CHECK CACHE
    //    //cache will be cleared when update is needed
    //    if (CachedEventDataList.Any())
    //    {
    //        return CachedEventDataList;
    //    }

    //    //prepare url to call
    //    var url = $"{_api.URL.GetEventDataList}";
    //    var listNoPolling = await _api.GetListNoPolling(url, EventData.FromJsonList);

    //    //NOTE: ToList is needed to make clone, else copies by ref and is lost
    //    CachedEventDataList = listNoPolling.ToList();

    //    return CachedEventDataList;

    //}
}