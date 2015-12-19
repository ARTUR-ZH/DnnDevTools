using weweave.DnnDevTools.SignalR;

namespace weweave.DnnDevTools.Dto
{
    public class LogMessageNotification : LogMessage, INotification
    {
        public string Type => "LogMessage";
    }
}
