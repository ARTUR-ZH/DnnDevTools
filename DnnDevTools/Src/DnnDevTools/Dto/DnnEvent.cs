using System;

namespace weweave.DnnDevTools.Dto
{
    public class DnnEvent
    {
        public string Id { get; set; }

        public string LogType { get; set; }

        public string Portal { get; set; }

        public DateTime TimeStamp { get; set; }
    }
}
