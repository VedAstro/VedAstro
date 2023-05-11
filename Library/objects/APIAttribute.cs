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
        /// <summary>
        /// nice and sweet name to id the Astro data
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// include alternate names and explanation for data
        /// </summary>
        public string Description { get; }

        public APIAttribute(string name, string description = "")
        {
            Description = description;
            Name = name;
        }

    }

}
