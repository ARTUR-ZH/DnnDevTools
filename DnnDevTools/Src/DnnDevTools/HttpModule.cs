using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Configuration;
using System.Net.Mail;
using System.Resources;
using System.Threading;
using System.Web;
using System.Web.Configuration;
using System.Web.Hosting;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using DotNetNuke.Entities.Portals;
using DotNetNuke.Framework;
using DotNetNuke.Web.Client;
using DotNetNuke.Web.Client.ClientResourceManagement;
using DotNetNuke.Web.Client.Providers;
using Newtonsoft.Json;
using weweave.DnnDevTools.SignalR;

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
                smtpSection.SpecifiedPickupDirectory.PickupDirectoryLocation = MailPickupFolderWatcher.Path;
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
                if (section.SpecifiedPickupDirectory.PickupDirectoryLocation != MailPickupFolderWatcher.Path)
                {
                    section.SpecifiedPickupDirectory.PickupDirectoryLocation = MailPickupFolderWatcher.Path;
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
            if (!PortalController.Instance.GetCurrentPortalSettings().UserInfo.IsSuperUser)
                return;

            var page = (Page)sender;

            ServicesFramework.Instance.RequestAjaxScriptSupport();

            var bodyControl = page?.FindControl("Body") as HtmlContainerControl;
            if (bodyControl == null) return;

            ClientResourceManager.RegisterScript(page, "~/desktopmodules/DnnDevTools/Scripts/jquery.signalR-2.2.0.js", FileOrder.Js.DefaultPriority, DnnFormBottomProvider.DefaultName);

            // Read Toolbar resources
            var rsxr = new ResXResourceReader(HttpContext.Current.Server.MapPath("~/DesktopModules/DnnDevTools/App_LocalResources/Toolbar.resx"));
            var resources = new Dictionary<string, string>();
            foreach (DictionaryEntry d in rsxr)
            {
                resources[d.Key.ToString()] = d.Value.ToString();
            }

            // Get enable mail catch config
            var enableMailCatch = ServiceLocatorFactory.Instance.ConfigService.GetEnableMailCatch();

            var toolbarHtml = $@"<script src=""{HostingEnvironment.ApplicationVirtualPath}/signalr/hubs""></script>";
            toolbarHtml += @"<script type=""text/javascript"">window.dnnMailDev={config:{enableMailCatch: " + enableMailCatch.ToString().ToLowerInvariant() + "},toolbar:{resources:" + JsonConvert.SerializeObject(resources) + "}}</script>";
            toolbarHtml += System.IO.File.ReadAllText(
                HttpContext.Current.Server.MapPath("~/DesktopModules/DnnDevTools/Toolbar.html")
            );

            var scriptControl = new LiteralControl { Text = toolbarHtml };
            bodyControl.Controls.Add(scriptControl);
        }
    }
}
