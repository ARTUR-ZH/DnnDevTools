using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using DotNetNuke.Common.Utilities;
using DotNetNuke.Web.Api;
using weweave.DnnDevTools.Dto;

namespace weweave.DnnDevTools.Api.Controller
{

    [SuperUserAuthorize]
    [ValidateAntiForgeryToken]
    public class DnnEventController : ApiControllerBase
    {
        [HttpGet]
        public List<DnnEvent> List()
        {
            var totalRecords = 0;
            var logs = DotNetNuke.Services.Log.EventLog.LogController.Instance.GetLogs(Null.NullInteger, Null.NullString, 100, 0, ref totalRecords);
            return logs.Select(e => new DnnEvent
            {
                LogType = e.LogTypeKey,
                Portal = e.LogPortalName,
                Id = e.LogGUID,
                TimeStamp = e.LogCreateDate,
                Username = e.LogUserName
            }).ToList();
        }
    }
}
