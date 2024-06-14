using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

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

        [JsonIgnore]
        public virtual CategoriasIngreso? objCategoriaIngreso { get; set; }
    }
}
