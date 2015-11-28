using System;
using System.Collections;
using System.IO;
using System.Linq;
using System.Net.Configuration;
using System.Net.Mail;
using System.Resources;
using System.Text;
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
using weweave.DnnDevTools.SignalR;

namespace weweave.DnnDevTools
{
    public class LocalizeResourceFilter : MemoryStream
    {
        private readonly Stream _outputStream;
        public LocalizeResourceFilter(Stream outputStream)
        {
            _outputStream = outputStream;
        }

        public override void Write(byte[] buffer, int offset, int count)
        {
            var contentInBuffer = Encoding.UTF8.GetString(buffer);

            var rsxr = new ResXResourceReader(HttpContext.Current.Server.MapPath("~/DesktopModules/DnnDevTools/App_LocalResources/Overlay.aspx.resx"));

            contentInBuffer = rsxr.Cast<DictionaryEntry>().Aggregate(contentInBuffer, (current, d) => current.Replace($"[res:{d.Key}]", d.Value.ToString()));

            _outputStream.Write(Encoding.UTF8.GetBytes(contentInBuffer), offset, Encoding.UTF8.GetByteCount(contentInBuffer));
        }
    }

    public class HttpModule : IHttpModule
    {
        public void Init(HttpApplication context)
        {
            context.BeginRequest += OnBeginRequest;
            context.PreRequestHandlerExecute += OnPreRequestHandlerExecute;
            context.ReleaseRequestState += OnReleaseRequestState;
        }

        private static void OnReleaseRequestState(object sender, EventArgs e)
        {
            var request = HttpContext.Current.Request;

            if (!"~/DesktopModules/DnnDevTools/Overlay.aspx".Equals(request.AppRelativeCurrentExecutionFilePath, StringComparison.OrdinalIgnoreCase))
                return;

            HttpContext.Current.Response.Filter = new LocalizeResourceFilter(HttpContext.Current.Response.Filter);
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

            // Register SignalR
            ClientResourceManager.RegisterScript(page, "~/desktopmodules/DnnDevTools/Scripts/jquery.signalR-2.2.0.js", FileOrder.Js.DefaultPriority, DnnFormBottomProvider.DefaultName);

            // Get (static) toolbar html
            var toolbarHtml = File.ReadAllText(
                HttpContext.Current.Server.MapPath("~/DesktopModules/DnnDevTools/Toolbar.html")
            );

            // Replace toolbar resources
            var rsxr = new ResXResourceReader(HttpContext.Current.Server.MapPath("~/DesktopModules/DnnDevTools/App_LocalResources/Toolbar.html.resx"));
            toolbarHtml = rsxr.Cast<DictionaryEntry>().Aggregate(toolbarHtml, (current, d) => current.Replace($"[res:{d.Key}]", d.Value.ToString()));

            // Inject HTML into end of body
            var enableMailCatch = ServiceLocatorFactory.Instance.ConfigService.GetEnableMailCatch();
            var html = $@"<script src=""{HostingEnvironment.ApplicationVirtualPath}/signalr/hubs""></script>";
            html += @"<script type=""text/javascript"">window.dnnMailDev={enableMailCatch: " + enableMailCatch.ToString().ToLowerInvariant() + "}</script>";
            html += toolbarHtml;
            var scriptControl = new LiteralControl { Text = html };
            bodyControl.Controls.Add(scriptControl);
        }

    }

}
