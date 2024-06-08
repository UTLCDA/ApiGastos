using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;

using Microsoft.EntityFrameworkCore;
using ApiGastos.Models;

using Microsoft.AspNetCore.Cors;
using System.Text.Json.Serialization;

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

        #region Lista de Ingresos
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
                // COMO AUTOMATIZAR UN PROCESO A TRAVEZ DE UN XML
                return StatusCode(StatusCodes.Status200OK, new { mensaje = ex.Message, response = lista });
            }
        }

        #endregion

        #region Obtener Ingreso por ID
        [HttpGet]
        [Route("ObtenerPorId/{idIngreso:int}")]
        public IActionResult ObtenerIngresoUnico(int idIngreso)
        {
            Ingreso ingreso = new Ingreso();
            ingreso = _bdGastosContext.Ingresos.Find(idIngreso);
            if (idIngreso == null)
            {
                return BadRequest("Ingreso no encontrado");
            }
            try
            {
                ingreso = _bdGastosContext.Ingresos.Include(gc => gc.objCategoriaIngreso).Where(g => g.IdIngreso == idIngreso).FirstOrDefault();
                if (ingreso == null)
                {
                    return BadRequest("Ingreso no encontrado de acuerdo a los parametros de busqueda");
                }
                return StatusCode(StatusCodes.Status200OK, new { mensaje = "ok", response = ingreso });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status400BadRequest, new { mensaje = ex.Message, response = ingreso });
            }
        }

        #endregion

        #region Guardar Ingreso
        [HttpPost]
        [Route("Guardar/")]
        public IActionResult GuargarIngreso([FromBody] Ingreso ingreso)
        {
            DateTime fecha = DateTime.Now;
            try
            // checar para omitir el objeto objCategoriaingreso lo pide en el cuerpo json.
            // agregar una condicion para que el JsonIgnore no lo pida en el cuerpo json
            // se agrega la fecha para que la solicitud tome la fecha actual del sistema.
            {
                ingreso.Fecha = fecha;
                _bdGastosContext.Ingresos.Add(ingreso);
                _bdGastosContext.SaveChanges();
                return StatusCode(StatusCodes.Status200OK, new { mensaje = "ok", response = ingreso });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status400BadRequest, new { mensaje = ex.Message });
            }
        }

        #endregion

        #region Obtener Ingreso por ID
        [HttpPut]
        [Route("EditarIngreso/")]
        public IActionResult EditarIngreso([FromBody] Ingreso solicitudIngreso)
        {
            DateTime fecha = DateTime.Now;
            //string fechaFormato = fecha.ToString("yyyy-MM-dd");

            Ingreso ingreso = _bdGastosContext.Ingresos.Find(solicitudIngreso.IdIngreso);
            if (ingreso == null)
            {
                return BadRequest($"El ingreso {solicitudIngreso.IdIngreso} no encontrado ");
            }
            try
            {
                // checar fecha 
                // si la solicitud de la fecha es null se toma la fecha que se actualizo 
                // si en la solicitud no manda la fecha se asigna la fecha que se registra el cambio
                if (solicitudIngreso.Fecha == null)
                {
                    ingreso.Fecha = fecha;
                    if (solicitudIngreso.Descripcion != null)
                    {
                        ingreso.Descripcion = solicitudIngreso.Descripcion + $" | Registro Actualizado {fecha}";
                    }
                    else
                    {
                        // se implementa asignacion compuesta porque ingreso.Descripcion no es null y se concatena el cambio de la fecha cuando la fecha no se manda por la solicitud
                        ingreso.Descripcion += $" | Registro Actualizado {fecha}";
                    }
                }
                // si la fecha de solicitud es distinta porque se requiere cambiar a la que esta almacenada
                else if (ingreso.Fecha != solicitudIngreso.Fecha)
                {
                    ingreso.Fecha = solicitudIngreso.Fecha;
                    if (solicitudIngreso.Descripcion != null)
                    {
                        ingreso.Descripcion = solicitudIngreso.Descripcion + $" | Registro Actualizado {fecha}";
                    }
                    else
                    {
                        ingreso.Descripcion += $" | Registro Actualizado {fecha}";
                    }
                }
                // si no se quiere modificar la fecha 2024-06-01 (solicitud fecha = 2024-06-01 )se queda igual, se agrega la fecha de modificacion a descripcion
                else
                {
                    ingreso.Fecha = ingreso.Fecha;
                    if (solicitudIngreso.Descripcion == null)
                    {
                        if (ingreso.Descripcion != null)
                        {
                            int index = ingreso.Descripcion.IndexOf(" | ");
                            if (index != -1)
                            {
                                // Reemplazar todo lo que está a la derecha del símbolo
                                ingreso.Descripcion = ingreso.Descripcion.Substring(0, index + 3) + $" Registro Actualizado {fecha}"; ;
                            }
                        }  
                    }
                    else
                    {
                        ingreso.Descripcion = solicitudIngreso.Descripcion + $" | Registro Actualizado {fecha}";
                    }
                }
                ingreso.IdCategoriaIngreso = solicitudIngreso.IdCategoriaIngreso is null ? ingreso.IdCategoriaIngreso : solicitudIngreso.IdCategoriaIngreso;
                //ingreso.Descripcion = solicitudIngreso.Descripcion is null ? ingreso.Descripcion : solicitudIngreso.Descripcion;
                ingreso.Monto = solicitudIngreso.Monto is null ? ingreso.Monto : solicitudIngreso.Monto;
                //ingreso.Fecha = solicitudIngreso.Fecha is null ? ingreso.Fecha : solicitudIngreso.Fecha;
                // configuracion por parametro de login en 2 etapa front end
                ingreso.IdUsuario = 1;

                _bdGastosContext.Update(ingreso);
                _bdGastosContext.SaveChanges();

                return StatusCode(StatusCodes.Status200OK, new { mensaje = "ok" });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status400BadRequest, new { mensaje = ex.Message });
            }
        }
        #endregion

        // eliminar 
        // genenrar consulta por parametro de fechas 
        // genenrar consulta por vista cargando desde el dbcontext para mejores practicas
        // despues implementar un worker services para generar un reporte mensual de todos los gastos | ingresos | etc

    }
}
