using System;

namespace weweave.DnnDevTools.Dto
{
    public class Email
    {

        public string Subject { get; set; }
        public string Body { get; set; }

        public string From { get; set; }
        public string To { get; set; }
        public DateTime Date { get; set; }

    }
}
