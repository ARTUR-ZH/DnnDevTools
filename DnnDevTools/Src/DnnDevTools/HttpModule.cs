using System;
using System.Configuration;
using System.Net.Configuration;
using System.Net.Mail;
using System.Threading;
using System.Web;
using System.Web.Configuration;
using System.Web.Hosting;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using DotNetNuke.Framework;
using DotNetNuke.Web.Client;
using DotNetNuke.Web.Client.ClientResourceManagement;
using DotNetNuke.Web.Client.Providers;

namespace weweave.DnnDevTools
{
    public class HttpModule : IHttpModule
    {
        public void Init(HttpApplication context)
        {
            context.BeginRequest += OnBeginRequest;
            context.PreRequestHandlerExecute += OnPreRequestHandlerExecute;
        }

        public void Dispose()
        {

        }

        private static void OnBeginRequest(object sender, EventArgs e)
        {
            var request = HttpContext.Current.Request;

            if (
                !"get".Equals(request.HttpMethod, StringComparison.OrdinalIgnoreCase) || 
                (request.AppRelativeCurrentExecutionFilePath != null && (
                    request.AppRelativeCurrentExecutionFilePath.StartsWith("~/signalr/", StringComparison.OrdinalIgnoreCase) ||
                    request.AppRelativeCurrentExecutionFilePath.StartsWith("~/WebResource.axd", StringComparison.OrdinalIgnoreCase))
                )
            ) return;

            var configuration = WebConfigurationManager.OpenWebConfiguration("~");

            var section = configuration.GetSection("system.net/mailSettings/smtp") as SmtpSection;

            var saveConfig = false;
            if (section == null)
            {
                var smtpSection = new SmtpSection();
                smtpSection.SpecifiedPickupDirectory.PickupDirectoryLocation = MailPickupFileWatcher.Path;
                smtpSection.DeliveryMethod = SmtpDeliveryMethod.SpecifiedPickupDirectory;
                configuration.Sections.Add("system.net/mailSettings/smtp", smtpSection);
                saveConfig = true;
            }
            else
            {
                if (section.DeliveryMethod != SmtpDeliveryMethod.SpecifiedPickupDirectory)
                {
                    section.DeliveryMethod = SmtpDeliveryMethod.SpecifiedPickupDirectory;
                    saveConfig = true;
                }
                if (section.SpecifiedPickupDirectory.PickupDirectoryLocation != MailPickupFileWatcher.Path)
                {
                    section.SpecifiedPickupDirectory.PickupDirectoryLocation = MailPickupFileWatcher.Path;
                    saveConfig = true;
                }
            }
            if (saveConfig)
            {
                new Thread(() =>
                {
                    configuration.Save();
                }).Start();
            }

        }

        private static void OnPreRequestHandlerExecute(object sender, EventArgs e)
        {
            var context = HttpContext.Current;

            var page = context?.Handler as CDefault;
            if (page == null) return;

            page.Init += OnPageInit;
        }

        private static void OnPageInit(object sender, EventArgs e)
        {
            var page = (Page)sender;

            ServicesFramework.Instance.RequestAjaxScriptSupport();

            var bodyControl = page?.FindControl("Body") as HtmlContainerControl;
            if (bodyControl == null) return;

            ClientResourceManager.RegisterScript(page, "~/desktopmodules/DnnDevTools/Scripts/jquery.signalR-2.2.0.js", FileOrder.Js.DefaultPriority, DnnFormBottomProvider.DefaultName);

            var toolbarHtml = $@"<script src=""{HostingEnvironment.ApplicationVirtualPath}/signalr/hubs""></script>";
            toolbarHtml += $@"<script type=""text/javascript"">window.dnnMailDev={{config:{{enableMailCatch: true}}}}</script>";
            toolbarHtml += System.IO.File.ReadAllText(
                HttpContext.Current.Server.MapPath("~/DesktopModules/DnnDevTools/Toolbar.html")
            );

            var scriptControl = new LiteralControl { Text = toolbarHtml };
            bodyControl.Controls.Add(scriptControl);
        }
    }
}
