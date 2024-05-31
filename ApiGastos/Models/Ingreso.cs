using System;
using System.Collections.Generic;

namespace ApiGastos.Models
{
    public partial class Ingreso
    {
        public int IdIngreso { get; set; }
        public int? IdCategoriaIngreso { get; set; }
        public string? Descripcion { get; set; }
        public decimal? Monto { get; set; }
        public DateTime? Fecha { get; set; }
        public int? IdUsuario { get; set; }

        public virtual CategoriasIngreso? IdCategoriaIngresoNavigation { get; set; }
    }
}
