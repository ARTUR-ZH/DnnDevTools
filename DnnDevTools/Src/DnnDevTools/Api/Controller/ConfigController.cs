using System.Net;
using System.Net.Http;
using System.Web.Http;
using DotNetNuke.Web.Api;

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
    }
}
