using ApiGastos.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ApiGastos.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PeriocidadController : ControllerBase
    {
        public readonly bdGastosContext _bdGastosContext;

        public PeriocidadController(bdGastosContext bdGastosContext)
        {
            _bdGastosContext = bdGastosContext;
        }

        [HttpGet]
        [Route("Lista/Periocidad")]
        public IActionResult ListarPeriocidad()
        {
            List<Periocidad> listaPeriocidad = new List<Periocidad>();
            try
            {
                listaPeriocidad = _bdGastosContext.Periocidads.ToList();
                if (listaPeriocidad == null)
                {
                    return BadRequest("El valor de la periociidad esta vacio.");
                }
                return StatusCode(StatusCodes.Status200OK, new { mensaje = "ok", response = listaPeriocidad });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status400BadRequest, new { mensaje = ex.Message });
            }
        }

        [HttpGet]
        [Route("Obtener/{IdPeriocidad:int}")]
        public IActionResult ObtenerPeriocidad(int idPeriocidad)
        {
            Periocidad periocidad = new Periocidad();
            periocidad = _bdGastosContext.Periocidads.Find(idPeriocidad);
            if (periocidad == null)
            {
                return BadRequest("Periocidad no encontrada.");
            }
            try
            {
                periocidad = _bdGastosContext.Periocidads.Where(g => g.IdPeriocidad == idPeriocidad).FirstOrDefault();
                if (periocidad == null)
                {
                    return BadRequest("Periocidad no encontrad de acuerdo a los parametros de busqueda.");
                }
                return StatusCode(StatusCodes.Status200OK, new { mensaje = "ok", response = periocidad });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status400BadRequest, new { mensaje = ex.Message, response = periocidad });
            }
        }

        [HttpPost]
        [Route("GuardarPeriocidad")]
        public IActionResult GuardarPeriocidad([FromBody] Periocidad solicitudPeriocidad)
        {
            try
            {
                _bdGastosContext.Periocidads.Add(solicitudPeriocidad);
                _bdGastosContext.SaveChanges();
                return StatusCode(StatusCodes.Status200OK, new { mensaje = "ok", response = solicitudPeriocidad });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status400BadRequest, new { mensaje = ex.Message });
            }
        }

        [HttpPut]
        [Route("EditarPeriocidad/")]
        public IActionResult EditarPeriocidad([FromBody] Periocidad solicitudPeriocidad)
        {
            Periocidad periocidad = _bdGastosContext.Periocidads.Find(solicitudPeriocidad.IdPeriocidad);
            if (periocidad == null)
            {
                return BadRequest($"El id de la periocidad : {solicitudPeriocidad.IdPeriocidad} no fue encontrado ");
            }
            try
            {
                periocidad.Descripcion = solicitudPeriocidad.Descripcion is null ? periocidad.Descripcion : solicitudPeriocidad.Descripcion;
                periocidad.TipoPeriocidad = solicitudPeriocidad.TipoPeriocidad is null ? periocidad.TipoPeriocidad : solicitudPeriocidad.TipoPeriocidad;

                _bdGastosContext.Update(periocidad);
                _bdGastosContext.SaveChanges();

                return StatusCode(StatusCodes.Status200OK, new { mensaje = "ok" });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status400BadRequest, new { mensaje = ex.Message });
            }
        }

        [HttpDelete]
        [Route("Eliminar/{IdPeriocidad:int}")]
        public IActionResult EliminarPeriocidad(int idPeriocidad)
        {
            Periocidad periocidad = _bdGastosContext.Periocidads.Find(idPeriocidad);
            if (periocidad == null)
            {
                return BadRequest($"El id periocidad : {idPeriocidad} no existe en la base de datos. ");
            }
            try
            {
                if (periocidad == null)
                {
                    return StatusCode(StatusCodes.Status200OK, new { mensaje = "No se encontro ninguna periocidad." });

                }
                _bdGastosContext.Periocidads.Remove(periocidad);
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
