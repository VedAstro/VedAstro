using System.Xml.Linq;

namespace VedAstro.Library
{
    /// <summary>
    /// Simple type to encapsulate only lite chart data
    /// used because all chart data encapsulated by Chart is
    /// too heavy for quick viewing
    /// </summary>
    public readonly struct ChartName 
    {
        public ChartName(string name, string chartId)
        {
            Name = name;
            ChartId = chartId;
        }

        public string Name { get; init; }
        public string ChartId { get; init; }



        /// <summary>
        /// Data coming in
        /// </summary>
        public static ChartName FromXml(XElement chartNameXml)
        {
            //EXAMPLE DATA
            //<Root>
            //    <ChartName>
            //        <Name>Viknesh - 4/1994 to 3/2114</Name>
            //        <ChartId>895183858</ChartId>
            //    </ChartName>
            //</Root>

            var name = chartNameXml.Element("Name")?.Value;
            var chartId = chartNameXml.Element("ChartId")?.Value;

            var parsedPerson = new ChartName(name, chartId);

            return parsedPerson;
        }


    }

}
