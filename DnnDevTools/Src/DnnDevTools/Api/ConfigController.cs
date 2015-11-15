using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using DotNetNuke.Web.Api;

namespace weweave.DnnDevTools.Api
{
    public class ConfigController : DnnApiController
    {

        [AllowAnonymous]
        [HttpGet]
        public HttpResponseMessage EnableMailCatch(bool enableMailCatch)
        {
            ServiceLocatorFactory.Instance.ConfigService.SetEnableMailCatch(enableMailCatch);
            return new HttpResponseMessage(HttpStatusCode.OK);
        }
    }
}
