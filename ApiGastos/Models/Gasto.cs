﻿using System;
using System.Collections.Generic;

namespace ApiGastos.Models
{
    public partial class Gasto
    {
        public int IdGasto { get; set; }
        public int? IdCategoriaGasto { get; set; }
        public string? Descripcion { get; set; }
        public decimal? Monto { get; set; }
        public DateTime? Fecha { get; set; }
        public int? IdUsuario { get; set; }

        public virtual CategoriasGasto? objCategoriaGasto { get; set; }
    }
}
