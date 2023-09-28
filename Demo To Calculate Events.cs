

var startTime = Time.Now(GeoLocation.Bangkok);
var endTime = startTime.AddHours(4380); //6 months in hours
var johnDoe = new Person("Juliet", Time.StandardHoroscope(), Gender.Female);

//# set how accurately the start & end time of each event is calculated
//# exp: setting 1 hour, means given in a time range of 1 day, it will be checked 24 times 
var precisionInHours = 1;

//set what events to include
var tagList = new List<EventTag>
{
    EventTag.PD1,EventTag.PD2, EventTag.PD3, EventTag.PD4,EventTag.PD5, //dasa, antar, pratuantar, prana, sookshma
    EventTag.AshtakvargaGochara,
    EventTag.Gochara
};

//do calculation (heavy computation)
var eventList = await EventManager.CalculateEvents(precisionInHours,
    startTime,
    endTime,
    GeoLocation.Bangkok, johnDoe, tagList);

//# print out each event
foreach (var _event in eventList) { Console.WriteLine(_event.ToString()); }


