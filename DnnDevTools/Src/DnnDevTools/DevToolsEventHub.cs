using Microsoft.AspNet.SignalR;

namespace weweave.DnnDevTools
{
    public class DevToolsEventHub : Hub
    {
        public void Send(string message)
        {
            Clients.All.OnEvent("Ping", message);
        }

        public void OnMailSent()
        {
            Clients.All.OnEvent("mailSent", "{subject: 'sdasd'}");
        }

    }
}
