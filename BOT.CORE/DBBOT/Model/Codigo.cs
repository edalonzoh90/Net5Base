using System;
using System.Collections.Generic;

#nullable disable

namespace BOT.CORE.DBBOT.Model
{
    public partial class Codigo
    {
        public int CodigoId { get; set; }
        public int Codigo1 { get; set; }
        public string CorreoElectronico { get; set; }
        public DateTime FechaEnvio { get; set; }
        public bool Validado { get; set; }
    }
}
