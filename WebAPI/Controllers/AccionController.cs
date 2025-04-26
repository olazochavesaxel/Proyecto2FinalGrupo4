using CoreApp;
using DTO;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccionController : ControllerBase
    {
        private readonly AccionManager _accionManager;
        private readonly AlpacaManager _alpacaManager;

        public AccionController(AlpacaManager alpacaManager)
        {
            _accionManager = new AccionManager();
            _alpacaManager = alpacaManager;
        }

        // POST -> Create/Update
        [HttpPost]
        [Route("CreateUpdate")]
        public async Task<ActionResult> CreateUpdate([FromBody] Accion accion)
        {
            try
            {
                var activo = await _alpacaManager.GetAssetAsync(accion.Simbolo);

                if (activo == null)
                {
                    return BadRequest(new { errors = new { general = new[] { $"No se encontró el símbolo {accion.Simbolo} en Alpaca." } } });
                }

                double precio = -1;

                try
                {
                    var precioResult = await _alpacaManager.GetPrecioActual(accion.Simbolo);
                    if (precioResult.price > 0)
                    {
                        precio = (double)precioResult.price;
                    }
                }
                catch
                {
                    // Si falla, dejamos el precio en -1
                }

                accion.Nombre = activo.Name;
                accion.PrecioActual = precio;
                accion.Mercado = activo.Exchange.ToString();
                accion.Moneda = "USD";

                _accionManager.CreateUpdate(accion);

                return Ok(new { success = true });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { errors = new { general = new[] { ex.Message } } });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { errors = new { general = new[] { "Error interno del servidor: " + ex.Message } } });
            }
        }

        // GET -> RetrieveAll
        [HttpGet]
        [Route("RetrieveAll")]
        public ActionResult RetrieveAll()
        {
            try
            {
                var listResults = _accionManager.RetrieveAll();
                return Ok(listResults);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error al recuperar acciones: {ex.Message}");
            }
        }

        // GET -> RetrieveById
        [HttpGet]
        [Route("RetrieveById/{id}")]
        public ActionResult RetrieveById(int id)
        {
            try
            {
                var accion = _accionManager.RetrieveById(id);
                if (accion == null)
                {
                    return NotFound(new { mensaje = $"No se encontró la acción con ID {id}." });
                }

                return Ok(accion);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error al recuperar acción por ID: {ex.Message}");
            }
        }
    }
}
