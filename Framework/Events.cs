using System;
using System.Collections.Generic;
using System.Text;

namespace Genso.Framework
{
    /// <summary>
    /// event handlers for program related events
    /// </summary>
    public delegate void ProgramHandler();
    /// <summary>
    /// event handlers for programs events that process data
    /// </summary>
    public delegate void DataHandler(TransferData data);
    /// <summary>
    /// event handler for requests to server
    /// </summary>
    public delegate void RequestHandler(string message);
}
