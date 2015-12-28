using System.Collections.Generic;

namespace weweave.DnnDevTools.Service.DnnEvent
{
    public interface IDnnEventService
    {
        List<Dto.DnnEvent> GetList(string start, int? skip, int? take, string search);
    }
}
