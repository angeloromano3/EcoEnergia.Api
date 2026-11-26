using EcoEnergia.Api.Services;
using EcoEnergia.Api.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace EcoEnergia.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ConsumosController : ControllerBase
    {
        private readonly IConsumoService _service;

        public ConsumosController(IConsumoService service)
        {
            _service = service;
        }

        // GET: /api/consumos
        // GET: /api/consumos?dispositivoId=1
        [HttpGet]
        public async Task<IActionResult> Listar(
            [FromQuery] int pagina = 1,
            [FromQuery] int tamanho = 10,
            [FromQuery] long? dispositivoId = null)
        {
            if (pagina < 1)
                pagina = 1;

            if (tamanho < 1 || tamanho > 50)
                tamanho = 10;

            if ((long)(pagina - 1) * tamanho > int.MaxValue)
            {
                return Problem(
                    statusCode: 400,
                    title: "Página fora do limite permitido.");
            }

            if (dispositivoId.HasValue &&
                dispositivoId.Value <= 0)
            {
                return Problem(
                    statusCode: 400,
                    title: "Informe um dispositivo válido.");
            }

            var resultado = await _service.ListarAsync(
                pagina,
                tamanho,
                dispositivoId);

            return Ok(resultado);
        }

        // GET: /api/consumos/1
        [HttpGet("{id}")]
        public async Task<IActionResult> BuscarPorId(long id)
        {
            var consumo = await _service.BuscarPorIdAsync(id);

            if (consumo == null)
            {
                return Problem(
                    statusCode: 404,
                    title: "Consumo não encontrado.");
            }

            return Ok(consumo);
        }

        // POST: /api/consumos
        [HttpPost]
        public async Task<IActionResult> Criar(
            [FromBody] ConsumoInputViewModel input)
        {
            try
            {
                var criado = await _service.CriarAsync(input);

                return CreatedAtAction(
                    nameof(BuscarPorId),
                    new { id = criado.Id },
                    criado);
            }
            catch (ConsumoValidacaoException ex)
            {
                return Problem(
                    statusCode: 400,
                    title: ex.Message);
            }
        }
    }
}