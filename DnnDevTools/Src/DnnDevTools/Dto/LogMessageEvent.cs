using weweave.DnnDevTools.SignalR;

namespace weweave.DnnDevTools.Dto
{
    public class LogMessageEvent : LogMessage, IEvent
    {
        public string Type => "LogMessage";
    }
}
