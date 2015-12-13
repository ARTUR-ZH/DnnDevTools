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

                mails.Add(new Mail
                {
                    Id = Path.GetFileNameWithoutExtension(file),
                    Subject = mail.Subject,
                    Sender = mail.Sender,
                    SentOn = mail.SentOn,
                    To = mail.To
                });
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

            if (!File.Exists(file))
                return Request.CreateResponse(HttpStatusCode.NotFound);

            var mail = EmlFileParser.ParseEmlFile(file);
            if (mail == null)
                return Request.CreateResponse(HttpStatusCode.NotFound);

            return Request.CreateResponse(HttpStatusCode.OK, new MailDetail()
            {
                Body = string.IsNullOrWhiteSpace(mail.HTMLBody) ? mail.TextBody : mail.HTMLBody,
                BodyIsHtml = !string.IsNullOrWhiteSpace(mail.HTMLBody),
                Subject = mail.Subject,
                Sender = mail.Sender,
                SentOn = mail.SentOn,
                Id = id,
                To = mail.To
            });
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        public HttpResponseMessage SendMail()
        {
            DotNetNuke.Services.Mail.Mail.SendEmail("receiver@localhost", "sender@localhost", "EmailTest", "Hello world!");
            return Request.CreateResponse(HttpStatusCode.OK, "Mail sent!");
        }
    }
}
