using System;
using System.Collections.Generic;
using System.Linq;
using weweave.DnnDevTools.Dto;
using weweave.DnnDevTools.SignalR;

namespace weweave.DnnDevTools.Service.Log
{
    internal class LogService : ServiceBase, ILogService
    {
        public LogService(IServiceLocator serviceLocator) : base(serviceLocator)
        {
        }

        public List<Dto.LogMessage> GetList(int? skip, int? take, string search)
        {
            IEnumerable<Dto.LogMessage> logs = Log4NetAppender.LogMessageQueue.Select(e => e.Copy()).OrderByDescending(e => e.TimeStamp);

            if (!string.IsNullOrWhiteSpace(search))
                logs = logs.Where(e => string.Concat(e.ClassName, e.MethodName, e.Message).IndexOf(search, StringComparison.OrdinalIgnoreCase) >= 0);
            if (skip != null)
                logs = logs.Skip(skip.Value).ToList();
            if (take != null)
                logs = logs.Take(take.Value).ToList();

            return logs.ToList();
        }
    }
}
