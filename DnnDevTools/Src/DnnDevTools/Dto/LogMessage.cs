using System;
using log4net.Core;

namespace weweave.DnnDevTools.Dto
{
    public class LogMessage : IAction
    {
        public string Type => Globals.ActionTypeLogMessage;

        /// <summary>
        /// Unique Id
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Level of the log message (DEBUG, INFO, WARN, ERROR or FATAL)
        /// </summary>
        public string Level { get; set; }

        /// <summary>
        /// Log message (contain HTML tags like <b/> or <br/>
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// Time stamp when the log message was created
        /// </summary>
        public DateTime TimeStamp { get; set; }

        /// <summary>
        /// Full qualified name of class that logs the message (Example: DotNetNuke.Services.Scheduling.ScheduleHistoryItem)
        /// </summary>
        public string ClassName { get; set; }

        /// <summary>
        /// Name of the method that logs the message (Example: DotNetNuke.Services.Scheduling.ScheduleHistoryItem)
        /// </summary>
        public string MethodName { get; set; }

        public LogMessage() { }

        internal LogMessage(LoggingEvent loggingEvent)
        {
            Id = Guid.NewGuid().ToString();
            Level = loggingEvent.Level.DisplayName;
            Message = loggingEvent.RenderedMessage;
            TimeStamp = loggingEvent.TimeStamp;
            MethodName = loggingEvent.LocationInformation.MethodName;
            ClassName = loggingEvent.LocationInformation.ClassName;
        }

        internal LogMessage Copy()
        {
            return new LogMessage
            {
                Level  = Level,
                Id = Id,
                ClassName = ClassName,
                Message = Message,
                MethodName = MethodName,
                TimeStamp = TimeStamp
            };
        }
    }
}
