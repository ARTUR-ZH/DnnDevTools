using System;
using System.Collections.Concurrent;
using log4net.Appender;
using log4net.Core;
using Microsoft.AspNet.SignalR;
using weweave.DnnDevTools.Dto;
using weweave.DnnDevTools.Service;

namespace weweave.DnnDevTools.SignalR
{
    internal class Log4NetAppender : AppenderSkeleton
    {
        internal readonly static ConcurrentQueue<LogMessage> LoggingEvents = new ConcurrentQueue<LogMessage>();

        private IServiceLocator _serviceLocator;

        private IServiceLocator ServiceLocator => _serviceLocator ?? (_serviceLocator = new ServiceLocator());

        override protected void Append(LoggingEvent loggingEvent)
        {
            var level = ServiceLocator.ConfigService.GetLogMessageLevel();

            // Test log level
            if (level.Value > loggingEvent.Level.Value) return;

            // Create log event
            var logMessageEvent = new LogMessageEvent
            {
                Level = loggingEvent.Level.DisplayName,
                Message = loggingEvent.RenderedMessage,
                TimeStamp = loggingEvent.TimeStamp
            };

            // Queue logging event
            LoggingEvents.Enqueue(logMessageEvent);
            if (LoggingEvents.Count > 100)
            {
                LogMessage l;
                LoggingEvents.TryDequeue(out l);
            }

            // Send log event to clients
            GlobalHost.ConnectionManager.GetHubContext<DnnDevToolsEventHub>().Clients.All.OnEvent(logMessageEvent);
        }

        protected override bool RequiresLayout => false;

    }
    
}
