using System.Collections.Generic;

namespace weweave.DnnDevTools.Service.Mail
{
    public interface IMailService
    {

        List<Dto.Mail> GetList(string start, int? skip, int? take, string search);

    }
}
