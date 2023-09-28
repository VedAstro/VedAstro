using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace VedAstro.Library
{
    public readonly record struct LogItem(Time Time, string VisitorId, string UserId, string Url, string Branch, string Source, string Data)
    {
        public static LogItem FromXml(XElement logItemXml)
        {

            var UserId = logItemXml.Element("UserId")?.Value ?? "";
            var VisitorId = logItemXml.Element("VisitorId")?.Value ?? "";
            var Url = logItemXml.Element("Url")?.Value ?? "";
            var Branch = logItemXml.Element("Branch")?.Value ?? "";
            var Source = logItemXml.Element("Source")?.Value ?? "";
            var Data = logItemXml.Element("Data")?.Value ?? "";


            var timeString = logItemXml.Element("TimeStampServer")?.Value ?? "00:00 01/01/2000 +08:00";
            //know issue to have "." instead of "/" for date separator, so change it here if at all
            timeString = timeString.Replace('.', '/');
            var serverLocation = new GeoLocation("Ipoh", 101.0901, 4.5975);
            var parsedTime = new Time(timeString, serverLocation);

            var parsedPerson = new LogItem(parsedTime, VisitorId, UserId, Url, Branch, Source, Data);

            return parsedPerson;

        }

        public static List<LogItem> FromXml(IEnumerable<XElement> xmlRecordList) => xmlRecordList.Select(logItemXml => LogItem.FromXml(logItemXml)).ToList();

        public override string ToString()
        {
            return $"Visitor:{VisitorId} - Url:{Url} - Branch:{Branch} - Data:{Data} - ";
        }
    }

}
