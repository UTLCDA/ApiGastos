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
    public class GastoController : ControllerBase
    {
        public readonly bdGastosContext _bdGastosContext;

        public GastoController(bdGastosContext bdGastosContext)
        {
            _bdGastosContext = bdGastosContext;
        }

        [HttpGet]
        [Route("Lista/Gastos")]
        public IActionResult ConsultarListadoDeGastos()
        {
            // en una etapa agregar un metodo para consultar por fecha el listado que se desea consultar.
            List<Gasto> lista = new List<Gasto>();
            try
            {
                lista = _bdGastosContext.Gastos.ToList();
                return StatusCode(StatusCodes.Status200OK, new { mensaje = "ok", response = lista });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status400BadRequest, new { mensaje = ex.Message, response = lista });
            }
        }

        [HttpGet]
        [Route("Obtener/{idGasto:int}")]
        public IActionResult ObtenerGastoUnico(int idGasto)
        {
            Gasto gasto = new Gasto();
            gasto = _bdGastosContext.Gastos.Find(idGasto);
            if (idGasto == null)
            {
                return BadRequest("Gasto no encontrado");
            }
            try
            {
                gasto = _bdGastosContext.Gastos.Include(gc => gc.objCategoriaGasto).Where(g => g.IdGasto == idGasto).FirstOrDefault();
                if (gasto == null)
                {
                    return BadRequest("Gasto no encontrado de acuerdo a los parametros de busqueda");
                }
                return StatusCode(StatusCodes.Status200OK, new { mensaje = "ok", response = gasto });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status400BadRequest, new { mensaje = ex.Message, response = gasto });
            }
        }

        [HttpPost]
        [Route("Guardar/")]
        public IActionResult GuargarGasto([FromBody] Gasto gasto)
        {
            DateTime fecha = DateTime.Now;
            try
            // checar para omitir el objeto objCategoriaGasto lo pide en el cuerpo json.
            // agregar una condicion para que el JsonIgnore no lo pida en el cuerpo json
            // se agrega la fecha para que la solicitud tome la fecha actual del sistema.
            {
                gasto.Fecha = fecha;
                _bdGastosContext.Gastos.Add(gasto);
                _bdGastosContext.SaveChanges();
                return StatusCode(StatusCodes.Status200OK, new { mensaje = "ok", response = gasto });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status400BadRequest, new { mensaje = ex.Message });
            }
        }

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

        //consultar por vista para recolpiar los gastos de la semana y por mes en metodos separados
        //consultar por vista para recolpiar los ingresos de la semana y por mes en metodos separados y en el controllador correspondiente
    }
}
