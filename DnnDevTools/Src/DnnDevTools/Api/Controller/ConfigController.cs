using System.Net;
using System.Net.Http;
using System.Web.Http;
using DotNetNuke.Web.Api;
using weweave.DnnDevTools.Dto;

namespace weweave.DnnDevTools.Api.Controller
{
    [ValidateAntiForgeryToken]
    [SuperUserAuthorize]
    public class ConfigController : DnnApiController
    {

        [HttpPut]
        public HttpResponseMessage EnableMailCatch(bool enableMailCatch)
        {
            ServiceLocatorFactory.Instance.ConfigService.SetEnableMailCatch(enableMailCatch);
            return new HttpResponseMessage(HttpStatusCode.OK);
        }

        [HttpGet]
        public HttpResponseMessage List()
        {
            return Request.CreateResponse(HttpStatusCode.OK, new Config()
            {
                EnableMailCatch = ServiceLocatorFactory.Instance.ConfigService.GetEnableMailCatch()
            });
        }
    }
}
