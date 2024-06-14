using ApiGastos.Models;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ApiGastos.Controllers
{
    [EnableCors("ReglasCors")]
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriaGastoController : ControllerBase
    {
        public readonly bdGastosContext _bdGastosContext;

        public CategoriaGastoController(bdGastosContext bdGastosContext)
        {
            _bdGastosContext = bdGastosContext;
        }

        [HttpGet]
        [Route("Lista/CategoriaGasto")]
        public IActionResult ListarCategoriaGasto()
        {
            List<CategoriasGasto> listaCategoriaGastos = new List<CategoriasGasto>();
            try
            {
                listaCategoriaGastos = _bdGastosContext.CategoriasGastos.ToList();
                if (listaCategoriaGastos == null)
                {
                    return BadRequest("El valor de la categoria gasto esta vacio.");
                }
                return StatusCode(StatusCodes.Status200OK, new { mensaje = "ok", response = listaCategoriaGastos });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status400BadRequest, new { mensaje = ex.Message, response = listaCategoriaGastos });
            }
        }

        [HttpGet]
        [Route("Obtener/{IdCategoriasGasto:int}")]
        public IActionResult ObtenerCategoriasGasto(int IdCategoriasGasto)
        {
            CategoriasGasto categoriasGasto = new CategoriasGasto();
            categoriasGasto = _bdGastosContext.CategoriasGastos.Find(IdCategoriasGasto);
            if (categoriasGasto == null)
            {
                return BadRequest("Categoria de gasto no encontrado");
            }
            try
            {
                categoriasGasto = _bdGastosContext.CategoriasGastos.Where(g => g.IdCategoriasGasto == IdCategoriasGasto).FirstOrDefault();
                if (categoriasGasto == null)
                {
                    return BadRequest("Categoria de gasto no encontrado de acuerdo a los parametros de busqueda");
                }
                return StatusCode(StatusCodes.Status200OK, new { mensaje = "ok", response = categoriasGasto });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status400BadRequest, new { mensaje = ex.Message, response = categoriasGasto });
            }
        }

        [HttpPost]
        [Route("GuardarCategoriaGasto")]
        public IActionResult GuardarCategoriaGasto([FromBody] CategoriasGasto solicitudCategoriasGasto)
        {
            try
            {
                _bdGastosContext.CategoriasGastos.Add(solicitudCategoriasGasto);
                _bdGastosContext.SaveChanges();
                return StatusCode(StatusCodes.Status200OK, new { mensaje = "ok", response = solicitudCategoriasGasto });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status400BadRequest, new { mensaje = ex.Message });
            }
        }

        [HttpPut]
        [Route("EditarCategoriaGasto/")]
        public IActionResult EditarCategoriaGasto([FromBody] CategoriasGasto solicitudCategoriasGasto)
        {
            DateTime fecha = DateTime.Now;

            CategoriasGasto categoriasGasto = _bdGastosContext.CategoriasGastos.Find(solicitudCategoriasGasto.IdCategoriasGasto);
            if (categoriasGasto == null)
            {
                return BadRequest($"EL id de la  categoria del gasto {solicitudCategoriasGasto.IdCategoriasGasto} no fue encontrado ");
            }
            try
            {
                categoriasGasto.Nombre = solicitudCategoriasGasto.Nombre is null ? categoriasGasto.Nombre : solicitudCategoriasGasto.Nombre;
                categoriasGasto.Descripcion = solicitudCategoriasGasto.Descripcion is null ? categoriasGasto.Descripcion : solicitudCategoriasGasto.Descripcion;

                _bdGastosContext.Update(categoriasGasto);
                _bdGastosContext.SaveChanges();

                return StatusCode(StatusCodes.Status200OK, new { mensaje = "ok" });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status400BadRequest, new { mensaje = ex.Message });
            }
        }

        [HttpDelete]
        [Route("Eliminar/{IdCategoriasGasto:int}")]
        public IActionResult EliminarCategoriaGasto(int idCategoriasGasto)
        {
            CategoriasGasto categoriasGasto = _bdGastosContext.CategoriasGastos.Find(idCategoriasGasto);
            if (idCategoriasGasto == null)
            {
                return BadRequest($"El gasto {idCategoriasGasto} no fue encontrado ");
            }
            try
            {
                if (categoriasGasto == null)
                {
                    return StatusCode(StatusCodes.Status200OK, new { mensaje = "No se encontro ningun gasto." });

                }
                _bdGastosContext.CategoriasGastos.Remove(categoriasGasto);
                _bdGastosContext.SaveChanges();
                return StatusCode(StatusCodes.Status200OK, new { mensaje = "ok" });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status400BadRequest, new { mensaje = ex.Message});
            }
        }
        
         
    }
}
