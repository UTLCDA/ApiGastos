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
                return BadRequest("Gasto no encontrado");
            }
            try
            {
                categoriasGasto = _bdGastosContext.CategoriasGastos.Where(g => g.IdCategoriasGasto == IdCategoriasGasto).FirstOrDefault();
                if (categoriasGasto == null)
                {
                    return BadRequest("Gasto no encontrado de acuerdo a los parametros de busqueda");
                }
                return StatusCode(StatusCodes.Status200OK, new { mensaje = "ok", response = categoriasGasto });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status400BadRequest, new { mensaje = ex.Message, response = categoriasGasto });
            }
        }
    }
}
