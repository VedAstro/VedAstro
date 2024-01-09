using System.Data;
using System.Xml.Linq;
using Newtonsoft.Json.Linq;

namespace VedAstro.Library
{
    /// <summary>
    /// convertible to JSON
    /// </summary>
    public interface IToDataTable
    {

        DataTable ToDataTable();

        
    }
}
