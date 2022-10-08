using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Genso.Astrology.Library
{
    public class Chart
    {
        public Chart(string contentSvg, int personHash, XElement eventTagListXml, XElement startTimeXml,
            XElement endTimeXml, double daysPerPixel)
        {
            ContentSvg = contentSvg;
            PersonHash = personHash;
            EventTagListXml = eventTagListXml;
            StartTimeXml = startTimeXml;
            EndTimeXml = endTimeXml;
            DaysPerPixel = daysPerPixel;
        }


        public string ContentSvg { get; set; }
        public int PersonHash { get; }
        public XElement EventTagListXml { get; }
        public XElement StartTimeXml { get; }
        public XElement EndTimeXml { get; }
        public double DaysPerPixel { get; }

        public XElement ToXml()
        {
            var person = new XElement("Chart");
            var contentSvg = new XElement("ContentSvg", ContentSvg);
            var personHash = new XElement("PersonHash", PersonHash);
            var daysPerPixel = new XElement("DaysPerPixel", DaysPerPixel);

            person.Add(contentSvg,
                personHash,
                daysPerPixel,
                EventTagListXml,
                new XElement("StartTime",
                    StartTimeXml),
                new XElement("EndTime",
                    EndTimeXml));

            return person;
        }

        public static Chart FromXml(XElement personXml)
        {
            var personHash = int.Parse(personXml.Element("PersonHash")?.Value);
            var contentSvg = personXml.Element("ContentSvg")?.Value;
            var eventTagListXml = personXml.Element("EventTagList");
            var startTimeXml = personXml.Element("StartTime");
            var endTimeXml = personXml.Element("EndTime");
            var daysPerPixel = double.Parse(personXml.Element("DaysPerPixel")?.Value);


            var parsedPerson = new Chart(contentSvg, personHash, eventTagListXml, startTimeXml, endTimeXml, daysPerPixel);

            return parsedPerson;
        }


        /// <summary>
        /// Unique data used to generate Hash
        /// </summary>
        public override int GetHashCode()
        {
            //get hash of all the fields & combine them
            var hash1 = PersonHash.GetHashCode();
            var hash2 = Tools.GetStringHashCode(StartTimeXml.ToString());
            var hash3 = Tools.GetStringHashCode(EndTimeXml.ToString());
            var hash4 = Tools.GetStringHashCode(EventTagListXml.ToString());

            //take out negative before returning
            var hashCode = Math.Abs(hash1 + hash2 + hash3 + hash4);
            return hashCode;
        }
    }
}
