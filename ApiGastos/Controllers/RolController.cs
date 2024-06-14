using ApiGastos.Models;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ApiGastos.Controllers
{
    [EnableCors("ReglasCors")]
    [Route("api/[controller]")]
    [ApiController]
    public class RolController : ControllerBase
    {
        public readonly bdGastosContext _bdGastosContext;

        public RolController(bdGastosContext bdGastosContext)
        {
            _bdGastosContext = bdGastosContext;
        }

        [HttpGet]
        [Route("Lista/Rol")]
        public IActionResult ListarRoles()
        {
            List<Role> listaRoles = new List<Role>();
            try
            {
                listaRoles = _bdGastosContext.Roles.ToList();
                if (listaRoles == null)
                {
                    return BadRequest("El valor de los roles esta vacio.");
                }
                return StatusCode(StatusCodes.Status200OK, new { mensaje = "ok", response = listaRoles });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status400BadRequest, new { mensaje = ex.Message });
            }
        }

        [HttpGet]
        [Route("Obtener/{IdRol:int}")]
        public IActionResult ObtenerRol(int idRol)
        {
            Role rol = new Role();
            rol = _bdGastosContext.Roles.Find(idRol);
            if (rol == null)
            {
                return BadRequest("Rol no encontrado.");
            }
            try
            {
                rol = _bdGastosContext.Roles.Where(g => g.IdRol == idRol).FirstOrDefault();
                if (rol == null)
                {
                    return BadRequest("Rol no encontrado de acuerdo a los parametros de busqueda.");
                }
                return StatusCode(StatusCodes.Status200OK, new { mensaje = "ok", response = rol });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status400BadRequest, new { mensaje = ex.Message });
            }
        }

        [HttpPost]
        [Route("GuardarRol")]
        public IActionResult GuardarRol([FromBody] Role solicitudRol)
        {
            try
            {
                _bdGastosContext.Roles.Add(solicitudRol);
                _bdGastosContext.SaveChanges();
                return StatusCode(StatusCodes.Status200OK, new { mensaje = "ok", response = solicitudRol });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status400BadRequest, new { mensaje = ex.Message });
            }
        }

        [HttpPut]
        [Route("EditarRol/")]
        public IActionResult EditarRol([FromBody] Role solicitudRol)
        {
            Role rol = _bdGastosContext.Roles.Find(solicitudRol.IdRol);
            if (rol == null)
            {
                return BadRequest($"El id del rol : {solicitudRol.IdRol} no fue encontrado ");
            }
            try
            {
                rol.Descripcion = solicitudRol.Descripcion is null ? rol.Descripcion : solicitudRol.Descripcion;

                // 2da etapa agregar campos de roles
                /*
                    Estado: Estado actual del rol. 0 inactivo 1 activo
                    Permisos: Descripción de los permisos o referencia a otra tabla.
                    Prioridad: Nivel de prioridad del rol.
                    Notas: Notas adicionales.
                 */
                _bdGastosContext.Update(rol);
                _bdGastosContext.SaveChanges();

                return StatusCode(StatusCodes.Status200OK, new { mensaje = "ok" });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status400BadRequest, new { mensaje = ex.Message });
            }
        }

        [HttpDelete]
        [Route("Eliminar/{IdRol:int}")]
        public IActionResult EliminarRol(int idRol)
        {
            Role rol = _bdGastosContext.Roles.Find(idRol);
            if (rol == null)
            {
                return BadRequest($"El id rol : {idRol} no existe en la base de datos. ");
            }
            try
            {
                if (rol == null)
                {
                    return StatusCode(StatusCodes.Status200OK, new { mensaje = "No se encontro ningun rol." });

                }
                _bdGastosContext.Roles.Remove(rol);
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
