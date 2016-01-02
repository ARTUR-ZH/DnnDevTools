using System.Security.Cryptography;
using System.Text;

namespace weweave.DnnDevTools.Util
{
    internal static class HashUtil
    {

        internal static string GetMd5Hash(string s)
        {
            if (string.IsNullOrEmpty(s)) return string.Empty;

            MD5 md5 = new MD5CryptoServiceProvider();
            var textToHash = Encoding.Default.GetBytes(s);
            var result = md5.ComputeHash(textToHash);
            return System.BitConverter.ToString(result);
        }
    }
}
