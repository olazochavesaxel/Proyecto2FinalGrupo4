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
                _userManager.Create(user);
                return Ok(user);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error al crear el asesor: {ex.Message}");
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
                _userManager.Update(user);
                return Ok(user);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error al actualizar asesor: {ex.Message}");
            }
        }

        // DELETE -> DeleteUser
        [HttpDelete]
        [Route("Delete")]
        public ActionResult Delete(Asesor user)
        {
            try
            {
                _userManager.Delete(user);
                return Ok("Asesor eliminado correctamente.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error al eliminar asesor: {ex.Message}");
            }
        }
    }
}
