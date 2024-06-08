﻿using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace ApiGastos.Models
{
    public partial class Usuario
    {
        public int IdUsuario { get; set; }
        public string? NombreUsuario { get; set; }
        public string? Contrasena { get; set; }
        public bool? Activo { get; set; }
        public int? IdRol { get; set; }

        [JsonIgnore]
        public virtual Role? objRoles { get; set; }
    }
}
