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
    public class DnnEventController : ApiControllerBase
    {

        [HttpGet]
        public List<DnnEvent> List()
        {
            return List(null, null, null, null);
        }

        [HttpGet]
        public List<DnnEvent> List(string start, int? skip, int? take, string search)
        {
            return ServiceLocator.DnnEventService.GetList(start, skip, take, search);
        }

        [HttpGet]
        public HttpResponseMessage Detail(string id)
        {
            var dnnEvent = ServiceLocator.DnnEventService.GetList(null, null, null, null).FirstOrDefault(e => e.Id == id);
            return dnnEvent == null ?
                Request.CreateResponse(HttpStatusCode.NotFound) :
                Request.CreateResponse(HttpStatusCode.OK, dnnEvent);
        }
    }
}
