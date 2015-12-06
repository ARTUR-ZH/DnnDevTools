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
        public HttpResponseMessage Enable(bool status)
        {
            ServiceLocatorFactory.Instance.ConfigService.SetEnable(status);
            return new HttpResponseMessage(HttpStatusCode.OK);
        }

        [HttpPut]
        public HttpResponseMessage EnableMailCatch(bool status)
        {
            ServiceLocatorFactory.Instance.ConfigService.SetEnableMailCatch(status);
            return new HttpResponseMessage(HttpStatusCode.OK);
        }

        [HttpGet]
        public HttpResponseMessage List()
        {
            return Request.CreateResponse(HttpStatusCode.OK, new Config()
            {
                Enable = ServiceLocatorFactory.Instance.ConfigService.GetEnable(),
                EnableMailCatch = ServiceLocatorFactory.Instance.ConfigService.GetEnableMailCatch()
            });
        }
    }
}
