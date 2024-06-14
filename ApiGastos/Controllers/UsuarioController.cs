using ApiGastos.Models;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ApiGastos.Controllers
{
    [EnableCors("ReglasCors")]
    [Route("api/[controller]")]
    [ApiController]
    public class UsuarioController : ControllerBase
    {
        public readonly bdGastosContext _bdGastosContext;

        public UsuarioController(bdGastosContext bdGastosContext)
        {
            _bdGastosContext = bdGastosContext;
        }

        [HttpGet]
        [Route("Lista/Usuarios")]
        public IActionResult ListarUsuarios()
        {
            // agregar condicion para que solo se pueda ver la lista por usuarios de tipo 1 admin
            List<Usuario> listaUsuarios = new List<Usuario>();
            try
            {
                listaUsuarios = _bdGastosContext.Usuarios.ToList();
                if (listaUsuarios == null)
                {
                    return BadRequest("El valor de los usuarios esta vacio.");
                }
                return StatusCode(StatusCodes.Status200OK, new { mensaje = "ok", response = listaUsuarios });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status400BadRequest, new { mensaje = ex.Message });
            }
        }

        [HttpGet]
        [Route("Obtener/{idUsuario:int}")]
        public IActionResult ObtenerUsuario(int idUsuario)
        {
            Usuario usuario = new Usuario();
            usuario = _bdGastosContext.Usuarios.Find(idUsuario);
            if (usuario == null)
            {
                return BadRequest("Usuario no encontrado.");
            }
            try
            {
                usuario = _bdGastosContext.Usuarios.Where(g => g.IdUsuario == idUsuario).FirstOrDefault();
                if (usuario == null)
                {
                    return BadRequest("Usuario no encontrado de acuerdo a los parametros de busqueda.");
                }
                return StatusCode(StatusCodes.Status200OK, new { mensaje = "ok", response = usuario });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status400BadRequest, new { mensaje = ex.Message });
            }
        }

        [HttpPost]
        [Route("GuardarUsuario")]
        public IActionResult GuardarUsuario([FromBody] Usuario solicitudUsuario)
        {
            try
            {
                _bdGastosContext.Usuarios.Add(solicitudUsuario);
                _bdGastosContext.SaveChanges();
                return StatusCode(StatusCodes.Status200OK, new { mensaje = "ok", response = solicitudUsuario });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status400BadRequest, new { mensaje = ex.Message });
            }
        }

        [HttpPut]
        [Route("EditarUsuario/")]
        public IActionResult EditarUsuario([FromBody] Usuario solicitudUsuario)
        {
            // en la solicitud va traer el idRol ya sea por variable de sesion o por parametro.
            // el id rol siempre va ser requerido 
            Usuario usuario = _bdGastosContext.Usuarios.Find(solicitudUsuario.IdUsuario);
            if (usuario == null)
            {
                return BadRequest($"El id del usuario : {solicitudUsuario.IdUsuario} no fue encontrado ");
            }
            try
            {
                usuario.NombreUsuario = solicitudUsuario.NombreUsuario is null ? usuario.NombreUsuario : solicitudUsuario.NombreUsuario;
                // se creara otro servicio para modificar la contraseña
                // agregar validacion de contraseña a mas de 8 caracteres y sugerir contraseñas
                usuario.Contrasena = solicitudUsuario.Contrasena is null ? usuario.Contrasena : solicitudUsuario.Contrasena;
                // se agrerara la condicion que el usuario : 1  solo puede gestionar los roles
                // debe de agregar otro campo para verificar el rol que se manda o actualizar el rol por otro metodo
                if (solicitudUsuario.IdRol == 1)
                {
                    usuario.IdRol = solicitudUsuario.IdRol;
                }
                else if (solicitudUsuario.IdRol == null)
                {
                    usuario.IdRol = usuario.IdRol;
                }
                else
                {
                    return StatusCode(StatusCodes.Status200OK, new { mensaje = "No tienes privilegios de tipo Admin para realizar este ajuste." });
                }

                _bdGastosContext.Update(usuario);
                _bdGastosContext.SaveChanges();

                return StatusCode(StatusCodes.Status200OK, new { mensaje = "ok" });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status400BadRequest, new { mensaje = ex.Message });
            }
        }

        [HttpPut]
        [Route("Baja/")]
        public IActionResult DarDeBajaUsuario([FromBody] Usuario solicitudUsuario)
        {
            // hacer una condicion para que mediante una solicitud solo pida el id usuario y activo
            Usuario usuario = _bdGastosContext.Usuarios.Find(solicitudUsuario.IdUsuario);
            if (usuario == null)
            {
                return BadRequest($"El id del usuario : {solicitudUsuario.IdUsuario} no fue encontrado ");
            }
            try
            {
                if (usuario == null)
                {
                    return StatusCode(StatusCodes.Status200OK, new { mensaje = "No se encontro ningun usuario." });

                }
                if (usuario.Activo != false)
                {
                    usuario.Activo = false;
                }
                else
                {
                    return StatusCode(StatusCodes.Status200OK, new { mensaje = $"El usuario {usuario.NombreUsuario} ya esta inactivo en el sistema." });

                }
                _bdGastosContext.Update(usuario);
                _bdGastosContext.SaveChanges();
                return StatusCode(StatusCodes.Status200OK, new { mensaje = "ok" });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status400BadRequest, new { mensaje = ex.Message });
            }
        }
    }
}
