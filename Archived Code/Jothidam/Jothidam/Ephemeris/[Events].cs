using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace SwissEphNet
{
    /// <summary>
    /// Arguments for trace event
    /// </summary>
    public class TraceEventArgs : EventArgs
    {
        /// <summary>
        /// Create new arguments
        /// </summary>
        public TraceEventArgs(String message) {
            this.Message = message;
        }

        /// <summary>
        /// Message
        /// </summary>
        public String Message { get; private set; }

    }

    /// <summary>
    /// Arguments for load file event
    /// </summary>
    public class LoadFileEventArgs : EventArgs
    {
        /// <summary>
        /// Create new arguments
        /// </summary>
        public LoadFileEventArgs(String fileName) {
            this.FileName = fileName;
            this.File = null;
        }

        /// <summary>
        /// File to load
        /// </summary>
        public String FileName { get; private set; }

        /// <summary>
        /// Stream
        /// </summary>
        public Stream File { get; set; }

    }
}
