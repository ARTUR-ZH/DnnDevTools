using System;
using log4net.Core;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace weweave.DnnDevTools.Test.Dto.LogMessage
{

    [TestClass]
    public class ConstructorTest
    {

        [TestMethod]
        public void TestIdsAreEqual()
        {
            var loggingEvent = new LoggingEvent(new LoggingEventData());

            var logMessage1 = new DnnDevTools.Dto.LogMessage(loggingEvent);
            var logMessage2 = new DnnDevTools.Dto.LogMessage(loggingEvent);

            Assert.AreEqual(logMessage1.Id, logMessage2.Id);
        }

        [TestMethod]
        public void TestIdsAreDifferent()
        {
            var loggingEvent1 = new LoggingEvent(new LoggingEventData { TimeStamp = DateTime.Parse("2016-01-18T20:52:04.0255171+01:00")});
            var logMessage1 = new DnnDevTools.Dto.LogMessage(loggingEvent1);
            
            var loggingEvent2 = new LoggingEvent(new LoggingEventData { TimeStamp = DateTime.Parse("2016-01-18T20:52:04.024512+01:00") });
            var logMessage2 = new DnnDevTools.Dto.LogMessage(loggingEvent2);

            Assert.AreNotEqual(logMessage1.Id, logMessage2.Id);
        }


    }
}
