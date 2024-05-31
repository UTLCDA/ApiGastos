using System;
using System.Collections.Generic;

namespace ApiGastos.Models
{
    public partial class Pago
    {
        public int IdPago { get; set; }
        public int? IdCategoriaGasto { get; set; }
        public decimal? Monto { get; set; }
        public int? IdPeriocidad { get; set; }
        public string? Descripcion { get; set; }
        public DateTime? Fecha { get; set; }
        public int? Pagado { get; set; }

        public virtual CategoriasGasto? IdCategoriaGastoNavigation { get; set; }
        public virtual Periocidad? IdPeriocidadNavigation { get; set; }
    }
}
