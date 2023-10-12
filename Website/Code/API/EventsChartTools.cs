using VedAstro.Library;

namespace Website;

public class EventsChartTools
{
    private readonly VedAstroAPI _api;

    public EventsChartTools(VedAstroAPI vedAstroApi) => _api = vedAstroApi;

    public async Task<string> GetEventsChart(Person person, TimeRange timeRange, List<EventTag> inputedEventTags, int maxWidth, ChartOptions options)
    {
        //no person no entry!
        if (Person.Empty.Equals(person)) { throw new InvalidOperationException("NO CHART FOR EMPTY PERSON!"); }

        //generate URL to get chart from API
        var eventsChartApiCallUrl = GetEventsChartApiUrl(person,timeRange,inputedEventTags, maxWidth, options);

        //make the call to API, NOTE:call is held here
        var chartString = await _api.PollApiTillData(eventsChartApiCallUrl);

        return chartString;
    }

    /// <summary>
    /// Get Events chart api call GET URL that is sent to API,
    /// note: used as a shortcut in website rather going into network tab in F12 
    /// </summary>
    public string GetEventsChartApiUrl(Person person, TimeRange timeRange, List<EventTag> inputedEventTags, int maxWidth,
        ChartOptions summaryOptions)
    {
        //put specs to make chart into a URL format
        var chartSpecsUrl = EventsChart.FromData(person, timeRange, inputedEventTags, maxWidth, summaryOptions).ToUrl();

        //add in server address & API call name
        var finalUrl = _api.URL.GetEventsChart + chartSpecsUrl;

        return finalUrl;
    }

}