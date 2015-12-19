using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using DotNetNuke.Web.Api;
using weweave.DnnDevTools.Dto;
using weweave.DnnDevTools.SignalR;

namespace weweave.DnnDevTools.Api.Controller
{

    [SuperUserAuthorize]
    [ValidateAntiForgeryToken]
    public class LogController : ApiControllerBase
    {
        [HttpGet]
        public List<LogMessage> List()
        {
            return List(null, null, null);
        }

        [HttpGet]
        public List<LogMessage> List(int? skip, int? take, string search)
        {
            IEnumerable<LogMessage> logs = Log4NetAppender.LogMessageQueue.Select(e => e.Copy()).OrderByDescending(e => e.TimeStamp);

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
