namespace weweave.DnnDevTools.Dto
{
    public class MailSentEvent : Mail, IEvent
    {
        public string Type => "MailSent";
    }
}
