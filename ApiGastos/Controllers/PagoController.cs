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
    public class PagoController : ControllerBase
    {
        public readonly bdGastosContext _bdGastosContext;

        public PagoController(bdGastosContext bdGastosContext)
        {
            _bdGastosContext = bdGastosContext;
        }

        #region Lista de Pagos
        [HttpGet]
        [Route("Lista/Pagos")]
        public IActionResult ListaPagos()
        {
            List<Pago> lista = new List<Pago>();
            try
            {
                lista = _bdGastosContext.Pagos.ToList();
                return StatusCode(StatusCodes.Status200OK, new { mensaje = "ok", response = lista });
            }
            catch (Exception ex)
            {
                // COMO AUTOMATIZAR UN PROCESO A TRAVEZ DE UN XML
                return StatusCode(StatusCodes.Status200OK, new { mensaje = ex.Message, response = lista });
            }
        }

        #endregion

        #region Obtener Pago por ID
        [HttpGet]
        [Route("ObtenerPorId/{IdPago:int}")]
        public IActionResult ObtenerPagoUnico(int idPago)
        {
            Pago pago = new Pago();
            pago = _bdGastosContext.Pagos.Find(idPago);
            if (idPago == null)
            {
                return BadRequest("Pago no encontrado");
            }
            try
            {
                pago = _bdGastosContext.Pagos.Include(gc => gc.objCategoriaGasto).Where(g => g.IdPago == idPago).FirstOrDefault();
                if (pago == null)
                {
                    return BadRequest("Pago no encontrado de acuerdo a los parametros de busqueda");
                }
                return StatusCode(StatusCodes.Status200OK, new { mensaje = "ok", response = pago });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status400BadRequest, new { mensaje = ex.Message, response = pago });
            }
        }

        #endregion

        #region Guardar Ingreso
        [HttpPost]
        [Route("Guardar")]
        public IActionResult GuargarPago([FromBody] Pago solicitudPago)
        {
            DateTime fecha = DateTime.Now;
            try

            // checar para omitir el objeto objCategoriaingreso lo pide en el cuerpo json.
            // agregar una condicion para que el JsonIgnore no lo pida en el cuerpo json
            // se agrega la fecha para que la solicitud tome la fecha actual del sistema.
            {
                solicitudPago.Fecha = fecha;
                _bdGastosContext.Pagos.Add(solicitudPago);
                _bdGastosContext.SaveChanges();
                return StatusCode(StatusCodes.Status200OK, new { mensaje = "ok", response = solicitudPago });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status400BadRequest, new { mensaje = ex.Message });
            }
        }

        #endregion

        #region Obtener Ingreso por ID
        [HttpPut]
        [Route("EditarPago/")]
        public IActionResult EditarPago([FromBody] Pago solicitudPago)
        {
            DateTime fecha = DateTime.Now;
            //string fechaFormato = fecha.ToString("yyyy-MM-dd");

            Pago pago = _bdGastosContext.Pagos.Find(solicitudPago.IdPago);
            if (pago == null)
            {
                return BadRequest($"El ingreso {solicitudPago.IdPago} no encontrado ");
            }
            try
            {
                if (solicitudPago.Fecha == null)
                {
                    pago.Fecha = fecha;
                    if (solicitudPago.Descripcion != null)
                    {
                        pago.Descripcion = solicitudPago.Descripcion + $" | Registro Actualizado {fecha}";
                    }
                    else
                    {
                        if (pago.Descripcion.Contains("|"))
                        {
                            int index = pago.Descripcion.IndexOf(" | ");
                            if (index != -1)
                            {
                                pago.Descripcion = pago.Descripcion.Substring(0, index + 3) + $" Registro Actualizado {fecha}"; ;
                            }
                        }
                        else
                        {
                            pago.Descripcion += $" | Registro Actualizado {fecha}";
                        }
                    
                    }
                }
                else if (pago.Fecha != solicitudPago.Fecha)
                {
                    pago.Fecha = solicitudPago.Fecha;
                    if (solicitudPago.Descripcion != null)
                    {
                        pago.Descripcion = solicitudPago.Descripcion + $" | Registro Actualizado {fecha}";
                    }
                    else
                    {
                        pago.Descripcion += $" | Registro Actualizado {fecha}";
                    }
                }
                else
                {
                    pago.Fecha = pago.Fecha;
                    if (solicitudPago.Descripcion == null)
                    {
                        if (pago.Descripcion != null)
                        {
                            int index = pago.Descripcion.IndexOf(" | ");
                            if (index != -1)
                            {
                                pago.Descripcion = pago.Descripcion.Substring(0, index + 3) + $" Registro Actualizado {fecha}"; ;
                            }
                        }
                    }
                    else
                    {
                        pago.Descripcion = solicitudPago.Descripcion + $" | Registro Actualizado {fecha}";
                    }
                }
                // verificar condicion , al hacer el update si la solicitud viene llena al registro existente 
                if (solicitudPago.Monto != null)
                {
                    if (solicitudPago.Monto > pago.Balance)
                    {
                        return StatusCode(StatusCodes.Status200OK, new { mensaje = "El importe de tu balance es menor a lo que vas a pagar de tu pago." });
                    }
                    pago.Balance = pago.Balance - solicitudPago.Monto;
                    pago.Pagado = 1;
                }
                pago.IdCategoriaGasto = solicitudPago.IdCategoriaGasto is null ? pago.IdCategoriaGasto : solicitudPago.IdCategoriaGasto;
                pago.Monto = solicitudPago.Monto is null ? pago.Monto : solicitudPago.Monto;
                pago.IdMetodoPago = solicitudPago.IdMetodoPago is null ? pago.IdMetodoPago : solicitudPago.IdMetodoPago;
                pago.FechaTentativa = solicitudPago.FechaTentativa is null ? pago.FechaTentativa : solicitudPago.FechaTentativa;
                pago.Balance = solicitudPago.Balance is null ? pago.Balance : solicitudPago.Balance;

                _bdGastosContext.Update(pago);
                _bdGastosContext.SaveChanges();

                return StatusCode(StatusCodes.Status200OK, new { mensaje = "ok" });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status400BadRequest, new { mensaje = ex.Message });
            }
        }
        #endregion

    }
}
