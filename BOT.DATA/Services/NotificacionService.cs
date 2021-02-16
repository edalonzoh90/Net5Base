using BOT.CORE.DBBOT.Context;
using BOT.CORE.DBBOT.Model;
using BOT.DATA.Helper;
using BOT.DATA.Interfaces;
using BOT.DATA.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BOT.DATA.Services
{
    public class NotificacionService : INotificacionService
    {
        private readonly BOTContext _context;

        public NotificacionService(BOTContext context)
        {
            _context = context;
        }

        public async Task<List<NotificacionModel>> List(NotificacionModel model, PagerModel pager)
        {
            IQueryable<Notificacion> query = _context.Notificacions;
            DynamicPager pagerLogSync = new DynamicPager();

            var data = (from a in query
                              select a
                              ).AsQueryable().ToList();

            if(data != null)
            {
                if (pager.Limit == 0)
                    pager.Limit = int.MaxValue;

                var dataPaginated = data.Skip(pager.Offset).Take(pager.Limit).ToList();

                var result = dataPaginated.Select(x => new NotificacionModel
                {
                    NotificacionId = x.NotificacionId,
                    Descripcion = x.Descripcion
                }).ToList();

                if (string.IsNullOrEmpty(pager.sortDir))
                    pager.sortDir = "ASC";
                if (string.IsNullOrEmpty(pager.sortColumn))
                    pager.sortColumn = "NotificacionId";

                var dataOrdered = pagerLogSync.DynamicSorting(result, pager.sortColumn, pager.sortDir).ToList();
                pager.TotalRows = data.Count();

                return dataOrdered;
            }

            return null;
        }
    }
}
