using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure.Functions.Worker.Http;


namespace VedAstro.Library
{
    public static class CurrentCallerData
    {
        public static HttpRequestData originalHttpRequest { get; set; }


        ///// <summary>
        ///// given a new request will fill headers with details from original call if any
        ///// </summary>
        ///// <param name="httpRequestMessage"></param>
        //public static void AddOriginalCallerHeadersIfAny(HttpRequestMessage httpRequestMessage)
        //{
        //    if (originalHttpRequest != null)
        //    {
        //        foreach (var header in CurrentCallerData.originalHttpRequest.Headers)
        //        {
        //            httpRequestMessage.Headers.TryAddWithoutValidation(header.Key, header.Value.ToArray());
        //        }
        //    }
        //}

        //public static void AddOriginalCallerHeadersIfAny(HttpResponseData httpResponseData)
        //{
        //    if (originalHttpRequest != null)
        //    {
        //        foreach (var header in CurrentCallerData.originalHttpRequest.Headers)
        //        {
        //            httpResponseData.Headers.TryAddWithoutValidation(header.Key, header.Value.ToArray());
        //        }
        //    }
        //}

        //public static void AddOriginalCallerHeadersIfAny(HttpRequestMessage httpRequestMessage)
        //{
        //    try
        //    {
        //        if (originalHttpRequest != null && originalHttpRequest.Headers.Contains("Host"))
        //        {
        //            httpRequestMessage.Headers.TryAddWithoutValidation("Host", originalHttpRequest.Headers.GetValues("Host").ToArray());
        //            LibLogger.Debug("HOST OVERRIDE!!!");

        //        }

        //    }
        //    catch
        //    {
        //        LibLogger.Debug("HOST OVERRIDE FAIL!!!");

        //        // Handle all exceptions silently
        //    }
        //}

        public static void AddOriginalCallerHeadersIfAny(HttpResponseData httpResponseData)
        {
            try
            {
                if (originalHttpRequest != null && originalHttpRequest.Headers.Contains("Host"))
                {
                    LibLogger.Debug("HOST OVERRIDE!!!");
                    httpResponseData.Headers.TryAddWithoutValidation("Host", originalHttpRequest.Headers.GetValues("Host").ToArray());
                }
            }
            catch
            {
                LibLogger.Debug("HOST OVERRIDE FAIL!!!");
                // Handle all exceptions silently
            }
        }


    }
}
