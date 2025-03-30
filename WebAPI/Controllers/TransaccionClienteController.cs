
using Microsoft.AspNetCore.Mvc;
using CoreApp;
using _00_DTO;

namespace WebAPI.Controllers
{
    // Marca esta clase como un controlador de API REST
    [ApiController]

    // Define la ruta base de este controlador → api/transaccioncliente
    [Route("api/[controller]")]
    public class TransaccionClienteController : ControllerBase
    {
        // Instancia del manager que maneja la lógica de negocio
        private readonly TransaccionClienteManager _manager;

        // Constructor que inicializa el manager
        public TransaccionClienteController()
        {
            _manager = new TransaccionClienteManager();
        }

        // ---------------------------
        // GET: api/transaccioncliente
        // Obtener todas las transacciones de clientes
        // ---------------------------
        [HttpGet]
        public IActionResult GetAll()
        {
            var lista = _manager.RetrieveAll();
            return Ok(lista); // Devuelve 200 OK con la lista
        }

        // -------------------------------
        // GET: api/transaccioncliente/{id}
        // Obtener una transacción por su ID
        // -------------------------------
        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var trans = _manager.RetrieveById(id);

            if (trans == null)
                return NotFound("Transacción no encontrada");

            return Ok(trans); // Devuelve 200 OK con la transacción
        }

        // -------------------------------
        // POST: api/transaccioncliente
        // Crear una nueva transacción de cliente
        // -------------------------------
        [HttpPost]
        public IActionResult Create([FromBody] TransaccionCliente trans)
        {
            try
            {
                // Llama al manager para crearla
                _manager.Create(trans);

                // Devuelve 200 OK si todo salió bien
                return Ok("✅ Transacción registrada correctamente");
            }
            catch (Exception ex)
            {
                // Devuelve 400 BadRequest si hubo error
                return BadRequest($"❌ Error al crear: {ex.Message}");
            }
        }

        // -------------------------------------
        // PUT: api/transaccioncliente/{id}
        // Actualizar una transacción existente
        // -------------------------------------
        [HttpPut("{id}")]
        public IActionResult Update(int id, [FromBody] TransaccionCliente trans)
        {
            try
            {
                // Primero busca si existe
                var existente = _manager.RetrieveById(id);
                if (existente == null)
                    return NotFound("Transacción no encontrada");

                // Asegura que el ID no se pierda
                trans.Id = id;

                // Llama al manager para actualizar
                _manager.Update(trans);

                return Ok("✅ Transacción actualizada");
            }
            catch (Exception ex)
            {
                return BadRequest($"❌ Error al actualizar: {ex.Message}");
            }
        }

        // -------------------------------------
        // DELETE: api/transaccioncliente/{id}
        // Eliminar una transacción existente
        // -------------------------------------
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            try
            {
                var existente = _manager.RetrieveById(id);
                if (existente == null)
                    return NotFound("Transacción no encontrada");

                _manager.Delete(existente);
                return Ok("✅ Transacción eliminada");
            }
            catch (Exception ex)
            {
                return BadRequest($"❌ Error al eliminar: {ex.Message}");
            }
        }
    }
}



