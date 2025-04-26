using CoreApp;
using DTO;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InversionController : ControllerBase
    {
        private readonly InversionManager _inversionManager;

        public InversionController()
        {
            _inversionManager = new InversionManager();
        }

        // GET -> RetrieveAll
        [HttpGet]
        [Route("RetrieveAll")]
        public ActionResult RetrieveAll()
        {
            try
            {
                var inversiones = _inversionManager.RetrieveAll();
                return Ok(inversiones);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error al recuperar inversiones: {ex.Message}");
            }
        }

        // 🔥 Nuevo -> Retrieve inversiones por ID de cliente
        [HttpGet]
        [Route("RetrieveByCliente/{idCliente}")]
        public ActionResult RetrieveByIdCliente(int idCliente)
        {
            try
            {
                var inversionesCliente = _inversionManager.RetrieveByIdCliente(idCliente);

                if (inversionesCliente == null || inversionesCliente.Count == 0)
                {
                    return NotFound(new { mensaje = $"No se encontraron inversiones para el cliente con ID {idCliente}." });
                }

                return Ok(inversionesCliente);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error al recuperar inversiones del cliente: {ex.Message}");
            }
        }
    }
}
