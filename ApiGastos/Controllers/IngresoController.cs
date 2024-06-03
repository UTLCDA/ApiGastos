using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;

using Microsoft.EntityFrameworkCore;
using ApiGastos.Models;

using Microsoft.AspNetCore.Cors;

namespace ApiGastos.Controllers
{

    [EnableCors("ReglasCors")]
    [Route("/api/[controller]")]
    [ApiController]
    public class IngresoController : Controller
    {
        public readonly bdGastosContext _bdGastosContext;

        public IngresoController(bdGastosContext bdGastosContext)
        {
            _bdGastosContext = bdGastosContext;
        }

        [HttpGet]
        [Route("Lista/Ingresos")]
        public IActionResult Lista()
        {
            List<Ingreso> lista = new List<Ingreso>();
            try
            {
                lista = _bdGastosContext.Ingresos.ToList();
                return StatusCode(StatusCodes.Status200OK, new { mensaje = "ok", response = lista });
            }
            catch (Exception ex)
            {

                return StatusCode(StatusCodes.Status200OK, new { mensaje = ex.Message, response = lista });
            }
        }
    }
}
