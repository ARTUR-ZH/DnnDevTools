using System.Collections.Generic;
using weweave.DnnDevTools.Dto;

namespace weweave.DnnDevTools.Service.Log
{
    public interface ILogService
    {
        List<LogMessage> GetList(string start, int? skip, int? take, string search);    

    }
}
