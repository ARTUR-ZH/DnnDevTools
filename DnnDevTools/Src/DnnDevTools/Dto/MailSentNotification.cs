namespace weweave.DnnDevTools.Dto
{
    public class MailSentNotification : Mail, INotification
    {
        public string Type => "MailSent";
    }
}
