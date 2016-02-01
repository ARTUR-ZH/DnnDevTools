using System;
using System.Collections.Generic;
using System.Linq;
using log4net.Core;
using weweave.DnnDevTools.Dto;
using weweave.DnnDevTools.SignalR;

namespace weweave.DnnDevTools.Service.Log
{
    internal class LogService : ServiceBase, ILogService
    {
        public LogService(IServiceLocator serviceLocator) : base(serviceLocator)
        {
        }

        public List<LogMessage> GetList(string start, int? skip, int? take, string search)
        {
            var logs = Log4NetAppender.LogMessageQueue.Select(e => e.Copy());
            var level = ServiceLocator.ConfigService.GetLogMessageTraceLevel();
            if (level != Level.All)
            {
                logs = logs.Where(e => e.LevelValue >= level.Value);
            }
            logs = logs.OrderByDescending(e => e.TimeStamp);
            if (!string.IsNullOrWhiteSpace(search))
                logs = logs.Where(e => 
                    e.Logger.IndexOf(search, StringComparison.OrdinalIgnoreCase) >= 0 ||
                    e.ClassName.IndexOf(search, StringComparison.OrdinalIgnoreCase) >= 0 ||
                    e.MethodName.IndexOf(search, StringComparison.OrdinalIgnoreCase) >= 0 ||
                    e.Message.IndexOf(search, StringComparison.OrdinalIgnoreCase) >= 0 ||
                    e.Level.IndexOf(search, StringComparison.OrdinalIgnoreCase) >= 0
                );
            if (!string.IsNullOrWhiteSpace(start))
                logs = logs.SkipWhile(e => e.Id != start);
            if (skip != null)
                logs = logs.Skip(skip.Value).ToList();
            if (take != null)
                logs = logs.Take(take.Value).ToList();

            return logs.ToList();
        }
    }
}
