using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Nintek.Core.Gmail
{
    public interface IMailSender
    {
        Task Send(string fromAddress, string toRecipients, string subject, string body = "", bool isBodyHtml = false);
    }
}
