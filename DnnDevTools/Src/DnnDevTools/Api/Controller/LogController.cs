using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
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

        [HttpGet]
        public HttpResponseMessage Detail(string id)
        {
            var logMessage = ServiceLocator.LogService.GetList(null, null, null, null).FirstOrDefault(e => e.Id == id);
            return logMessage == null ?
                Request.CreateResponse(HttpStatusCode.NotFound) :
                Request.CreateResponse(HttpStatusCode.OK, logMessage);
        }

    }
}
