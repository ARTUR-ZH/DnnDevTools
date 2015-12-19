using System;

namespace weweave.DnnDevTools.Dto
{
    public class LogMessage
    {
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


    }
}
