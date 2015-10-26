using Microsoft.Owin;
using Owin;
using weweave.DnnDevTools;

[assembly: OwinStartup(typeof(Startup))]
namespace weweave.DnnDevTools
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            // Any connection or hub wire up and configuration should go here
            app.MapSignalR();

            MailPickupFileWatcher.Run();
        }
    }
}