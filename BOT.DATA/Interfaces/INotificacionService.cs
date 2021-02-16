using BOT.DATA.Helper;
using BOT.DATA.Models;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace BOT.DATA.Interfaces
{
    public interface INotificacionService
    {
        Task<List<NotificacionModel>> List(NotificacionModel model, PagerModel pager);
    }
}
