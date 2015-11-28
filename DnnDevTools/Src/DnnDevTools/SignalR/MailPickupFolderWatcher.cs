using System.IO;
using System.Web;
using Microsoft.AspNet.SignalR;
using weweave.DnnDevTools.Dto;
using weweave.DnnDevTools.Util;

namespace weweave.DnnDevTools.SignalR
{
    internal class MailPickupFolderWatcher
    {

        internal static string Path => HttpContext.Current.Server.MapPath("~/App_Data/MailPickup");

        internal static void Run()
        {
            if (!Directory.Exists(Path))
                Directory.CreateDirectory(Path);

            // Create a new FileSystemWatcher and set its properties.
            var watcher = new FileSystemWatcher
            {
                Path = Path,
                NotifyFilter = NotifyFilters.LastWrite | NotifyFilters.FileName | NotifyFilters.CreationTime,
                Filter = "*.eml"
            };

            // Add event handlers.
            watcher.Created += OnChanged;

            // Begin watching.
            watcher.EnableRaisingEvents = true;
        }

        // Define the event handlers.
        private static void OnChanged(object source, FileSystemEventArgs e)
        {
            var mail = EmlFileParser.ParseEmlFile(e.FullPath);
            if (mail == null) return;
            var emailSentEvent = new MailSentEvent
            {
                Id = System.IO.Path.GetFileNameWithoutExtension(e.Name),
                Sender = mail.Sender,
                To = mail.To,
                Subject = mail.Subject,
                SentOn = mail.SentOn,
            };
            
            GlobalHost.ConnectionManager.GetHubContext<DnnDevToolsEventHub>().Clients.All.OnEvent(emailSentEvent);
        }

    }
}
