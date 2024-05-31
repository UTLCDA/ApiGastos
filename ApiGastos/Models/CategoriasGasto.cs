using System;
using System.Collections.Generic;

namespace ApiGastos.Models
{
    public partial class CategoriasGasto
    {
        public CategoriasGasto()
        {
            Gastos = new HashSet<Gasto>();
            Pagos = new HashSet<Pago>();
        }

        public int IdCategoriasGasto { get; set; }
        public string? Nombre { get; set; }
        public string? Descripcion { get; set; }

        public virtual ICollection<Gasto> Gastos { get; set; }
        public virtual ICollection<Pago> Pagos { get; set; }
    }
}
