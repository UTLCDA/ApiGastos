using System;
using System.Collections.Generic;

namespace ApiGastos.Models
{
    public partial class CategoriasIngreso
    {
        public CategoriasIngreso()
        {
            Ingresos = new HashSet<Ingreso>();
        }

        public int IdCategoriasIngreso { get; set; }
        public string? Nombre { get; set; }
        public string? Descripcion { get; set; }

        public virtual ICollection<Ingreso> Ingresos { get; set; }
    }
}
