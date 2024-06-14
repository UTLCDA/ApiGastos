using ApiGastos.Models;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ApiGastos.Controllers
{
    [EnableCors("ReglasCors")]
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriasIngresoController : ControllerBase
    {
        public readonly bdGastosContext _bdGastosContext;

        public CategoriasIngresoController(bdGastosContext bdGastosContext)
        {
            _bdGastosContext = bdGastosContext;
        }

        [HttpGet]
        [Route("Lista/CategoriaIngreso")]
        public IActionResult ListarCategoriaIngreso()
        {
            List<CategoriasIngreso> listaCategoriaIngresos = new List<CategoriasIngreso>();
            try
            {
                listaCategoriaIngresos = _bdGastosContext.CategoriasIngresos.ToList();
                if (listaCategoriaIngresos == null)
                {
                    return BadRequest("El valor de la categoria Ingreso esta vacio.");
                }
                return StatusCode(StatusCodes.Status200OK, new { mensaje = "ok", response = listaCategoriaIngresos });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status400BadRequest, new { mensaje = ex.Message });
            }
        }

        [HttpGet]
        [Route("Obtener/{IdCategoriasIngreso:int}")]
        public IActionResult ObtenerCategoriasIngreso(int IdCategoriasIngreso)
        {
            CategoriasIngreso categoriasIngreso = new CategoriasIngreso();
            categoriasIngreso = _bdGastosContext.CategoriasIngresos.Find(IdCategoriasIngreso);
            if (categoriasIngreso == null)
            {
                return BadRequest("Categoria de Ingreso no encontrado");
            }
            try
            {
                categoriasIngreso = _bdGastosContext.CategoriasIngresos.Where(g => g.IdCategoriasIngreso == IdCategoriasIngreso).FirstOrDefault();
                if (categoriasIngreso == null)
                {
                    return BadRequest("Categoria de Ingreso no encontrado de acuerdo a los parametros de busqueda");
                }
                return StatusCode(StatusCodes.Status200OK, new { mensaje = "ok", response = categoriasIngreso });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status400BadRequest, new { mensaje = ex.Message });
            }
        }

        [HttpPost]
        [Route("GuardarCategoriaIngreso")]
        public IActionResult GuardarCategoriaIngreso([FromBody] CategoriasIngreso solicitudCategoriasIngreso)
        {
            try
            {
                _bdGastosContext.CategoriasIngresos.Add(solicitudCategoriasIngreso);
                _bdGastosContext.SaveChanges();
                return StatusCode(StatusCodes.Status200OK, new { mensaje = "ok", response = solicitudCategoriasIngreso });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status400BadRequest, new { mensaje = ex.Message });
            }
        }

        [HttpPut]
        [Route("EditarCategoriaIngreso/")]
        public IActionResult EditarCategoriaIngreso([FromBody] CategoriasIngreso solicitudCategoriasIngreso)
        {
            DateTime fecha = DateTime.Now;

            CategoriasIngreso categoriasIngreso = _bdGastosContext.CategoriasIngresos.Find(solicitudCategoriasIngreso.IdCategoriasIngreso);
            if (categoriasIngreso == null)
            {
                return BadRequest($"EL id de la  categoria del Ingreso {solicitudCategoriasIngreso.IdCategoriasIngreso} no fue encontrado ");
            }
            try
            {
                categoriasIngreso.Nombre = solicitudCategoriasIngreso.Nombre is null ? categoriasIngreso.Nombre : solicitudCategoriasIngreso.Nombre;
                categoriasIngreso.Descripcion = solicitudCategoriasIngreso.Descripcion is null ? categoriasIngreso.Descripcion : solicitudCategoriasIngreso.Descripcion;

                _bdGastosContext.Update(categoriasIngreso);
                _bdGastosContext.SaveChanges();

                return StatusCode(StatusCodes.Status200OK, new { mensaje = "ok" });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status400BadRequest, new { mensaje = ex.Message });
            }
        }

        [HttpDelete]
        [Route("Eliminar/{IdCategoriasIngreso:int}")]
        public IActionResult EliminarCategoriaIngreso(int idCategoriasIngreso)
        {
            CategoriasIngreso categoriasIngreso = _bdGastosContext.CategoriasIngresos.Find(idCategoriasIngreso);
            if (idCategoriasIngreso == null)
            {
                return BadRequest($"El Ingreso {idCategoriasIngreso} no fue encontrado ");
            }
            try
            {
                if (categoriasIngreso == null)
                {
                    return StatusCode(StatusCodes.Status200OK, new { mensaje = "No se encontro ningun Ingreso." });

                }
                _bdGastosContext.CategoriasIngresos.Remove(categoriasIngreso);
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
