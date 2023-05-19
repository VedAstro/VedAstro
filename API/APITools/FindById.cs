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
                var mainSavedXml = await APITools.GetXmlFileFromAzureStorage(APITools.SavedEventsChartListFile, APITools.BlobContainerName);
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
                await APILogger.Error(e, null);
                return new XElement("Chart");
            }
        }


        public static async Task<XElement> FindSavedMatchReportXMLById(string inputChartId)
        {
            try
            {
                //get all match report, if event don't exit at all sub empty list
                var savedListXml = await APITools.GetXmlFileFromAzureStorage(APITools.SavedMatchReportList, APITools.BlobContainerName);
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
                await APILogger.Error(e);
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
                //todo log failure
                return new XElement("Visitor");
            }
        }

        /// <summary>
        /// Given a id will return parsed person from main list
        /// Returns empty person if, no person found
        /// </summary>
        public static async Task<Person> GetPersonById(string personId)
        {
            //get the raw data of person
            var foundPersonXml = await FindPersonXMLById(personId);

            if (foundPersonXml == null) { return Person.Empty; }

            var foundPerson = Person.FromXml(foundPersonXml);

            return foundPerson;
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

        /// <summary>
        /// XML Person
        /// Will look for a person in a given list
        /// returns null if no person found
        /// This a unique id representing the unique person record
        /// </summary>
        public static async Task<XElement?> FindPersonXMLById(string personIdToFind)
        {
            try
            {
                //get latest file from server
                //note how this creates & destroys per call to method
                //might cost little extra cycles but it's a functionality
                //to always get the latest list
                var personListXmlDoc = await GetPersonListFile();

                //list of person XMLs
                var personXmlList = personListXmlDoc?.Root?.Elements() ?? new List<XElement>();

                //do the finding (default empty)
                var foundPerson = personXmlList?.Where(MatchPersonId)?.First();

                //log it (should not occur all the time)
                if (foundPerson == null)
                {
                    await APILogger.Error($"No person found with ID : {personIdToFind}");
                    //return empty value so caller will know
                    foundPerson = null; 
                }

                return foundPerson;
            }
            catch (Exception e)
            {
                //if fail log it and return empty value so caller will know
                await APILogger.Error(e);
                return null;
            }

            //--------
            //do the finding, for id both case should match, but stored in upper case because looks nice
            //but user might pump in with mixed case, who knows, so compensate.
            bool MatchPersonId(XElement personXml)
            {
                if (personXml == null) { return false; }

                var inputPersonId = personXml?.Element("PersonId")?.Value ?? ""; //todo PersonId has to be just Id

                //lower case it before checking
                var isMatch = inputPersonId == personIdToFind; //hoisting alert

                return isMatch;
            }
        }

    }
}