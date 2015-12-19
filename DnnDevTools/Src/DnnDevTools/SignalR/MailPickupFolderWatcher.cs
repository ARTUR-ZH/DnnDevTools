using System.IO;
using System.Web;
using Microsoft.AspNet.SignalR;
using weweave.DnnDevTools.Dto;
using weweave.DnnDevTools.Service;
using weweave.DnnDevTools.Util;

namespace weweave.DnnDevTools.SignalR
{
    internal class MailPickupFolderWatcher
    {

        internal static string Path => HttpContext.Current.Server.MapPath("~/App_Data/MailPickup");

        private IServiceLocator _serviceLocator;

        private IServiceLocator ServiceLocator => _serviceLocator ?? (_serviceLocator = new ServiceLocator());

        private static MailPickupFolderWatcher _instance;
        internal static MailPickupFolderWatcher Instance => _instance ?? (_instance = new MailPickupFolderWatcher());

        private MailPickupFolderWatcher() { }

        internal void Run()
        {
            if (!Directory.Exists(Path))
                Directory.CreateDirectory(Path);

            // Create a new FileSystemWatcher and set its properties
            var watcher = new FileSystemWatcher
            {
                Path = Path,
                NotifyFilter = NotifyFilters.LastWrite | NotifyFilters.FileName | NotifyFilters.CreationTime,
                Filter = "*.eml"
            };

            // Add event handlers
            watcher.Created += OnChanged;

            // Begin watching
            watcher.EnableRaisingEvents = true;
        }

        private void OnChanged(object source, FileSystemEventArgs e)
        {
            // Do nothing, if mail catch is not enabled
            if (!ServiceLocator.ConfigService.GetEnableMailCatch())
                return;
            
            // Try to parse mail
            var mail = EmlFileParser.ParseEmlFile(e.FullPath);
            if (mail == null) return;

            // Send mail notification to clients
            var emailSentEvent = new MailSentNotification(System.IO.Path.GetFileNameWithoutExtension(e.Name), mail);
            GlobalHost.ConnectionManager.GetHubContext<DnnDevToolsNotificationHub>().Clients.All.OnEvent(emailSentEvent);
        }

    }
}
