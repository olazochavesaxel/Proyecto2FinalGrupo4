using Microsoft.AspNetCore.Mvc;
using _00_DTO;
using CoreApp;

using TransaccionAsesor = DTOs.TransaccionAsesor;
using DTO;
using Microsoft.AspNetCore.Identity;





namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TransaccionAsesorController : ControllerBase
    {
        private readonly TransaccionAsesorManager _transaccionAsesorManager;

        public TransaccionAsesorController()
        {
            _transaccionAsesorManager = new TransaccionAsesorManager(); // It's recommended to use dependency injection instead of instantiating here.
        }

        // POST -> Create
        [HttpPost]
        [Route("Create")]
        public ActionResult Create([FromBody] TransaccionAsesor transaccionAsesor)
        {
            try
            {
                _transaccionAsesorManager.Create(transaccionAsesor);
                return Ok(new { success = true });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { errors = new { general = new[] { ex.Message } } });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { errors = new { general = new[] { "Internal server error." } } });
            }
        }

        // GET -> RetrieveAll
        [HttpGet]
        [Route("RetrieveAll")]
        public ActionResult RetrieveAll()
        {
            try
            {
                var listResults = _transaccionAsesorManager.RetrieveAll();
                return Ok(listResults);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error retrieving transactions: {ex.Message}");
            }
        }

        [HttpGet]
        [Route("RetrieveById/{id}")]
        public ActionResult RetrieveById(int id)
        {
            try
            {
                var result = _transaccionAsesorManager.RetrieveById(id);
                if (result == null)
                {
                    return NotFound(new { mensaje = $"No se encontró la transacción con ID {id}." });
                }

                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    errores = new { general = new[] { $"Error interno del servidor: {ex.Message}" } }
                });
            }
        }


        [HttpGet]
        [Route("RetrieveByAsesor/{id}")]
        public ActionResult RetrieveByIdAsesor(int id)
        {
            try
            {
                var asesores = new List<int> { id };
                var transacciones = _transaccionAsesorManager.RetrieveByIdAsesor(asesores);

                if (transacciones == null || transacciones.Count == 0)
                {
                    return NotFound(new { mensaje = $"No se encontraron transacciones para el asesor con ID {id}." });
                }

                return Ok(transacciones);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    errores = new { general = new[] { $"Error al recuperar transacciones: {ex.Message}" } }
                });
            }
        }


        [HttpGet]
        [Route("RetrieveByTipo/{tipo}")]
        public ActionResult RetrieveByTipo(string tipo)
        {
            try
            {
                // Crear una lista con el único tipo recibido
                var tipos = new List<string> { tipo };
                var transacciones = _transaccionAsesorManager.RetrieveByTipo(tipos);

                if (transacciones == null || transacciones.Count == 0)
                {
                    return NotFound($"No transactions found with type {tipo}");
                }

                return Ok(transacciones);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error retrieving transactions by type: {ex.Message}");
            }
        }



        // GET -> Retrieve By Comision
        [HttpGet]
        [Route("RetrieveByPayPal/{idPaypal}")]
        public ActionResult RetrieveByPaypall(int idPaypal)
        {
            try
            {
                var transaccionAsesor = _transaccionAsesorManager.RetrieveByPaypal(idPaypal);

                if (transaccionAsesor == null)
                {
                    return NotFound($"No transaction found with paypal ID ");
                }

                return Ok(transaccionAsesor);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error retrieving transaction by commission: {ex.Message}");
            }
        }

        [HttpGet]
        [Route("RetrieveByMyClientes/{idCliente}")]
        public ActionResult RetrieveByMyClientes(int idCliente)
        {
            try
            {
                var transaccionesAsesor = _transaccionAsesorManager.RetrieveByMyClientes(idCliente);

                if (transaccionesAsesor == null || transaccionesAsesor.Count == 0)
                {
                    return NotFound($"No transactions found for client with ID {idCliente}");
                }

                return Ok(transaccionesAsesor);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error retrieving transactions by client: {ex.Message}");
            }
        }



        // PUT -> Update
        [HttpPut]
        [Route("Update")]
        public ActionResult Update([FromBody] TransaccionAsesor transaccionAsesor)
        {
            try
            {
                _transaccionAsesorManager.Update(transaccionAsesor);
                return Ok(transaccionAsesor);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error al actualizar la transacción: {ex.Message}");
            }
        }


    }
}





