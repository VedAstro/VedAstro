using System.Xml.Linq;

namespace API
{
    /// <summary>
    /// A collection of general tools used by API
    /// </summary>
    public static partial class APITools
    {

        /// <summary>
        /// Gets main person list xml doc file
        /// </summary>
        /// <returns></returns>
        private static async Task<XDocument> GetPersonListFile()
        {
            var personListXml = await GetXmlFileFromAzureStorage(PersonListFile, BlobContainerName);

            return personListXml;
        }

        /// <summary>
        /// Gets main Saved Match Report list xml doc file
        /// </summary>
        /// <returns></returns>
        private static async Task<XDocument> GetSavedMatchReportListFile()
        {
            var savedMatchReportListXml = await GetXmlFileFromAzureStorage(SavedMatchReportList, BlobContainerName);

            return savedMatchReportListXml;
        }

    }
}
