using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VedAstro.Library
{

    /// <summary>
    /// Name used by API data in JSON
    /// </summary>
    public class APIAttribute : Attribute
    {
        public APIAttribute(string name)
        {
            Name = name;
        }

        public string Name { get; set; }
    }

}
