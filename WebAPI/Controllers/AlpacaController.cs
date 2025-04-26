using CoreApp;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AlpacaController : ControllerBase
    {
        private readonly AlpacaManager _alpaca;

        public AlpacaController(AlpacaManager alpaca)
        {
            _alpaca = alpaca;
        }

        [HttpGet("activo/{symbol}")]
        public async Task<IActionResult> GetActivo(string symbol)
        {
            var asset = await _alpaca.GetAssetAsync(symbol.ToUpper());
            return Ok(asset);
        }

        [HttpGet("activos")]
        public async Task<IActionResult> GetActivos()
        {
            var activos = await _alpaca.GetActivosAsync();
            return Ok(activos.Where(a => a.IsTradable).Take(50));
        }

        [HttpGet("activos/light")]
        public async Task<IActionResult> GetActivosLight()
        {
            var activos = await _alpaca.GetActivosAsync();
            var resultado = activos
                .Where(a => a.IsTradable)
                .Select(a => new
                {
                    simbolo = a.Symbol,
                    nombre = a.Name
                });

            return Ok(resultado);
        }

        [HttpGet("precio/{symbol}")]
        public async Task<IActionResult> GetPrecioActual(string symbol)
        {
            try
            {
                var decodedSymbol = Uri.UnescapeDataString(symbol);
                var precio = await _alpaca.GetPrecioActual(decodedSymbol.ToUpper());
                return Ok(new
                {
                    simbolo = decodedSymbol.ToUpper(),
                    precio = precio.price,
                    timeStamp = precio.timeStampUTC
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet("Historico")]
        public async Task<IActionResult> GetHistorico([FromQuery] string symbol, [FromQuery] DateTime from, [FromQuery] DateTime to)
        {
            try
            {
                var data = await _alpaca.GetHistoricoAsync(symbol.ToUpper(), from, to);
                return Ok(data.Select(bar => new
                {
                    fecha = bar.TimeUtc,
                    apertura = bar.Open,
                    cierre = bar.Close,
                    maximo = bar.High,
                    minimo = bar.Low,
                    volumen = bar.Volume
                }));
            }
            catch (Exception ex)
            {
                // ✔️ Ahora verás el mensaje real del error 500 en el navegador
                return StatusCode(500, $"Error al obtener historial: {ex.Message}");
            }
        }
    }
}
