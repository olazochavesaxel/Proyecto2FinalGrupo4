using Microsoft.AspNetCore.Mvc;
using CoreApp;
using _00_DTO;
using CoreApp.CoreApp;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class IngresoPlataformaController : ControllerBase
    {
        private readonly IngresoPlataformaManager _ingresoPlataformaManager;

        public IngresoPlataformaController()
        {
            _ingresoPlataformaManager = new IngresoPlataformaManager(); // Se recomienda usar inyección de dependencias en lugar de instanciar aquí.
        }


        // GET -> Recuperar todos
        [HttpGet]
        [Route("RetrieveAll")]
        public ActionResult RetrieveAll()
        {
            try
            {
                var listResults = _ingresoPlataformaManager.RetrieveAll();
                return Ok(listResults);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error al recuperar ingresos: {ex.Message}");
            }
        }

        // GET -> Recuperar por ID
        [HttpGet]
        [Route("RetrieveById/{id}")]
        public ActionResult RetrieveById(int id)
        {
            try
            {
                var ingresoPlataforma = _ingresoPlataformaManager.RetrieveById(id);

                if (ingresoPlataforma == null)
                {
                    return NotFound($"No se encontró el ingreso con ID {id}");
                }

                return Ok(ingresoPlataforma);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error al recuperar ingreso: {ex.Message}");
            }
        }

        // GET -> Recuperar por fecha de ingreso
        [HttpGet]
        [Route("RetrieveByFechaIngreso")]
        public ActionResult RetrieveByFechaIngreso([FromQuery] IngresoPlataforma filtro)
        {
            try
            {
                var ingresos = _ingresoPlataformaManager.RetrieveByFechaIngreso(filtro);

                if (ingresos == null || ingresos.Count == 0)
                {
                    return NotFound("No se encontraron ingresos con los criterios proporcionados.");
                }

                return Ok(ingresos);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error al recuperar ingresos por fecha: {ex.Message}");
            }
        }

        // GET -> Recuperar por ID de asesor
        [HttpGet]
        [Route("RetrieveByIdAsesor/{idAsesor}")]
        public ActionResult RetrieveByIdAsesor(int idAsesor)
        {
            try
            {
                var ingresos = _ingresoPlataformaManager.RetrieveByIdAsesor(idAsesor);

                if (ingresos == null || ingresos.Count == 0)
                {
                    return NotFound($"No se encontraron ingresos para el asesor con ID {idAsesor}");
                }

                return Ok(ingresos);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error al recuperar ingresos por asesor: {ex.Message}");
            }
        }

        // GET -> Recuperar por ID de cliente
        [HttpGet]
        [Route("RetrieveByIdCliente/{idCliente}")]
        public ActionResult RetrieveByIdCliente(int idCliente)
        {
            try
            {
                var ingresos = _ingresoPlataformaManager.RetrieveByIdCliente(idCliente);

                if (ingresos == null || ingresos.Count == 0)
                {
                    return NotFound($"No se encontraron ingresos para el cliente con ID {idCliente}");
                }

                return Ok(ingresos);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error al recuperar ingresos por cliente: {ex.Message}");
            }
        }

        // PUT -> Actualizar
        [HttpPut]
        [Route("Update")]
        public ActionResult Update([FromBody] IngresoPlataforma ingresoPlataforma)
        {
            try
            {
                _ingresoPlataformaManager.Update(ingresoPlataforma);
                return Ok(ingresoPlataforma);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error al actualizar el ingreso: {ex.Message}");
            }
        }

        
    }
}
