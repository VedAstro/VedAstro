using System.Threading.Tasks;

namespace VedAstro.Library
{
    /// <summary>
    /// Type that go into Open API calculator via URL
    /// FromXML used to get data back from URL format
    /// </summary>
    public interface IFromUrl
    {
        /// <summary>
        /// Set default to 2 places for enum, since can specify own
        /// other classes should override to their value
        /// also used to maintain name of variable to check
        /// </summary>
        public static int OpenAPILength = 2;

        public static abstract Task<dynamic> FromUrl(string url);
    }
}
