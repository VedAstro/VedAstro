using System.Xml.Linq;
using VedAstro.Library;

namespace API
{
    /// <summary>
    /// A collection of general tools used by API
    /// </summary>
    public static partial class APITools
    {
        public static async Task<XElement> FindSavedEventsChartXMLById(string inputChartId)
        {
            try
            {
                //get all match report, if event don't exit at all sub empty list
                var mainSavedXml = await Tools.GetXmlFileFromAzureStorage(APITools.SavedEventsChartListFile, Tools.BlobContainerName);
                var matchXmlList = mainSavedXml?.Root?.Elements() ?? new List<XElement>();

                //filter out only needed 
                var findSavedChartXmlById = matchXmlList.Where(delegate (XElement matchXml)
                {   //use chart id to find chart record
                    var thisId = MatchReport.FromXml(matchXml).Id;
                    return thisId == inputChartId;
                }).FirstOrDefault(MatchReport.Empty.ToXml());

                return findSavedChartXmlById;
            }
            catch (Exception e)
            {
                //if fail log it and return empty xelement
                APILogger.Error(e);
                return new XElement("Chart");
            }
        }


        public static async Task<XElement> FindSavedMatchReportXMLById(string inputChartId)
        {
            try
            {
                //get all match report, if event don't exit at all sub empty list
                var savedListXml = await Tools.GetXmlFileFromAzureStorage(APITools.SavedMatchReportList, Tools.BlobContainerName);
                var matchXmlList = savedListXml?.Root?.Elements() ?? new List<XElement>();

                //find match report that 
                return matchXmlList.Where(delegate (XElement savedMatchXml)
                {
                    var currentId = savedMatchXml.Element("Id")?.Value ?? ""; //all should have ID
                    return currentId == inputChartId;

                }).FirstOrDefault(MatchReport.Empty.ToXml());
            }
            catch (Exception e)
            {
                //if fail log it and return empty xelement
                APILogger.Error(e);
                return new XElement("Chart");
            }
        }

        public static XElement FindVisitorXMLById(XDocument visitorListXml, string visitorId)
        {
            try
            {
                var uniqueVisitorList = from visitorXml in visitorListXml.Root?.Elements()
                                        where visitorXml.Element("VisitorId")?.Value == visitorId
                                        select visitorXml;

                return uniqueVisitorList.FirstOrDefault();
            }
            catch (Exception e)
            {
                //if fail log it and return empty xelement
                 APILogger.Error(e); //log it
                return new XElement("Visitor");
            }
        }

        /// <summary>
        /// Given a id will return parsed person from main list
        /// Returns empty person if, no person found
        /// </summary>
        public static async Task<MatchReport> FindSavedMatchReportById(string matchId)
        {
            //get the raw data of person
            var foundMatchXml = await FindSavedMatchReportXMLById(matchId);

            if (foundMatchXml == null) { return MatchReport.Empty; }

            var foundPerson = MatchReport.FromXml(foundMatchXml);

            return foundPerson;
        }


    }
}