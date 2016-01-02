using System.Collections.Generic;
using System.Web.Http;
using DotNetNuke.Web.Api;
using weweave.DnnDevTools.Dto;

namespace weweave.DnnDevTools.Api.Controller
{

    [IsAllowedAuthorize]
    [ValidateAntiForgeryToken]
    public class LogController : ApiControllerBase
    {
        [HttpGet]
        public List<LogMessage> List()
        {
            return List(null, null, null, null);
        }

        [HttpGet]
        public List<LogMessage> List(string start, int? skip, int? take, string search)
        {
            return ServiceLocator.LogService.GetList(start, skip, take, search);
        }
    }
}
