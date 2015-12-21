using System;
using DotNetNuke.Services.Log.EventLog;

namespace weweave.DnnDevTools.Dto
{
    public class DnnEvent : IAction
    {
        public string Type => Globals.ActionTypeDnnEvent;

        /// <summary>
        /// Unique Id
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Type of the event (Examples: USER_CREATED, LOGIN_SUCCESS, ...)
        /// </summary>
        public string LogType { get; set; }

        /// <summary>
        /// Name of the portal where the event happened (Examples: "My Website")
        /// </summary>
        public string Portal { get; set; }

        /// <summary>
        /// Time stamp when the event happened
        /// </summary>
        public DateTime TimeStamp { get; set; }

        /// <summary>
        /// Username of the user that triggered the event (NULL for system events) 
        /// </summary>
        public string Username { get; set; }

        /// <summary>
        /// Message of the event
        /// </summary>
        public string Message { get; set; }

        public DnnEvent() { }

        internal DnnEvent(LogInfo log)
        {
            LogType = log.LogTypeKey;
            Portal = log.LogPortalName;
            Id = log.LogGUID;
            TimeStamp = log.LogCreateDate;
            Username = log.LogUserName;
            Message = log.LogProperties.Summary;
        }
    }
}
