using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;

namespace API
{

    public static class HttpRequestMessageExtensions
    {

        /// <summary>
        /// Gets public IP address of client sending the http request
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public static IPAddress GetCallerIp(this HttpRequestMessage request)
        {
            IPAddress result = null;
            if (request.Headers.TryGetValues("X-Forwarded-For", out IEnumerable<string> values))
            {
                var ipn = values.FirstOrDefault().Split(new char[] { ',' }).FirstOrDefault().Split(new char[] { ':' }).FirstOrDefault();
                IPAddress.TryParse(ipn, out result);
            }
            return result;
        }

    }
}
