using CDO;

namespace weweave.DnnDevTools.Dto
{
    public class MailDetail : Mail
    {
        public bool BodyIsHtml { get; set; }

        public string Body { get; set; }

        public MailDetail() { }

        internal MailDetail(string id, IMessage mail) : base(id, mail)
        {
            Body = string.IsNullOrWhiteSpace(mail.HTMLBody) ? mail.TextBody : mail.HTMLBody;
            BodyIsHtml = !string.IsNullOrWhiteSpace(mail.HTMLBody);
        }
    }
}
