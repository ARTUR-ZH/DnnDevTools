using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web.Http;
using DotNetNuke.Web.Api;
using weweave.DnnDevTools.Dto;
using weweave.DnnDevTools.SignalR;
using weweave.DnnDevTools.Util;

namespace weweave.DnnDevTools.Api.Controller
{

    [SuperUserAuthorize]
    public class MailController : ApiControllerBase
    {
        [ValidateAntiForgeryToken]
        [HttpGet]
        public List<Mail> List()
        {
            var mails = new List<Mail>();

            var files = Directory.EnumerateFiles(MailPickupFolderWatcher.Path, "*.eml", SearchOption.TopDirectoryOnly);
            foreach (var file in files)
            {
                var mail = EmlFileParser.ParseEmlFile(file);
                if (mail == null) continue;

                mails.Add(new Mail(Path.GetFileNameWithoutExtension(file), mail));
            }

            return mails;
        }

        [ValidateAntiForgeryToken]
        [HttpDelete]
        public HttpResponseMessage Delete(string id)
        {
            var file = MailPickupFolderWatcher.Path + Path.DirectorySeparatorChar + id + ".eml";

            if (File.Exists(file)) File.Delete(file);

            return Request.CreateResponse(HttpStatusCode.OK);
        }

        [HttpGet]
        public HttpResponseMessage Download(string id)
        {
            var file = MailPickupFolderWatcher.Path + Path.DirectorySeparatorChar + id + ".eml";

            if (!File.Exists(file))
                return Request.CreateResponse(HttpStatusCode.NotFound);

            var result = Request.CreateResponse(HttpStatusCode.OK);
            result.Content = new StreamContent(new FileStream(file, FileMode.Open, FileAccess.Read)) {
                Headers =
                {
                    ContentDisposition = new ContentDispositionHeaderValue("attachment"),
                    ContentType = new MediaTypeHeaderValue("message/rfc822")
                }
            };
            result.Content.Headers.ContentDisposition.FileName = id + ".eml";
            return result;
        }

        [ValidateAntiForgeryToken]
        [HttpGet]
        public HttpResponseMessage Detail(string id)
        {
            var file = MailPickupFolderWatcher.Path + Path.DirectorySeparatorChar + id + ".eml";
            var mail = EmlFileParser.ParseEmlFile(file);
            return mail == null ? 
                Request.CreateResponse(HttpStatusCode.NotFound) : 
                Request.CreateResponse(HttpStatusCode.OK, new MailDetail(id, mail));
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        public HttpResponseMessage SendMail()
        {
            DotNetNuke.Services.Mail.Mail.SendEmail("sender@localhost", "receiver@localhost", "Test mail from DNN Dev Tools", "Hello world!");
            return Request.CreateResponse(HttpStatusCode.OK);
        }
    }
}
