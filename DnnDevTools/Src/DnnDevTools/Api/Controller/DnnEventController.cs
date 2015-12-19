using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using DotNetNuke.Common.Utilities;
using DotNetNuke.Web.Api;
using weweave.DnnDevTools.Dto;

namespace weweave.DnnDevTools.Api.Controller
{

    [SuperUserAuthorize]
    [ValidateAntiForgeryToken]
    public class DnnEventController : ApiControllerBase
    {

        [HttpGet]
        public List<DnnEvent> List()
        {
            return List(null, null, null);
        }

        [HttpGet]
        public List<DnnEvent> List(int? skip, int? take, string search)
        {
            var totalRecords = 0;
            var logs = DotNetNuke.Services.Log.EventLog.LogController.Instance.GetLogs(Null.NullInteger, Null.NullString, 1000, 0, ref totalRecords);

            IEnumerable<DnnEvent> events = logs.Select(e => new DnnEvent(e)).OrderByDescending(e => e.TimeStamp);

            if (!string.IsNullOrWhiteSpace(search))
                events = events.Where(e => string.Concat(e.Message, e.Username).IndexOf(search, StringComparison.OrdinalIgnoreCase) >= 0);
            if (skip != null)
                events = events.Skip(skip.Value);
            if (take != null)
                events = events.Take(take.Value);

            return events.ToList();
        }

    }
}
