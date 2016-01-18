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
        private const int QueueSize = 100000;

        internal readonly static ConcurrentQueue<LogMessage> LogMessageQueue = new ConcurrentQueue<LogMessage>();

        private IServiceLocator _serviceLocator;

        private IServiceLocator ServiceLocator => _serviceLocator ?? (_serviceLocator = new ServiceLocator());

        override protected void Append(LoggingEvent loggingEvent)
        {
            var level = ServiceLocator.ConfigService.GetLogMessageTraceLevel();

            // Test log level
            if (level.Value > loggingEvent.Level.Value) return;

            // Create log event
            var logMessageEvent = new LogMessage(loggingEvent);

            // Queue logging event
            LogMessageQueue.Enqueue(logMessageEvent);
            if (LogMessageQueue.Count > QueueSize)
            {
                LogMessage l;
                LogMessageQueue.TryDequeue(out l);
            }

            // Send log event to clients
            GlobalHost.ConnectionManager.GetHubContext<DnnDevToolsNotificationHub>().Clients.All.OnEvent(logMessageEvent);
        }

        protected override bool RequiresLayout => false;

    }
    
}
