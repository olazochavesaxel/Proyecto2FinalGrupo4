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
                var mng = new AdminManager();
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
                return StatusCode(500, $"Error al recuperar administradores: {ex.Message}");
            }
        }

        // Get -> Retrieve By Id
        [HttpGet]
        [Route("RetrieveById/{id}")]  // <-- Asegúrate de que la ruta acepta el ID como parámetro
        public ActionResult RetrieveById(int id)
        {
            try
            {
                var admin = _userManager.RetrieveById(id);

                if (admin == null)
                {
                    return NotFound($"No se encontró el administrador con ID {id}");
                }

                return Ok(admin);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error al recuperar al administrador: {ex.Message}");
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
                return StatusCode(500, $"Error al recuperar admin: {ex.Message}");
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

        [HttpPut]
        [Route("Update")]
        public ActionResult Update([FromBody] Admin user)
        {
            try
            {
                var existingUser = _userManager.RetrieveById(user.Id);
                if (existingUser == null)
                {
                    return NotFound("Usuario no encontrado.");
                }

                // Actualizar los datos
                existingUser.Cedula = user.Cedula;
                existingUser.Nombre = user.Nombre;
                existingUser.PrimerApellido = user.PrimerApellido;
                existingUser.SegundoApellido = user.SegundoApellido;
                existingUser.Direccion = user.Direccion;
                existingUser.Telefono = user.Telefono;
                existingUser.Estado = user.Estado;
                existingUser.Rol = user.Rol;
                existingUser.Contrasenna = user.Contrasenna;
                existingUser.FechaNacimiento = user.FechaNacimiento;
                existingUser.Correo = user.Correo;
                existingUser.FotoPerfil = user.FotoPerfil;

                _userManager.Update(existingUser);
                return Ok("Administrador actualizado correctamente.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error al actualizar administrador: {ex.Message}");
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
                    return NotFound("Usuario no encontrado.");
                }

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

