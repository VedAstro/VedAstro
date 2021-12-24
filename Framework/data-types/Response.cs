using System;
using System.Collections.Generic;
using System.Text;

namespace Genso.Framework
{

    /// <summary>
    /// A simple encapsulation of the data received after a request from server
    /// Example after "add domain" request is sent to API from Client,
    /// the reponse data can be stored in this form
    /// </summary>
    public class Response
    {
        public Response(string status, string message)
        {
            Status = status;
            Message = message;
        }

        public string Status { get; }
        public string Message { get; }

        /// <summary>
        /// Returns true if the respnse failed
        /// </summary>
        public bool IsFail() => Status == "Fail";
        /// <summary>
        /// Returns true if the respnse passed
        /// </summary>
        public bool IsPass() => Status == "Pass";


    }
}
