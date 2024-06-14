using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace ApiGastos.Models
{
    public partial class Role
    {
        public Role()
        {
            Usuarios = new HashSet<Usuario>();
        }

        public int IdRol { get; set; }
        public string? Descripcion { get; set; }
        [JsonIgnore]
        public virtual ICollection<Usuario> Usuarios { get; set; }
    }
}
