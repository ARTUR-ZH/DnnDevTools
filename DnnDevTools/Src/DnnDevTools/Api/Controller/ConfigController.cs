using System.Net;
using System.Net.Http;
using System.Web.Http;
using DotNetNuke.Web.Api;
using Newtonsoft.Json;

namespace weweave.DnnDevTools.Api.Controller
{
    [ValidateAntiForgeryToken]
    [SuperUserAuthorize]
    public class ConfigController : DnnApiController
    {

        [HttpPut]
        public HttpResponseMessage EnableMailCatch(bool enableMailCatch)
        {
            var status = ServiceLocatorFactory.Instance.ConfigService.SetEnableMailCatch(enableMailCatch);
            var result = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(JsonConvert.SerializeObject(status))
            };
            return result;
        }
    }
}
