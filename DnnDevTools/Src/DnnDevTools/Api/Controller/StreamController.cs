using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using DotNetNuke.Web.Api;
using weweave.DnnDevTools.Dto;

namespace weweave.DnnDevTools.Api.Controller
{
    [SuperUserAuthorize]
    [ValidateAntiForgeryToken]
    public class StreamController : ApiControllerBase
    {
        [HttpGet]
        public HttpResponseMessage Index([FromUri] string[] type)
        {
            return Index(type, null, null, null, null);
        }

        [HttpGet]
        public HttpResponseMessage Index([FromUri] string[] type, string start, int? skip, int? take, string search)
        {
            var actions = new List<IAction>();

            if (type.Length == 0 || type.Contains(Globals.ActionTypeMail, StringComparer.OrdinalIgnoreCase))
            {
                var mails = ServiceLocator.MailService.GetList(null, 0, take, search);
                actions.AddRange(mails);
            }

            if (type.Length == 0 || type.Contains(Globals.ActionTypeLogMessage, StringComparer.OrdinalIgnoreCase))
            {
                var logMessages = ServiceLocator.LogService.GetList(null, 0, take, search);
                actions.AddRange(logMessages);
            }

            if (type.Length == 0 || type.Contains(Globals.ActionTypeDnnEvent, StringComparer.OrdinalIgnoreCase))
            {
                var dnnEvents = ServiceLocator.DnnEventService.GetList(null, 0, take, search);
                actions.AddRange(dnnEvents);
            }

            IEnumerable<IAction> result = actions.OrderByDescending(e => e.TimeStamp);
            if (!string.IsNullOrWhiteSpace(start))
                result = result.SkipWhile(e => e.Id != start);
            if (skip != null)
                result = result.Skip(skip.Value);
            if (take != null)
                result = result.Take(take.Value);

            return ControllerContext.Request
                .CreateResponse(HttpStatusCode.OK, new { all = result });
        }

    }
}
