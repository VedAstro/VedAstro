using Microsoft.VisualStudio.TestTools.UnitTesting;
using VedAstro.Library;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VedAstro.Library.Tests
{
    [TestClass()]
    public class EventManagerTests
    {

        /// <summary>
        /// Debugger to make sure all scenarios for event generation from slices work
        /// </summary>
        [TestMethod()]
        public void EventSlicesToEventsTest()
        {
            //case 1 : 1 event in middle of 2 nulls


            //EventManager.EventSlicesToEvents(null, null, null, null, new[] { 1, 2});

            Assert.Fail();
        }
    }
}