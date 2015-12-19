using log4net.Core;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace weweave.DnnDevTools.Test.Util.Log4NetUtil
{

    [TestClass]
    public class ParseLevelTest
    {

        [TestMethod]
        public void ParseLevelInfo()
        {
            Assert.AreEqual(Level.Info, DnnDevTools.Util.Log4NetUtil.ParseLevel("info"));
        }

        [TestMethod]
        public void ParseLevelInfoCaseInsensitive()
        {
            Assert.AreEqual(Level.Info, DnnDevTools.Util.Log4NetUtil.ParseLevel("iNfO"));
        }

        [TestMethod]
        public void ParseUnkownLevel()
        {
            Assert.IsNull(DnnDevTools.Util.Log4NetUtil.ParseLevel("UnkownLevel"));
        }
    }
}
