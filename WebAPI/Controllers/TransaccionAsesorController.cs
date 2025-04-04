using Microsoft.AspNetCore.Mvc;
using _00_DTO;
using CoreApp;
using TransaccionAsesor = DTOs.TransaccionAsesor;




namespace WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TransaccionAsesorController : ControllerBase
    {
        private readonly TransaccionAsesorManager manager = new();

        [HttpGet]
        public IActionResult GetAll() => Ok(manager.RetrieveAll());

        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var item = manager.RetrieveById(id);
            return item == null ? NotFound() : Ok(item);
        }

        [HttpPost]
        public IActionResult Create([FromBody] TransaccionAsesor trans)
        {
            try
            {
                manager.Create(trans);
                return Ok(" Transacción registrada correctamente");
            }
            catch (Exception ex)
            {
                return BadRequest("❌ " + ex.Message);
            }
        }

        [HttpPut("{id}")]
        public IActionResult Update(int id, [FromBody] TransaccionAsesor trans)
        {
            trans.Id = id;
            manager.Update(trans);
            return Ok(" Transacción actualizada");
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var trans = manager.RetrieveById(id);
            if (trans == null) return NotFound();
            manager.Delete(trans);
            return Ok(" Transacción eliminada");
        }
    }
}








