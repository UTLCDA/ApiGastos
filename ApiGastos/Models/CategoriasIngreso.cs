using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

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
        [JsonIgnore]
        public virtual ICollection<Ingreso> Ingresos { get; set; }
    }
}
