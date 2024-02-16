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


        /// <summary>
        /// Method to detect event start and end times under multiple possibilities of input
        /// </summary>
        [TestMethod()]
        public void GetTrueRangesTest()
        {

            //TEST1
            bool[] array = new bool[] { false, true, true, false, true, true, true, false };

            List<(int Start, int End)> ranges = EventManager.GetTrueRanges(array);

            foreach (var range in ranges) { Console.WriteLine($"Start: {range.Start}, End: {range.End}"); }

            Assert.IsTrue(ranges.Count == 2);


            //TEST 2
            array = new bool[] { true, false, true, false, true, true, true, false };

            ranges = EventManager.GetTrueRanges(array);

            foreach (var range in ranges) { Console.WriteLine($"Start: {range.Start}, End: {range.End}"); }

            Assert.IsTrue(ranges.Count == 3);


            //TEST 3
            array = new bool[] { true, true, true, true, true, true, true, true };

            ranges = EventManager.GetTrueRanges(array);

            foreach (var range in ranges) { Console.WriteLine($"Start: {range.Start}, End: {range.End}"); }

            Assert.IsTrue(ranges.Count == 1);


            //TEST 4
            array = new bool[] { true, true, true, true, false, true, true, true };

            ranges = EventManager.GetTrueRanges(array);

            foreach (var range in ranges) { Console.WriteLine($"Start: {range.Start}, End: {range.End}"); }

            Assert.IsTrue(ranges.Count == 2);


            //TEST 5
            array = new bool[] { false, false, false, false, false, false, false, false };

            ranges = EventManager.GetTrueRanges(array);

            foreach (var range in ranges) { Console.WriteLine($"Start: {range.Start}, End: {range.End}"); }

            Assert.IsTrue(ranges.Count == 0);

        }


        /// <summary>
        /// Sample to generate raw events like Dasa, Gochara, Tarabala, etc...
        /// </summary>
        [TestMethod()]
        public void CalculateEventsTest()
        {
            //create needed data to generate events
            var startTime = Time.NowSystem(GeoLocation.Bangkok);
            var endTime = startTime.AddHours(1000);
            var johnDoe = new Person("Juliet", Time.StandardHoroscope(), Gender.Female);

            //# set how accurately the start & end time of each event is calculated
            //# exp: setting 1 hour, means given in a time range of 1 day, it will be checked 24 times 
            var precisionInHours = 1;

            //set what events to include
            var tagList = new List<EventTag>
            {
                EventTag.PD1,EventTag.PD2, EventTag.PD3, EventTag.PD4,EventTag.PD5, //dasa, antar, pratuantar, prana, sookshma
                EventTag.AshtakvargaGochara,
                EventTag.Gochara
            };

            //do calculation (heavy computation)
            var eventList = EventManager.CalculateEvents(precisionInHours,
                startTime,
                endTime, johnDoe, tagList);

            //# print out each event
            foreach (var _event in eventList) { Console.WriteLine(_event.ToString()); }

        }
    }
}