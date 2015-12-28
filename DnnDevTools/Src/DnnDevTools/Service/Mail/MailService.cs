using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using weweave.DnnDevTools.SignalR;
using weweave.DnnDevTools.Util;

namespace weweave.DnnDevTools.Service.Mail 
{
    internal class MailService : ServiceBase, IMailService
    {
        public MailService(IServiceLocator serviceLocator) : base(serviceLocator)
        {
        }

        public List<Dto.Mail> GetList(string start, int? skip, int? take, string search)
        {
            var mails = new List<Dto.Mail>();

            var files = Directory.EnumerateFiles(MailPickupFolderWatcher.Path, "*.eml", SearchOption.TopDirectoryOnly);
            foreach (var file in files)
            {
                var message = EmlFileParser.ParseEmlFile(file);
                if (message == null) continue;

                mails.Add(new Dto.Mail(Path.GetFileNameWithoutExtension(file), message));
            }

            IEnumerable<Dto.Mail> result = mails.OrderByDescending(e => e.TimeStamp);

            if (!string.IsNullOrWhiteSpace(search))
                result = result.Where(e => string.Concat(e.Sender, e.Subject, e.To).IndexOf(search, StringComparison.OrdinalIgnoreCase) >= 0);
            if (!string.IsNullOrWhiteSpace(start)) result = result.SkipWhile(e => e.Id != start);
            if (skip != null) result = result.Skip(skip.Value);
            if (take != null) result = result.Take(take.Value);

            return result.ToList();
        }


    }
}
