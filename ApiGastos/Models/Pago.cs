using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace ApiGastos.Models
{
    public partial class Pago
    {
        public int IdPago { get; set; }
        public int? IdCategoriaGasto { get; set; }
        public decimal? Monto { get; set; }
        // monto total de la deuda - moto - casa - quitas
        public decimal? Balance { get; set; }
        public int? IdPeriocidad { get; set; }
        public int? IdMetodoPago { get; set; }
        public string? Descripcion { get; set; }
        public DateTime? Fecha { get; set; }
        public DateTime? FechaTentativa { get; set; }
        public int? Pagado { get; set; }
        [JsonIgnore]
        public virtual CategoriasGasto? objCategoriaGasto { get; set; }
        [JsonIgnore]
        public virtual Periocidad? objPeriocidad { get; set; }
    }
}
