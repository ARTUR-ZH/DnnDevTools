using System;
using System.IO;

namespace weweave.DnnDevTools.Util
{
    internal static class EmlFileParser
    {

        internal static CDO.Message ParseEmlFile(string file)
        {
            // Check file exists and is readable
            if (!File.Exists(file)) return null;
            try
            {
                File.Open(file, FileMode.Open, FileAccess.Read).Dispose();
            }
            catch (IOException)
            {
                return null;
            }

            CDO.Message msg = new CDO.MessageClass();
            ADODB.Stream stream = new ADODB.StreamClass();

            stream.Open(Type.Missing, ADODB.ConnectModeEnum.adModeUnknown, ADODB.StreamOpenOptionsEnum.adOpenStreamUnspecified, string.Empty, string.Empty);
            stream.LoadFromFile(file);
            stream.Flush();
            msg.DataSource.OpenObject(stream, "_Stream");
            msg.DataSource.Save();

            stream.Close();

            if (string.IsNullOrWhiteSpace(msg.Sender) &&
                string.IsNullOrWhiteSpace(msg.To) &&
                string.IsNullOrWhiteSpace(msg.From) &&
                string.IsNullOrWhiteSpace(msg.TextBody) &&
                string.IsNullOrWhiteSpace(msg.HTMLBody) &&
                string.IsNullOrWhiteSpace(msg.From)
            ) return null;

            return msg;
        }

    }
}
