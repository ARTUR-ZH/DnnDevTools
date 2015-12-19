using CDO;

namespace weweave.DnnDevTools.Dto
{
    public class MailSentNotification : Mail, INotification
    {
        public string Type => "MailSent";

        internal MailSentNotification(string id, IMessage mail) : base(id, mail)
        {
        }

        public MailSentNotification() { }
    }
}
