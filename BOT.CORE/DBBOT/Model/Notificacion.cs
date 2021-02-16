using System;
using System.Collections.Generic;

#nullable disable

namespace BOT.CORE.DBBOT.Model
{
    public partial class Notificacion
    {
        public int NotificacionId { get; set; }
        public string Descripcion { get; set; }
        public string Asunto { get; set; }
        public string Cabecera { get; set; }
        public string Cuerpo { get; set; }
        public string Pie { get; set; }
    }
}
