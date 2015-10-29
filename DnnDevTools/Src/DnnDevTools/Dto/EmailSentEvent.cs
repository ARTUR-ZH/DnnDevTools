using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace weweave.Src.DnnDevTools.Dto
{
    public class EmailSentEvent : EventBase
    {
        
        public string Sender { get; set; }
        public string To { get; set; }

        public string Subject { get; set; }
        

        public DateTime SentOn { get; set; }

        public string Type => "MailSent";
    }

    public interface EventBase
    {
        string Type { get; }
    }
}
