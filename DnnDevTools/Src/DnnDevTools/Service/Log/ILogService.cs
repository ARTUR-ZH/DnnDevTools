using System.Collections.Generic;
using weweave.DnnDevTools.Dto;

namespace weweave.DnnDevTools.Service.Log
{
    public interface ILogService
    {
        List<Dto.LogMessage> GetList(int? skip, int? take, string search);    

    }
}
