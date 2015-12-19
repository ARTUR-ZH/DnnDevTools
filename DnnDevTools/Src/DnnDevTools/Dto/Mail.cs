using System;
using CDO;

namespace weweave.DnnDevTools.Dto
{
    public class Mail
    {
        public string Id { get; set; }

        public string Sender { get; set; }

        public string To { get; set; }

        public string Subject { get; set; }

        public DateTime SentOn { get; set; }

        public Mail() {}

        internal Mail(string id, IMessage mail)
        {
            Id = id;
            Subject = mail.Subject;
            Sender = mail.Sender;
            SentOn = mail.SentOn;
            To = mail.To;
        }
    }
}
