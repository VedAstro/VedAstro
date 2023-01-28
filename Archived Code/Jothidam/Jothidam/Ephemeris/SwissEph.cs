using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SwissEphNet
{
    /// <summary>
    /// Swiss Ephemeris C conversion
    /// </summary>
    public partial class SwissEph : IDisposable
    {

        #region Ctors & Dest

        /// <summary>
        /// Create a new context
        /// </summary>
        public SwissEph() {
            Sweph = new CPort.Sweph(this);
            SweJPL = new CPort.SweJPL(this);
            SwephLib = new CPort.SwephLib(this);
            SwemMoon = new CPort.SwemMoon(this);
            SwemPlan = new CPort.SwemPlan(this);
            SweDate = new CPort.SweDate(this);
            SweHouse = new CPort.SweHouse(this);
            SweCL = new CPort.SweCL(this);
            SweHel = new CPort.SweHel(this);
        }

        /// <summary>
        /// Internal release resources
        /// </summary>
        protected virtual void Dispose(bool disposing) {
            if (disposing) {
                swe_close();
            }
        }

        /// <summary>
        /// Release resources
        /// </summary>
        public void Dispose() {
            Dispose(true);
        }

        #endregion

        #region Trace

        /// <summary>
        /// Trace information
        /// </summary>
        public void Trace(String format, params object[] args) {
            var h = OnTrace;
            if (h != null) {
                String message = args != null ? C.sprintf(format, args) : format;
                h(this, new TraceEventArgs(message));
            }
        }

        #endregion

        #region File management

        /// <summary>
        /// Default encoding
        /// </summary>
        public static Encoding DefaultEncoding = Encoding.GetEncoding("Windows-1252");

        /// <summary>
        /// Load a file
        /// </summary>
        /// <param name="filename">File name</param>
        /// <returns>File loaded or null if file not found</returns>
        internal protected CFile LoadFile(String filename) {
            var h = OnLoadFile;
            if (h != null) {
                var e = new LoadFileEventArgs(filename);
                h(this, e);
                if (e.File == null) return null;
                return new CFile(e.File, DefaultEncoding);
            }
            return null;
        }

        #endregion

        #region Internals

        /// <summary>
        /// Sweph
        /// </summary>
        internal CPort.Sweph Sweph { get; private set; }

        /// <summary>
        /// SweJPL
        /// </summary>
        internal CPort.SweJPL SweJPL { get; private set; }

        /// <summary>
        /// SwephLib
        /// </summary>
        internal CPort.SwephLib SwephLib { get; private set; }

        /// <summary>
        /// SwemMoon
        /// </summary>
        internal CPort.SwemMoon SwemMoon { get; private set; }

        /// <summary>
        /// SwemPlan 
        /// </summary>
        internal CPort.SwemPlan SwemPlan { get; private set; }

        /// <summary>
        /// SweDate
        /// </summary>
        internal CPort.SweDate SweDate { get; private set; }

        /// <summary>
        /// SweHouse
        /// </summary>
        internal CPort.SweHouse SweHouse { get; private set; }

        /// <summary>
        /// SweCL
        /// </summary>
        internal CPort.SweCL SweCL { get; private set; }

        /// <summary>
        /// SweHel
        /// </summary>
        internal CPort.SweHel SweHel { get; private set; }

        #endregion

        #region Events

        /// <summary>
        /// Event raised when a new trace message is invoked
        /// </summary>
        public event EventHandler<TraceEventArgs> OnTrace;

        /// <summary>
        /// Event raised when loading a file is required
        /// </summary>
        public event EventHandler<LoadFileEventArgs> OnLoadFile;

        #endregion

    }

}
