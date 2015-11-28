using Microsoft.Owin;
using Owin;
using weweave.DnnDevTools;
using weweave.DnnDevTools.SignalR;

[assembly: OwinStartup(typeof(Startup))]
namespace weweave.DnnDevTools
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            // Wire up SignalR
            app.MapSignalR();

            // Start mail pickup folder watcher
            MailPickupFolderWatcher.Run();
        }
    }
}