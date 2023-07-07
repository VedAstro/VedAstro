using System.Xml.Linq;
using VedAstro.Library;

namespace API
{
    /// <summary>
    /// A collection of general tools used by API
    /// </summary>
    public static partial class APITools
    {


        /// <summary>
        /// Gets main Saved Match Report list xml doc file
        /// </summary>
        /// <returns></returns>
        private static async Task<XDocument> GetSavedMatchReportListFile()
        {
            var savedMatchReportListXml = await Tools.GetXmlFileFromAzureStorage(SavedMatchReportList, Tools.BlobContainerName);

            return savedMatchReportListXml;
        }

    }
}
