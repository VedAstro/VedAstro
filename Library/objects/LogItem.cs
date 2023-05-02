using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace VedAstro.Library
{
    public readonly record struct LogItem(Time Time, string VisitorId, string UserId)
    {
        public static List<LogItem> FromXml(IEnumerable<XElement> xmlRecordList)
        {
            throw new NotImplementedException();
        }
    }

}
