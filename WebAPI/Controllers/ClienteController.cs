using CoreApp;
using DTO;
using DTOs; // Para ClienteAsesorDTO
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CLienteController : ControllerBase
    {
        private readonly ClienteManager _userManager;

        public CLienteController()
        {
            _userManager = new ClienteManager(); // Se recomienda inyección de dependencias
        }

        // POST -> Create
        [HttpPost]
        [Route("Create")]
        public ActionResult Create([FromBody] Cliente user)
        {
            try
            {
                var mng = new ClienteManager();
                mng.Create(user);
                return Ok(new { success = true });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { errors = new { general = new[] { ex.Message } } });
            }
            catch (Exception)
            {
                return StatusCode(500, new { errors = new { general = new[] { "Error interno del servidor." } } });
            }
        }

        // GET -> RetrieveAll
        [HttpGet]
        [Route("RetrieveAll")]
        public ActionResult RetrieveAll()
        {
            try
            {
                var listResults = _userManager.RetrieveAll();
                return Ok(listResults);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error al recuperar clientes: {ex.Message}");
            }
        }

        // GET -> Retrieve By Id
        [HttpGet]
        [Route("RetrieveById")]
        public ActionResult RetrieveById(int id)
        {
            try
            {
                var result = _userManager.RetrieveById(id);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error al recuperar al cliente: {ex.Message}");
            }
        }

        [HttpGet]
        [Route("RetrieveByCedula")]
        public ActionResult RetrieveByCedula(string cedula)
        {
            try
            {
                var result = _userManager.RetrieveByCedula(cedula);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error al recuperar cliente: {ex.Message}");
            }
        }

        [HttpGet]
        [Route("RetrieveByCorreo")]
        public ActionResult RetrieveByCorreo(string correo)
        {
            try
            {
                var result = _userManager.RetrieveByCorreo(correo);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error al recuperar asesor: {ex.Message}");
            }
        }

        // PUT -> Update
        [HttpPut]
        [Route("Update")]
        public ActionResult Update([FromBody] Cliente user)
        {
            try
            {
                var mng = new ClienteManager();
                mng.Update(user, false); // Se especifica que la contraseña no está hasheada
                return Ok(new { success = true });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { errors = new { general = new[] { ex.Message } } });
            }
            catch (Exception)
            {
                return StatusCode(500, new { errors = new { general = new[] { "Error interno del servidor." } } });
            }
        }

        // DELETE -> DeleteCliente
        [HttpDelete]
        [Route("Delete/{id}")]
        public ActionResult Delete(int id)
        {
            try
            {
                var user = _userManager.RetrieveById(id);
                if (user == null)
                    return NotFound("Cliente no encontrado.");

                _userManager.Delete(user);
                return Ok("Cliente eliminado correctamente.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error al eliminar cliente: {ex.Message}");
            }
        }

        // ✅ NUEVO ENDPOINT: Obtener clientes asignados a un asesor
        [HttpGet]
        [Route("Asignados/{idAsesor}")]
        public ActionResult GetClientesAsignados(int idAsesor)
        {
            try
            {
                var listResults = _userManager.RetrieveClientesAsignados(idAsesor);
                return Ok(listResults);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error al recuperar clientes asignados: {ex.Message}");
            }
        }

        // ✅ NUEVO ENDPOINT: Asignar asesor a cliente
        [HttpPost]
        [Route("AsignarClienteAsesor")]
        public ActionResult AsignarAsesor([FromBody] ClienteAsesorDTO asignacion)
        {
            try
            {
                _userManager.AsignarAsesor(asignacion.IdCliente, asignacion.IdAsesor);
                return Ok("Cliente asignado correctamente al asesor.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"❌ Error al asignar asesor: {ex.Message}");
            }
        }
    }
}


