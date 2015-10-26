using System;
using log4net.Appender;
using log4net.Core;
using Microsoft.AspNet.SignalR;

namespace weweave.DnnDevTools
{
    /*public class Log4NetAppender : AppenderSkeleton
    {
        override protected void Append(LoggingEvent loggingEvent)
        {
            string title = string.Format("{0} {1}",
                loggingEvent.Level.DisplayName,
                loggingEvent.LoggerName);

            string message = string.Format(
                "{0}{1}{1}{2}{1}{1}(Yes to continue, No to debug)",
                RenderLoggingEvent(loggingEvent),
                Environment.NewLine,
                loggingEvent.LocationInformation.FullInfo);

            // Specify what is done when a file is renamed.
            GlobalHost.ConnectionManager.GetHubContext<DevToolsHub>().Clients.All.addMessage(
                $"Title: {title}, Message: {message}");

        }

        /// <summary>
        /// This appender requires a <see cref="Layout"/> to be set.
        /// </summary>
        protected override bool RequiresLayout => false;

    }*/
}
