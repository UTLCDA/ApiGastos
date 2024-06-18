using ApiGastos.Models;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Metadata;

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
                return StatusCode(StatusCodes.Status400BadRequest, new { mensaje = ex.Message});
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

            var condicionCatGastos = _bdGastosContext.CategoriasGastos.FirstOrDefault(p => p.IdCategoriasGasto == gasto.IdCategoriaGasto);
            if (condicionCatGastos == null)
            {
                return StatusCode(StatusCodes.Status200OK, new { mensaje = $"El id {gasto.IdCategoriaGasto} no existe en el catalogo de Categoria de gastos."});

                // Etapa 3 agregar estatus de respuesta Enum 
                /*
                return new RespuestaDto
                {
                    EstatusProceso = EstatusProceso.Invalido,
                    MensajeProceso = $"No existe registro del anticipo Maestro | Cliente : {solicitud.IdCliente} | Folio : {solicitud.Folio} | Sucursal : {solicitud.Udn} | Empresa : {solicitud.IdEmpresa} ."
                };
                */
            }

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
                // si la solicitud de la fecha es null se toma la fecha que se actualizo 
                // si en la solicitud no manda la fecha se asigna la fecha que se registra el cambio
                if (solicitudGasto.Fecha == null)
                {
                    gasto.Fecha = fecha;
                    if (solicitudGasto.Descripcion != null)
                    {
                        gasto.Descripcion = solicitudGasto.Descripcion + $" | Registro Actualizado {fecha}";
                    }
                    else
                    {
                        // se implementa asignacion compuesta porque gasto.Descripcion no es null y se concatena el cambio de la fecha cuando la fecha no se manda por la solicitud
                        gasto.Descripcion += $" | Registro Actualizado {fecha}";
                    }
                }
                // si la fecha de solicitud es distinta porque se requiere cambiar a la que esta almacenada
                else if (gasto.Fecha != solicitudGasto.Fecha)
                {
                    gasto.Fecha = solicitudGasto.Fecha;
                    if (solicitudGasto.Descripcion != null)
                    {
                        gasto.Descripcion = solicitudGasto.Descripcion + $" | Registro Actualizado {fecha}";
                    }
                    else
                    {
                        if (gasto.Descripcion.Contains("|"))
                        {
                            int index = gasto.Descripcion.IndexOf(" | ");
                            if (index != -1)
                            {
                                // Reemplazar todo lo que está a la derecha del símbolo
                                gasto.Descripcion = gasto.Descripcion.Substring(0, index + 3) + $" Registro Actualizado {fecha}"; ;
                            }
                        }
                        else
                        {
                            gasto.Descripcion += $" | Registro Actualizado {fecha}";
                        }
                        
                    }
                }
                // si no se quiere modificar la fecha 2024-06-01 (solicitud fecha = 2024-06-01 )se queda igual, se agrega la fecha de modificacion a descripcion
                else
                {
                    gasto.Fecha = gasto.Fecha;
                    if (solicitudGasto.Descripcion == null)
                    {
                        if (gasto.Descripcion != null)
                        {
                            if (gasto.Descripcion.Contains("|"))
                            {
                                int index = gasto.Descripcion.IndexOf(" | ");
                                if (index != -1)
                                {
                                    // Reemplazar todo lo que está a la derecha del símbolo
                                    gasto.Descripcion = gasto.Descripcion.Substring(0, index + 3) + $" Registro Actualizado {fecha}"; ;
                                }
                            }
                            else
                            {
                                gasto.Descripcion += $" | Registro Actualizado {fecha}";
                            }
                        }
                    }
                    else
                    {
                        gasto.Descripcion = solicitudGasto.Descripcion + $" | Registro Actualizado {fecha}";
                    }
                }

                gasto.IdCategoriaGasto = solicitudGasto.IdCategoriaGasto is null ? gasto.IdCategoriaGasto : solicitudGasto.IdCategoriaGasto;
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
                return StatusCode(StatusCodes.Status400BadRequest, new { mensaje = ex.Message });
            }
        }

        //consultar por vista para recolpiar los gastos de la semana y por mes en metodos separados
        //consultar por vista para recolpiar los gastos de la semana y por mes en metodos separados y en el controllador correspondiente
        //[HttpPut]
        //[Route("Editar/")]
        //public IActionResult ConsultarGastoPorCategoria([FromBody] Gasto solicitudGasto)
        //{
        //    DateTime fechaInicio = DateTime.Now;
        //    DateTime fechaFin = DateTime.Now;

        //    // agregar funcion de periocidad
        //    solicitudGasto.Fecha = fechaInicio;

        //    List<Gasto> gastoLista = new List<Gasto>();
        //    var transaccionesEnRango = gastoLista
        //    .Where(t => t.Fecha >= fechaInicio && t.Fecha <= fechaFin)
        //    .ToList();

        //    // Imprimir las transacciones encontradas
        //    foreach (var transaccion in transaccionesEnRango)
        //    {
        //        Console.WriteLine(transaccion);
        //    }
        //    if (gastoLista == null)
        //    {
        //        return BadRequest($"El gasto {solicitudGasto.IdGasto} no encontrado ");
        //    }
        //    try
        //    {
        //        gastoLista = _bdGastosContext.Gastos.ToList();
        //        return StatusCode(StatusCodes.Status200OK, new { mensaje = "ok", response = gastoLista });
        //    }
        //    catch (Exception ex)
        //    {
        //        return StatusCode(StatusCodes.Status400BadRequest, new { mensaje = ex.Message });
        //    }
        //}
        // metodo para consultar los gatos del mes o por semana o por categoria 
        //Hacer el update para los gastos tengan su categoria exacta
        // Obtener gasto por dos peri
    }
}
