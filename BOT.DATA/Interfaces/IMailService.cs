using BOT.DATA.Models;
using System.Threading.Tasks;

namespace BOT.DATA.Interfaces
{
    public interface IMailService
    {
        Task Send(MailNotificationModel parameters);
    }
}
