using log4net.Core;

namespace weweave.DnnDevTools.Dto
{
    public class LogMessageNotification : LogMessage, INotification
    {
        public string Type => "LogMessage";

        internal LogMessageNotification(LoggingEvent loggingEvent) : base(loggingEvent)
        {
        }

        public LogMessageNotification() { }
    }
}
