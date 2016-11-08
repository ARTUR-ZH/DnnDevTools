using DotNetNuke.Instrumentation;
using log4net;
using log4net.Core;
using Microsoft.Owin;
using Owin;
using weweave.DnnDevTools;
using weweave.DnnDevTools.SignalR;
using weweave.DnnDevTools.Util;
using ILog = DotNetNuke.Instrumentation.ILog;

[assembly: OwinStartup(typeof(Startup))]
namespace weweave.DnnDevTools
{

    public class Startup
    {
        private static readonly ILog Logger = LoggerSource.Instance.GetLogger(typeof(Startup));

        public void Configuration(IAppBuilder app)
        {
            // Skip initialization if module is not defined
            if (!ModuleUtil.IsInstalled()) return;

            Logger.Debug("Starting up DNN Dev Tools");

            // Wire up SignalR
            app.MapSignalR();

            // Configure Log4Net appender
            var root = ((log4net.Repository.Hierarchy.Hierarchy)LogManager.GetRepository()).Root;
            root.Level = Level.All;
            var attachable = (IAppenderAttachable) root;
            var appender = new Log4NetAppender();
            attachable.AddAppender(appender);

            // Start mail pickup folder watcher
            MailPickupFolderWatcher.Instance.Run();

            // Start event watcher
            DnnEventWatcher.Instance.Run();
        }
    }
}