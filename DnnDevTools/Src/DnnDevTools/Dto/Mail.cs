using System;

namespace weweave.DnnDevTools.Dto
{
    public class Mail
    {
        public string Id { get; set; }

        public string Sender { get; set; }

        public string To { get; set; }

        public string Subject { get; set; }

        public DateTime SentOn { get; set; }
    }
}
