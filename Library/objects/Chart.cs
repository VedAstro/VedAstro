using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Genso.Astrology.Library
{

    public class ChartId
    {

    }

    /// <summary>
    /// Represents an events report chart
    /// Note: made so that a chart can be saved and accessed later
    /// </summary>
    public class Chart
    {

        public string ContentSvg { get; set; }
        public string PersonId { get; }
        public XElement EventTagListXml { get; }
        public XElement StartTimeXml { get; }
        public XElement EndTimeXml { get; }
        public double DaysPerPixel { get; }


        public Chart(string contentSvg, string personId, XElement eventTagListXml, XElement startTimeXml,
            XElement endTimeXml, double daysPerPixel)
        {
            ContentSvg = contentSvg;
            PersonId = personId;
            EventTagListXml = eventTagListXml;
            StartTimeXml = startTimeXml; //root time xml
            EndTimeXml = endTimeXml; //root time xml
            DaysPerPixel = daysPerPixel;
        }


        public XElement ToXml()
        {
            var person = new XElement("Chart");
            var contentSvg = new XElement("ContentSvg", ContentSvg);
            var personId = new XElement("PersonId", PersonId);
            var daysPerPixel = new XElement("DaysPerPixel", DaysPerPixel);

            person.Add(
                personId,
                daysPerPixel,
                EventTagListXml,
                new XElement("StartTime", StartTimeXml),
                new XElement("EndTime", EndTimeXml),
                contentSvg);

            return person;
        }

        public static Chart FromXml(XElement personXml)
        {
            var personId = personXml.Element("PersonId")?.Value;
            var contentSvg = personXml.Element("ContentSvg")?.Value;
            var eventTagListXml = personXml.Element("EventTagList");
            var startTimeXml = personXml.Element("StartTime").Element("Time");
            var endTimeXml = personXml.Element("EndTime").Element("Time");
            var daysPerPixel = double.Parse(personXml.Element("DaysPerPixel")?.Value);

            var parsedPerson = new Chart(contentSvg, personId, eventTagListXml, startTimeXml, endTimeXml, daysPerPixel);

            return parsedPerson;
        }


        /// <summary>
        /// Unique data used to generate Hash
        /// </summary>
        public override int GetHashCode()
        {
            //get hash of all the fields & combine them
            var hash1 = PersonId.GetHashCode();
            var hash2 = Tools.GetStringHashCode(StartTimeXml.ToString());
            var hash3 = Tools.GetStringHashCode(EndTimeXml.ToString());
            var hash4 = Tools.GetStringHashCode(EventTagListXml.ToString());

            //take out negative before returning
            var hashCode = Math.Abs(hash1 + hash2 + hash3 + hash4);
            return hashCode;
        }

        /// <summary>
        /// Gets a nice identifiable name for this chart to show user 
        /// </summary>
        public object GetFormattedName(string personName)
        {
            var startTime = Time.FromXml(StartTimeXml);
            var startYear = startTime.GetStdYear();
            var startMonth = startTime.GetStdMonth();
            var endTime = Time.FromXml(EndTimeXml);
            var endYear = endTime.GetStdYear();
            var endMonth = endTime.GetStdMonth();
            return $"{personName} - {startMonth}/{startYear} to {endMonth}/{endYear}";
        }
    }
}
