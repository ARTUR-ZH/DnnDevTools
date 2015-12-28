using System;
using System.Collections.Generic;
using System.Linq;
using DotNetNuke.Common.Utilities;

namespace weweave.DnnDevTools.Service.DnnEvent
{
    internal class DnnEventService : ServiceBase, IDnnEventService
    {
        public DnnEventService(IServiceLocator serviceLocator) : base(serviceLocator)
        {
        }

        public List<Dto.DnnEvent> GetList(string start, int? skip, int? take, string search)
        {
            var totalRecords = 0;
            var logs = DotNetNuke.Services.Log.EventLog.LogController.Instance.GetLogs(Null.NullInteger, Null.NullString, 1000, 0, ref totalRecords);

            IEnumerable<Dto.DnnEvent> events = logs.Select(e => new Dto.DnnEvent(e)).OrderByDescending(e => e.TimeStamp);

            if (!string.IsNullOrWhiteSpace(search))
                events = events.Where(e => string.Concat(e.Message, e.Username).IndexOf(search, StringComparison.OrdinalIgnoreCase) >= 0);
            if (!string.IsNullOrWhiteSpace(start))
                events = events.SkipWhile(e => e.Id != start);
            if (skip != null)
                events = events.Skip(skip.Value);
            if (take != null)
                events = events.Take(take.Value);

            return events.ToList();
        }
    }
}
