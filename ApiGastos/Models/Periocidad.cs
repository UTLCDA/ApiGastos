using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace ApiGastos.Models
{
    public partial class Periocidad
    {
        public Periocidad()
        {
            Pagos = new HashSet<Pago>();
        }

        public int IdPeriocidad { get; set; }
        public string? Descripcion { get; set; }
        public int? TipoPeriocidad { get; set; }
        [JsonIgnore]
        public virtual ICollection<Pago> Pagos { get; set; }
    }
}
