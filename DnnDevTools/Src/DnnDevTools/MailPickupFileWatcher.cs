using System.IO;
using System.Web;
using Microsoft.AspNet.SignalR;
using weweave.DnnDevTools.Dto;
using weweave.DnnDevTools.Util;
using weweave.Src.DnnDevTools.Dto;

namespace weweave.DnnDevTools
{
    internal class MailPickupFileWatcher
    {

        private static string Path => HttpContext.Current.Server.MapPath("~/App_Data/MailPickup");

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
            var email = EmlFileParser.ParseEmlFile(e.FullPath);
            if (email == null) return;
            var emailSentEvent = new EmailSentEvent
            {
                Sender = email.Sender,
                To = email.To,
                Subject = email.Subject,
                SentOn = email.SentOn,
            };
            GlobalHost.ConnectionManager.GetHubContext<DevToolsEventHub>().Clients.All.OnEvent(emailSentEvent);
        }

    }
}
