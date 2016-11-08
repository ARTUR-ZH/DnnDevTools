using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Resources;
using System.Text;
using System.Threading;
using System.Web;
using System.Web.Hosting;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using DotNetNuke.Common.Utilities;
using DotNetNuke.Entities.Host;
using DotNetNuke.Entities.Portals;
using DotNetNuke.Framework;
using DotNetNuke.Web.Client;
using DotNetNuke.Web.Client.ClientResourceManagement;
using DotNetNuke.Web.Client.Providers;
using Newtonsoft.Json;
using weweave.DnnDevTools.Util;

namespace weweave.DnnDevTools
{
    public class HttpModule : IHttpModule
    {
        private class OverlayLocalizeResourceFilter : MemoryStream
        {
            private readonly Stream _outputStream;
            public OverlayLocalizeResourceFilter(Stream outputStream)
            {
                _outputStream = outputStream;
            }

            public override void Write(byte[] buffer, int offset, int count)
            {
                var contentInBuffer = Encoding.UTF8.GetString(buffer);

                // Get localized resource file (or default as fallback)
                string resxFile = null;
                if (HttpContext.Current.Request.Cookies["language"] != null)
                {
                    var resxFileLocalized = HttpContext.Current.Server.MapPath($"~/DesktopModules/DnnDevTools/App_LocalResources/Overlay.{HttpContext.Current.Request.Cookies["language"]?.Value}.resx");
                    if (File.Exists(resxFileLocalized)) resxFile = resxFileLocalized;

                }
                if (string.IsNullOrWhiteSpace(resxFile)) 
                    resxFile = HttpContext.Current.Server.MapPath($"~/DesktopModules/DnnDevTools/App_LocalResources/Overlay.resx");

                // Localize content
                var rsxr = new ResXResourceReader(resxFile);
                contentInBuffer = rsxr.Cast<DictionaryEntry>().Aggregate(contentInBuffer, (current, d) => current.Replace($"[res:{d.Key}]", d.Value.ToString()));
                _outputStream.Write(Encoding.UTF8.GetBytes(contentInBuffer), offset, Encoding.UTF8.GetByteCount(contentInBuffer));
                _outputStream.Flush();
            }
        }


        public void Init(HttpApplication context)
        {
            context.PreRequestHandlerExecute += OnPreRequestHandlerExecute;
            context.ReleaseRequestState += OnReleaseRequestState;
        }

        private static void OnReleaseRequestState(object sender, EventArgs e)
        {
            // Do nothing if module has been uninstalled
            if (!ModuleUtil.IsInstalled()) return;

            var request = HttpContext.Current.Request;

            if (!"~/DesktopModules/DnnDevTools/Overlay.aspx".Equals(request.AppRelativeCurrentExecutionFilePath, StringComparison.OrdinalIgnoreCase))
                return;

            HttpContext.Current.Response.Filter = new OverlayLocalizeResourceFilter(HttpContext.Current.Response.Filter);
        }

        public void Dispose()
        {

        }

        private static void OnPreRequestHandlerExecute(object sender, EventArgs e)
        {
            // Do nothing if module has been uninstalled
            if (!ModuleUtil.IsInstalled()) return;

            var context = HttpContext.Current;

            var page = context?.Handler as CDefault;
            if (page == null) return;

            page.Init += OnPageInit;
        }

        private static void OnPageInit(object sender, EventArgs e)
        {
            var portalSettings = PortalController.Instance.GetCurrentPortalSettings();

            // Skip if user is not allowed
            if (!ServiceLocatorFactory.Instance.ConfigService.IsAllowed(portalSettings.UserInfo))
                return;

            // Skip for DNN popups
            if (UrlUtils.InPopUp()) return;

            var enabled = ServiceLocatorFactory.Instance.ConfigService.GetEnable();

            // Skip if DNN Dev Tools is not enabled
            if (!enabled) return;

            var page = (Page)sender;
            var bodyControl = page?.FindControl("Body") as HtmlContainerControl;
            if (bodyControl == null) return;

            ServicesFramework.Instance.RequestAjaxScriptSupport();

            // Included shared dnn.js and dnn.css
            ClientResourceManager.RegisterScript(page, "~/desktopmodules/DnnDevTools/Scripts/dnn.js", FileOrder.Js.DefaultPriority, DnnFormBottomProvider.DefaultName);
            ClientResourceManager.RegisterStyleSheet(page, "~/desktopmodules/DnnDevTools/Styles/dnn.css");

            var basePath = string.IsNullOrWhiteSpace(HostingEnvironment.ApplicationVirtualPath) ? string.Empty : HostingEnvironment.ApplicationVirtualPath.TrimEnd('/');

            // Build JavaScript config
            var javaScriptConfig = new Dictionary<string, object>
            {
                ["enable"] = ServiceLocatorFactory.Instance.ConfigService.GetEnable(),
                ["enableMailCatch"] = ServiceLocatorFactory.Instance.ConfigService.GetEnableMailCatch(),
                ["enableDnnEventTrace"] = ServiceLocatorFactory.Instance.ConfigService.GetEnableDnnEventTrace(),
                ["logMessageTraceLevel"] = ServiceLocatorFactory.Instance.ConfigService.GetLogMessageTraceLevel().DisplayName,
                ["baseUrl"] = $"{basePath}/DesktopModules/DnnDevTools/",
                ["hostSmtpConfigured"] =  !string.IsNullOrWhiteSpace(Host.SMTPServer)
            };

            // Get element to inject scripts and HTML (needs to be injected before dnn.js)
            // Use top of body as fallback
            var injectIndex = 0;
            Control injectControl = bodyControl;
            var bottomPlaceHolderControl = page?.FindControl(DnnFormBottomProvider.DnnFormBottomPlaceHolderName);
            if (bottomPlaceHolderControl != null)
            {
                injectControl = bottomPlaceHolderControl.Parent;
                injectIndex = injectControl.Controls.IndexOf(bottomPlaceHolderControl);
            }

            // Inject DnnDevTool
            injectControl.Controls.AddAt(injectIndex, new LiteralControl { Text = $@"<script type=""text/javascript"">var weweave=weweave||{{}};weweave.dnnDevTools={JsonConvert.SerializeObject(javaScriptConfig)}</script>" });

            // Include SignalR scripts
            injectControl.Controls.AddAt(injectIndex, new LiteralControl { Text = $@"<script src=""{basePath}/desktopmodules/DnnDevTools/Scripts/jquery.signalR-2.2.0.js""></script><script src=""{basePath}/signalr/hubs""></script>" });

            // Get (static) toolbar html
            var toolbarHtml = File.ReadAllText(
                HttpContext.Current.Server.MapPath("~/DesktopModules/DnnDevTools/Toolbar.html")
                );

            // Replace toolbar resources
            var resxFile = HttpContext.Current.Server.MapPath($"~/DesktopModules/DnnDevTools/App_LocalResources/Toolbar.{Thread.CurrentThread.CurrentCulture}.resx");
            if (!File.Exists(resxFile))
                resxFile = HttpContext.Current.Server.MapPath("~/DesktopModules/DnnDevTools/App_LocalResources/Toolbar.resx");
            var rsxr = new ResXResourceReader(resxFile);
            var html = rsxr.Cast<DictionaryEntry>().Aggregate(toolbarHtml, (current, d) => current.Replace($"[res:{d.Key}]", d.Value.ToString()));

            // Inject toolbar
            var scriptControl = new LiteralControl { Text = html };
            injectControl.Controls.AddAt(injectIndex, scriptControl);
        }

    }

}
