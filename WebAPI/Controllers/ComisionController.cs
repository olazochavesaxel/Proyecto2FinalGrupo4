using CoreApp;
using DTO;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ComisionController : ControllerBase
    {
        private readonly ComisionManager _comisionManager;

        public ComisionController()
        {
            _comisionManager = new ComisionManager(); // Se recomienda inyección de dependencias en lugar de instanciarlo aquí.
        }

        // POST -> Create
        [HttpPost]
        [Route("Create")]
        public ActionResult Create([FromBody] Comision comision)
        {
            try
            {
                _comisionManager.Create(comision);
                return Ok(comision);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error al crear la comisión: {ex.Message}");
            }
        }

        // GET -> RetrieveAll
        [HttpGet]
        [Route("RetrieveAll")]
        public ActionResult RetrieveAll()
        {
            try
            {
                var listResults = _comisionManager.RetrieveAll() ?? new List<Comision>();
                Console.WriteLine($"📡 Datos enviados: {System.Text.Json.JsonSerializer.Serialize(listResults)}");
                return Ok(listResults);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Error en RetrieveAll: {ex.Message}");
                return StatusCode(500, new { error = $"Error al recuperar comisiones: {ex.Message}" });
            }
        }



        // Get -> Retrieve By Id
        [HttpGet]
        [Route("RetrieveById")]
        public ActionResult RetrieveById(int id)
        {
            try
            {
                var listResults = _comisionManager.RetrieveById(id);
                return Ok(listResults);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error al recuperar la comisión: {ex.Message}");
            }
        }

        // get -> retrieve by User code

        [HttpGet]
        [Route("RetrieveByTipo")]
        public ActionResult RetrieveByTipo(string tipo)
        {
            try
            {
                var listResults = _comisionManager.RetrieveByTipo(tipo);
                return Ok(listResults);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error al recuperar la comisón: {ex.Message}");
            }
        }

        // PUT -> Update
        [HttpPut]
        [Route("Update")]
        public ActionResult Update([FromBody] Comision comision)
        {
            try
            {
                _comisionManager.Update(comision);
                return Ok(comision);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error al actualizar la comisión: {ex.Message}");
            }
        }

        // DELETE -> DeleteUser
        [HttpDelete]
        [Route("Delete")]
        public ActionResult Delete(Comision user)
        {
            try
            {
                _comisionManager.Delete(user);
                return Ok("Comisión eliminada correctamente.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error al eliminar comisión: {ex.Message}");
            }
        }
    }
}