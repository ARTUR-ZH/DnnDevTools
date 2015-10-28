using System.Net;
using System.Net.Http;
using System.Web.Http;
using DotNetNuke.Web.Api;

namespace weweave.DnnDevTools.Api
{
    public class MailController : DnnApiController
    {
        [AllowAnonymous]
        [HttpGet]
        public HttpResponseMessage HelloWorld()
        {
            return Request.CreateResponse(HttpStatusCode.OK, "Hello World!");
        }

        [AllowAnonymous]
        [HttpGet]
        public HttpResponseMessage SendMail()
        {
            DotNetNuke.Services.Mail.Mail.SendEmail("receiver@localhost", "sender@localhost", "EmailTest", "Hello world!");
            return Request.CreateResponse(HttpStatusCode.OK, "Mail sent!");
        }
    }
}
