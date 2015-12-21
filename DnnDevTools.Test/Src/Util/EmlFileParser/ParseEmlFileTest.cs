using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace weweave.DnnDevTools.Test.Util.EmlFileParser
{

    [TestClass]
    public class ParseEmlFileTest
    {

        [TestMethod]
        public void ParseMail1()
        {
            var message = DnnDevTools.Util.EmlFileParser.ParseEmlFile("Data/Mail1.eml");

            Assert.IsNotNull(message);
            Assert.IsTrue(message.Sender.Contains("sender@localhost"));
            Assert.IsTrue(message.To.Contains("receiver@localhost"));
            Assert.IsTrue(message.TextBody.Contains("Hello world"));
        }

        [TestMethod]
        public void ParseMailFileNotExist()
        {
            var message = DnnDevTools.Util.EmlFileParser.ParseEmlFile("Data/MailDoesNotExist.eml");
            Assert.IsNull(message);
        }

        [TestMethod]
        public void ParseInvalidMail()
        {
            var message = DnnDevTools.Util.EmlFileParser.ParseEmlFile("Data/InvalidMail.eml");
            Assert.IsNull(message);
        }


    }
}
