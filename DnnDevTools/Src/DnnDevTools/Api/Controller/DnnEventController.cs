using System.Collections.Generic;
using System.Web.Http;
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
            return ServiceLocator.DnnEventService.GetList(skip, take, search);
        }

    }
}
