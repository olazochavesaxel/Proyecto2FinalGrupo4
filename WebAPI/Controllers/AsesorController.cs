using CoreApp;
using DTO;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class AsesorController : ControllerBase
    {
        private readonly AsesorManager _userManager;

        public AsesorController()
        {
            _userManager = new AsesorManager(); // Se recomienda inyección de dependencias en lugar de instanciarlo aquí.
        }

        // POST -> Create
        [HttpPost]
        [Route("Create")]
        public ActionResult Create([FromBody] Asesor user)
        {
            try
            {
                var mng = new AsesorManager();
                mng.Create(user);
                return Ok(new { success = true });

            }
            catch (ArgumentException ex)
            {
                // Enviar el mensaje de error con la estructura adecuada
                return BadRequest(new { errors = new { general = new[] { ex.Message } } });
            }
            catch (Exception ex)
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
                return StatusCode(500, $"Error al recuperar asesores: {ex.Message}");
            }
        }

        // Get -> Retrieve By Id
        [HttpGet]
        [Route("RetrieveById")]
        public ActionResult RetrieveById(int id)
        {
            try
            {
                var listResults = _userManager.RetrieveById(id);
                return Ok(listResults);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error al recuperar al asesor: {ex.Message}");
            }
        }

        // get -> retrieve by User code

        [HttpGet]
        [Route("RetrieveByCedula")]
        public ActionResult RetrieveByCedula(string cedula)
        {
            try
            {
                var listResults = _userManager.RetrieveByCedula(cedula);
                return Ok(listResults);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error al recuperar asesor: {ex.Message}");
            }
        }

        [HttpGet]
        [Route("RetrieveByCorreo")]
        public ActionResult RetrieveByCorreo(string correo)
        {
            try
            {
                var listResults = _userManager.RetrieveByCorreo(correo);
                return Ok(listResults);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error al recuperar asesor: {ex.Message}");
            }
        }
        // PUT -> Update
        [HttpPut]
        [Route("Update")]
        public ActionResult Update([FromBody] Asesor user)
        {
            try
            {
                var mng = new AsesorManager();
                mng.Update(user);  // Se llama al método Update de AsesorManager
                return Ok(new { success = true });
            }
            catch (ArgumentException ex)
            {
                // Enviar el mensaje de error con la estructura adecuada
                return BadRequest(new { errors = new { general = new[] { ex.Message } } });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { errors = new { general = new[] { "Error interno del servidor." } } });
            }
        }


        [HttpDelete]
        [Route("Delete/{id}")]
        public ActionResult Delete(int id)
        {
            try
            {
                var user = _userManager.RetrieveById(id);
                if (user == null)
                {
                    return NotFound("Asesor no encontrado.");
                }

                _userManager.Delete(user);
                return Ok("Asesor eliminado correctamente.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error al eliminar Asesor: {ex.Message}");
            }
        }
    }
}
