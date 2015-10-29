using System;

namespace weweave.DnnDevTools.Util
{
    internal static class EmlFileParser
    {

        internal static CDO.Message ParseEmlFile(string file)
        {
            CDO.Message msg = new CDO.MessageClass();
            ADODB.Stream stream = new ADODB.StreamClass();

            stream.Open(Type.Missing, ADODB.ConnectModeEnum.adModeUnknown, ADODB.StreamOpenOptionsEnum.adOpenStreamUnspecified, String.Empty, String.Empty);
            stream.LoadFromFile(file);
            stream.Flush();
            msg.DataSource.OpenObject(stream, "_Stream");
            msg.DataSource.Save();

            stream.Close();

            return msg;
        }

    }
}
