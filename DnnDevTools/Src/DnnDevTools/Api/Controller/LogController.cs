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
            return Log4NetAppender.LogMessageQueue.Select(e => e.Copy()).ToList();
        }
    }
}
