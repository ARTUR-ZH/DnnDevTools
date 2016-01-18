using System.Security.Cryptography;
using System.Text;

namespace weweave.DnnDevTools.Util
{
    internal static class HashUtil
    {

        internal static string GetMd5Hash(string s)
        {
            if (string.IsNullOrEmpty(s)) return string.Empty;

            var md5 = MD5.Create();
            var inputBytes = Encoding.ASCII.GetBytes(s);
            var hash = md5.ComputeHash(inputBytes);
            var sb = new StringBuilder();
            foreach (var t in hash)
            {
                sb.Append(t.ToString("X2"));
            }
            return sb.ToString();
        }
    }
}
