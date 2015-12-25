using System;
using System.IO;
using System.Threading;
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
            watcher.Created += OnCreated;

            // Begin watching
            watcher.EnableRaisingEvents = true;
        }

        private void OnCreated(object source, FileSystemEventArgs e)
        {
            // Do nothing, if DNN Dev Tools or mail catch is not enabled
            if (!ServiceLocator.ConfigService.GetEnable() || !ServiceLocator.ConfigService.GetEnableMailCatch())
                return;

            // Start new background thread and wait until file is completely written and no longer locked
            new Thread(() =>
            {
                var start = DateTime.Now;
                var file = new FileInfo(e.FullPath);
                while (true)
                {
                    Thread.Sleep(100);

                    // Break if file is not released after max timeout
                    if (start - DateTime.Now > new TimeSpan(0, 0, 1)) break;

                    // Test if file is still locked
                    if (FileUtil.IsFileLocked(file)) continue;

                    // Try to parse mail
                    var message = EmlFileParser.ParseEmlFile(e.FullPath);
                    if (message == null) return;

                    // Send mail notification to clients
                    var mail = new Mail(System.IO.Path.GetFileNameWithoutExtension(e.Name), message);
                    GlobalHost.ConnectionManager.GetHubContext<DnnDevToolsNotificationHub>().Clients.All.OnEvent(mail);

                    break;
                }

            }).Start();

        }

    }
}
