using CoreApp;
using DTO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private readonly AdminManager _userManager;

        public AdminController()
        {
            _userManager = new AdminManager(); // Se recomienda inyección de dependencias en lugar de instanciarlo aquí.
        }

        // POST -> Create
        
        [HttpPost]
        [Route("Create")]
        public ActionResult Create([FromBody] Admin user)
        {
            try
            {
                _userManager.Create(user);
                return Ok(user);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error al crear el administrador: {ex.Message}");
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
                return StatusCode(500, $"Error al recuperar administradores: {ex.Message}");
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
                return StatusCode(500, $"Error al recuperar al administrador: {ex.Message}");
            }
        }

        // get -> retrieve by User code

        [HttpGet]
        [Route("RetrieveByCorreo")]
        public ActionResult RetrieveByUserCode(string cedula)
        {
            try
            {
                var listResults = _userManager.RetrieveByCorreo(cedula);
                return Ok(listResults);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error al recuperar administrador: {ex.Message}");
            }
        }

        // PUT -> Update
        [HttpPut]
        [Route("Update")]
        public ActionResult Update([FromBody] Admin user)
        {
            try
            {
                _userManager.Update(user);
                return Ok(user);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error al actualizar administrador: {ex.Message}");
            }
        }

        // DELETE -> DeleteUser
        [HttpDelete]
        [Route("Delete")]
        public ActionResult Delete(Admin user)
        {
            try
            {
                _userManager.Delete(user);
                return Ok("Administrador eliminado correctamente.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error al eliminar administrador: {ex.Message}");
            }
        }
    }
}