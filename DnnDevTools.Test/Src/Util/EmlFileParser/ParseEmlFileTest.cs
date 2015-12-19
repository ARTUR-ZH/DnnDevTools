using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace weweave.DnnDevTools.Test.Util.EmlFileParser
{

    [TestClass]
    public class ParseEmlFileTest
    {

        [TestMethod]
        public void ParseMail1()
        {
            var mail = DnnDevTools.Util.EmlFileParser.ParseEmlFile("Data/Mail1.eml");

            Assert.IsNotNull(mail);
            Assert.IsTrue(mail.Sender.Contains("sender@localhost"));
            Assert.IsTrue(mail.To.Contains("receiver@localhost"));
            Assert.IsTrue(mail.TextBody.Contains("Hello world"));
        }

        [TestMethod]
        public void ParseMailFileNotExist()
        {
            var mail = DnnDevTools.Util.EmlFileParser.ParseEmlFile("Data/MailDoesNotExist.eml");
            Assert.IsNull(mail);
        }

        [TestMethod]
        public void ParseInvalidMail()
        {
            var mail = DnnDevTools.Util.EmlFileParser.ParseEmlFile("Data/InvalidMail.eml");
            Assert.IsNull(mail);
        }


    }
}
