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

        /*
         * 
         * 
                 [HttpPut]
        [Route("Editar/")]
        public IActionResult EditarGasto([FromBody] Gasto solicitudGasto)
        {
            DateTime fecha = DateTime.Now;
            
            Gasto gasto = _bdGastosContext.Gastos.Find(solicitudGasto.IdGasto);
            if (gasto == null)
            {
                return BadRequest($"El gasto {solicitudGasto.IdGasto} no encontrado ");
            }
            try
            {
                // checar fecha 
                // si la fecha que esta guardada en base de datos es 2024-05-05 y la fecha que se manda por solicitud es actualizable
                if (gasto.Fecha != solicitudGasto.Fecha)
                {
                    gasto.Fecha = solicitudGasto.Fecha;
                }
                // si la fecha no se manda por solicitud pero la fecha que esta en base de datos es distinta a la que se modifico el dia. 
                // hacer una validacion para que no cambie la fecha si no es requerida para cambiar para que la solicitud solo cambie los valores que se van a cambiar 
                // en este caso solo agregar una columna de fechaUltimaModificacion
                // hacer testeo para cambiar id 3 fecha 30-05-2024 Paypal : cambiar solo monto y no debe de cambiar la fecha, verificar que no agregue la fecha del dia actual 02-06-2024
                else
                {
                    gasto.Fecha = fecha;
                }
                gasto.IdCategoriaGasto = solicitudGasto.IdCategoriaGasto is null ? gasto.IdCategoriaGasto : solicitudGasto.IdCategoriaGasto;
                gasto.Descripcion = solicitudGasto.Descripcion is null ? gasto.Descripcion : solicitudGasto.Descripcion;
                gasto.Monto = solicitudGasto.Monto is null ? gasto.Monto : solicitudGasto.Monto;
                // configuracion por parametro de login en 2 etapa front end
                gasto.IdUsuario = 1;

                _bdGastosContext.Update(gasto);
                _bdGastosContext.SaveChanges();

                return StatusCode(StatusCodes.Status200OK, new { mensaje = "ok" });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status400BadRequest, new { mensaje = ex.Message });
            }
        }

        [HttpDelete]
        [Route("Eliminar/{idGasto:int}")]
        public IActionResult EliminarGasto(int idGasto)
        {
            Gasto gasto = _bdGastosContext.Gastos.Find(idGasto);
            if (idGasto == null)
            {
                return BadRequest($"El gasto {idGasto} no fue encontrado ");
            }
            try
            {
                _bdGastosContext.Gastos.Remove(gasto);
                _bdGastosContext.SaveChanges();
                return StatusCode(StatusCodes.Status200OK, new { mensaje = "ok" });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status400BadRequest, new { mensaje = ex.Message, response = gasto });
            }
        }
         * 
         */
    }
}
