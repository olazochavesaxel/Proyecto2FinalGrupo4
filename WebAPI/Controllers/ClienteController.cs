using CoreApp;
using DTO;
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
            _userManager = new ClienteManager(); // Se recomienda inyección de dependencias en lugar de instanciarlo aquí.
        }

        // POST -> Create
        [HttpPost]
        [Route("Create")]
        public ActionResult Create([FromBody] Cliente user)
        {
            try
            {
                _userManager.Create(user);
                return Ok(user);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error al crear el cliente: {ex.Message}");
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
                return StatusCode(500, $"Error al recuperar al cliente: {ex.Message}");
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
                return StatusCode(500, $"Error al recuperar cliente: {ex.Message}");
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
        public ActionResult Update([FromBody] Cliente user)
        {
            try
            {
                _userManager.Update(user);
                return Ok(user);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error al actualizar cliente: {ex.Message}");
            }
        }

        // DELETE -> DeleteUser
        [HttpDelete]
        [Route("Delete")]
        public ActionResult Delete(Cliente user)
        {
            try
            {
                _userManager.Delete(user);
                return Ok("CLiente eliminado correctamente.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error al eliminar cliente: {ex.Message}");
            }
        }
    }
}


