using log4net.Core;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace weweave.DnnDevTools.Test.Dto.LogMessage
{

    [TestClass]
    public class ConstructorTest
    {

        [TestMethod]
        public void TestId()
        {
            var loggingEvent = new LoggingEvent(new LoggingEventData());

            var logMessage1 = new DnnDevTools.Dto.LogMessage(loggingEvent);
            var logMessage2 = new DnnDevTools.Dto.LogMessage(loggingEvent);

            Assert.AreEqual(logMessage1.Id, logMessage2.Id);
        }

    }
}
