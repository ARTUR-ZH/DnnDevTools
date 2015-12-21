using System;
using CDO;

namespace weweave.DnnDevTools.Dto
{
    public class Mail : IAction
    {
        public string Type => Globals.ActionTypeMail;

        public string Id { get; set; }

        public string Sender { get; set; }

        public string To { get; set; }

        public string Subject { get; set; }

        /// <summary>
        /// Time stamp when the mail was sent
        /// </summary>
        public DateTime TimeStamp { get; set; }

        public Mail() {}

        internal Mail(string id, IMessage mail)
        {
            Id = id;
            Subject = mail.Subject;
            Sender = mail.Sender;
            TimeStamp = mail.SentOn;
            To = mail.To;
        }
    }
}
