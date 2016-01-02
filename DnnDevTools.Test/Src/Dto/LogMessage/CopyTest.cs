using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace weweave.DnnDevTools.Test.Dto.LogMessage
{

    [TestClass]
    public class CopyTest
    {
        [TestMethod]
        public void CopyLogMessage()
        {
            var logMessage = new DnnDevTools.Dto.LogMessage
            {
                Logger = "MyLogger"
            };

            var copiedLogMessage = logMessage.Copy();

            Assert.AreEqual(logMessage.Id, copiedLogMessage.Id);
            Assert.AreEqual(logMessage.Logger, copiedLogMessage.Logger);
        }

    }
}
