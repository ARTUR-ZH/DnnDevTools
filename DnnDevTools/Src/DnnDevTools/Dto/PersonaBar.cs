using System.Collections.Generic;

namespace weweave.DnnDevTools.Dto
{
    public class PersonaBar
    {

        public bool Enable { get; set; }

        public bool EnableMailCatch { get; set; }

        public bool EnableDnnEventTrace { get; set; }

        public string LogMessageTraceLevel { get; set; }

        public bool HostSmtpConfigured { get; set; }

        public Dictionary<string, string> Resources { get; set; }

    }
}
