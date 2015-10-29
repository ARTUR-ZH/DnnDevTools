namespace weweave.DnnDevTools.Dto
{
    public class MailSentEvent : Mail, IEvent
    {
        public string Type => "MailSent";
    }

    public interface IEvent
    {
        string Type { get; }
    }
}
