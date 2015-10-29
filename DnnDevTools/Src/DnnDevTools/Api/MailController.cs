﻿using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using DotNetNuke.Web.Api;
using weweave.DnnDevTools.Dto;
using weweave.DnnDevTools.Util;

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
        public List<Mail> List()
        {
            var mails = new List<Mail>();

            var files = Directory.EnumerateFiles(MailPickupFileWatcher.Path, "*.eml", SearchOption.TopDirectoryOnly);
            foreach (var file in files)
            {
                var mail = EmlFileParser.ParseEmlFile(file);
                if (mail == null) continue;

                mails.Add(new Mail
                {
                    Id = System.IO.Path.GetFileNameWithoutExtension(file),
                    Subject = mail.Subject,
                    Sender = mail.Sender,
                    SentOn = mail.SentOn,
                    To = mail.To
                });
            }

            return mails;
        }

        [AllowAnonymous]
        [HttpGet]
        public HttpResponseMessage Delete(string id)
        {
            var file = MailPickupFileWatcher.Path + Path.DirectorySeparatorChar + id + ".eml";

            if (File.Exists(file)) File.Delete(file);

            return Request.CreateResponse(HttpStatusCode.OK);
        }


        [AllowAnonymous]
        [HttpGet]
        public HttpResponseMessage Detail(string id)
        {
            var file = MailPickupFileWatcher.Path + Path.DirectorySeparatorChar + id + ".eml";

            if (!File.Exists(file))
                return Request.CreateResponse(HttpStatusCode.NotFound);

            var mail = EmlFileParser.ParseEmlFile(file);
            if (mail == null)
                return Request.CreateResponse(HttpStatusCode.NotFound);

            return Request.CreateResponse(HttpStatusCode.OK, new MailDetail()
            {
                BodyHtml = mail.HTMLBody,
                Subject = mail.Subject,
                Sender = mail.Sender,
                SentOn = mail.SentOn,
                Id = id,
                To = mail.To
            });
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
