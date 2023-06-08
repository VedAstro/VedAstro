using System;

namespace VedAstro.Library
{
    /// <summary>
    /// category to group the calcs, not all cacls needed,
    /// so we categorize to filter better
    /// </summary>
    public enum Category
    {
        StarsAboveMe,
        Astronomical,
        Astrology,
        All,
        Yoga
    }

    /// <summary>
    /// Name used by API data in JSON
    /// </summary>
    public class APIAttribute : Attribute
    {
        public Category Category { get; }

        /// <summary>
        /// nice and sweet name to id the Astro data
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// include alternate names and explanation for data
        /// </summary>
        public string Description { get; }

        public APIAttribute(string name, string description = "", Category category = Category.Astronomical)
        {
            Category = category;
            Description = description;
            Name = name;
        }

    }

}
