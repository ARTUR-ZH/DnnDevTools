using System.Net;
using System.Net.Http;
using System.Web.Http;
using DotNetNuke.Web.Api;
using weweave.DnnDevTools.Dto;
using weweave.DnnDevTools.Util;

namespace weweave.DnnDevTools.Api.Controller
{
    [ValidateAntiForgeryToken]
    [SuperUserAuthorize]
    public class ConfigController : ApiControllerBase
    {

        [HttpPut]
        public HttpResponseMessage Enable(bool status)
        {
            ServiceLocator.ConfigService.SetEnable(status);
            return new HttpResponseMessage(HttpStatusCode.OK);
        }

        [HttpPut]
        public HttpResponseMessage EnableMailCatch(bool status)
        {
            if (status && !ServiceLocator.ConfigService.GetEnable())
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, "NOT_ENABLED");
            }

            ServiceLocator.ConfigService.SetEnableMailCatch(status);
            return new HttpResponseMessage(HttpStatusCode.OK);
        }

        [HttpPut]
        public HttpResponseMessage SetLogMessageLevel(string level)
        {
            var log4NetLevel = Log4NetUtil.ParseLevel(level);

            if (log4NetLevel == null)
                return Request.CreateResponse(HttpStatusCode.BadRequest);


            ServiceLocator.ConfigService.SetLogMessageLevel(log4NetLevel);

            return new HttpResponseMessage(HttpStatusCode.OK);
        }

        [HttpGet]
        public HttpResponseMessage List()
        {
            return Request.CreateResponse(HttpStatusCode.OK, new Config
            {
                Enable = ServiceLocator.ConfigService.GetEnable(),
                EnableMailCatch = ServiceLocator.ConfigService.GetEnableMailCatch(),
                LogMessageLevel = ServiceLocator.ConfigService.GetLogMessageLevel().DisplayName
            });
        }
    }
}
