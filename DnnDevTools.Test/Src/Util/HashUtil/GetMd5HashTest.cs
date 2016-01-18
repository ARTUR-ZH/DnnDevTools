using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace weweave.DnnDevTools.Test.Util.HashUtil
{

    [TestClass]
    public class ParseLevelTest
    {

        [TestMethod]
        public void ParseLevelInfo()
        {
            var md5 = DnnDevTools.Util.HashUtil.GetMd5Hash("hello world");
            Assert.IsNotNull(md5);
            Assert.AreEqual(32, md5.Length);
            Assert.AreEqual("5eb63bbbe01eeed093cb22bb8f5acdc3", md5.ToLowerInvariant());
        }

    }

}
