using DotNetNuke.Services.Log.EventLog;

namespace weweave.DnnDevTools.Dto
{
    public class DnnDnnEventNotification : DnnEvent, INotification
    {
        public string Type => "Event";

        public DnnDnnEventNotification() { }

        internal DnnDnnEventNotification(LogInfo log) : base(log) { }
    }
}
