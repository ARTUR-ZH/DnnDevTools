using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Resources;
using System.Web;
using System.Web.Http;
using DotNetNuke.Entities.Host;
using DotNetNuke.Web.Api;
using weweave.DnnDevTools.Dto;

namespace weweave.DnnDevTools.Api.Controller
{
    [ValidateAntiForgeryToken]
    [SuperUserAuthorize]
    public class PersonaBarController : ApiControllerBase
    {

        [HttpGet]
        public PersonaBar Index(string culture)
        {

            // Get localized resource file (or default as fallback)
            string resxFile = null;
            var resxFileLocalized = HttpContext.Current.Server.MapPath($"~/DesktopModules/DnnDevTools/App_LocalResources/PersonaBar.{culture}.resx");
            if (File.Exists(resxFileLocalized)) resxFile = resxFileLocalized;
            if (string.IsNullOrWhiteSpace(resxFile))
                resxFile = HttpContext.Current.Server.MapPath("~/DesktopModules/DnnDevTools/App_LocalResources/PersonaBar.resx");
            var rsxr = new ResXResourceReader(resxFile);
            var resources = rsxr.Cast<DictionaryEntry>().ToDictionary(x => x.Key.ToString(), x => x.Value.ToString());

            return new PersonaBar
            {
                Enable = ServiceLocatorFactory.Instance.ConfigService.GetEnable(),
                EnableMailCatch = ServiceLocatorFactory.Instance.ConfigService.GetEnableMailCatch(),
                EnableDnnEventTrace = ServiceLocatorFactory.Instance.ConfigService.GetEnableDnnEventTrace(),
                LogMessageTraceLevel = ServiceLocatorFactory.Instance.ConfigService.GetLogMessageTraceLevel().DisplayName,
                HostSmtpConfigured = !string.IsNullOrWhiteSpace(Host.SMTPServer),
                Resources = resources
            };
        }

    }
}
