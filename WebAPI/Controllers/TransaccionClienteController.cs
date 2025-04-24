
using Microsoft.AspNetCore.Mvc;
using CoreApp;
using _00_DTO;


namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TransaccionClienteController : ControllerBase
    {
        private readonly TransaccionClienteManager _transaccionClienteManager;

        public TransaccionClienteController()
        {
            _transaccionClienteManager = new TransaccionClienteManager(); // Se recomienda usar inyección de dependencias en lugar de instanciar aquí.
        }

        // POST -> Crear
        [HttpPost]
        [Route("Create")]
        public ActionResult Create([FromBody] TransaccionCliente transaccionCliente)
        {
            try
            {
                _transaccionClienteManager.Create(transaccionCliente);
                return Ok(new { success = true });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { errors = new { general = new[] { ex.Message } } });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { errors = new { general = new[] { "Error interno del servidor." } } });
            }
        }

        // GET -> Recuperar todos
        [HttpGet]
        [Route("RetrieveAll")]
        public ActionResult RetrieveAll()
        {
            try
            {
                var listResults = _transaccionClienteManager.RetrieveAll();
                return Ok(listResults);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error al recuperar transacciones: {ex.Message}");
            }
        }

        // GET -> Recuperar por ID
        [HttpGet]
        [Route("RetrieveById/{id}")]
        public ActionResult RetrieveById(int id)
        {
            try
            {
                var transaccionCliente = _transaccionClienteManager.RetrieveById(id);

                if (transaccionCliente == null)
                {
                    return NotFound($"No se encontró la transacción con ID {id}");
                }

                return Ok(transaccionCliente);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error al recuperar transacción: {ex.Message}");
            }
        }

        // GET -> Recuperar por tipo
        [HttpGet]
        [Route("RetrieveByTipo/{tipo}")]
        public ActionResult RetrieveByTipo(string tipo)
        {
            try
            {
                var transaccionCliente = _transaccionClienteManager.RetrieveByTipo(tipo);

                if (transaccionCliente == null)
                {
                    return NotFound($"No se encontró la transacción con tipo {tipo}");
                }

                return Ok(transaccionCliente);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error al recuperar transacción por tipo: {ex.Message}");
            }
        }

       
       

        // PUT -> Actualizar
        [HttpPut]
        [Route("Update")]
        public ActionResult Update([FromBody] TransaccionCliente transaccionCliente)
        {
            try
            {
                _transaccionClienteManager.Update(transaccionCliente);
                return Ok(transaccionCliente);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error al actualizar la transacción: {ex.Message}");
            }
        }

        // DELETE -> Eliminar
        [HttpDelete]
        [Route("Delete/{id}")]
        public ActionResult Delete(int id)
        {
            try
            {
                var transaccionCliente = _transaccionClienteManager.RetrieveById(id);
                if (transaccionCliente == null)
                {
                    return NotFound("Transacción no encontrada.");
                }

                _transaccionClienteManager.Delete(transaccionCliente);
                return Ok("Transacción eliminada con éxito.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error al eliminar transacción: {ex.Message}");
            }
        }
    }
}
