using EcoEnergia.Api.Services;
using EcoEnergia.Api.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace EcoEnergia.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DispositivosController : ControllerBase
    {
        private readonly IDispositivoService _service;

        public DispositivosController(
            IDispositivoService service)
        {
            _service = service;
        }

        // GET: /api/dispositivos
        [HttpGet]
        public async Task<IActionResult> Listar(
            [FromQuery] int pagina = 1,
            [FromQuery] int tamanho = 10)
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

            var resultado = await _service
                .ListarAsync(pagina, tamanho);

            return Ok(resultado);
        }

        // GET: /api/dispositivos/1
        [HttpGet("{id}")]
        public async Task<IActionResult> BuscarPorId(long id)
        {
            var dispositivo = await _service.BuscarPorIdAsync(id);

            if (dispositivo == null)
            {
                return Problem(
                    statusCode: 404,
                    title: "Dispositivo não encontrado.");
            }

            return Ok(dispositivo);
        }

        // POST: /api/dispositivos
        [HttpPost]
        public async Task<IActionResult> Criar(
            [FromBody] DispositivoInputViewModel input)
        {
            try
            {
                var criado = await _service.CriarAsync(input);

                return CreatedAtAction(
                    nameof(BuscarPorId),
                    new { id = criado.Id },
                    criado);
            }
            catch (DispositivoValidacaoException ex)
            {
                return Problem(
                    statusCode: 400,
                    title: ex.Message);
            }
        }

        // PUT: /api/dispositivos/1
        [HttpPut("{id}")]
        public async Task<IActionResult> Atualizar(
            long id,
            [FromBody] DispositivoInputViewModel input)
        {
            try
            {
                var atualizado = await _service
                    .AtualizarAsync(id, input);

                if (atualizado == null)
                {
                    return Problem(
                        statusCode: 404,
                        title: "Dispositivo não encontrado.");
                }

                return Ok(atualizado);
            }
            catch (DispositivoValidacaoException ex)
            {
                return Problem(
                    statusCode: 400,
                    title: ex.Message);
            }
            catch (DispositivoConflitoException ex)
            {
                return Problem(
                    statusCode: 409,
                    title: ex.Message);
            }
        }

        // DELETE: /api/dispositivos/1
        [HttpDelete("{id}")]
        public async Task<IActionResult> Remover(long id)
        {
            try
            {
                var removido = await _service.RemoverAsync(id);

                if (!removido)
                {
                    return Problem(
                        statusCode: 404,
                        title: "Dispositivo não encontrado.");
                }

                return NoContent();
            }
            catch (DispositivoConflitoException ex)
            {
                return Problem(
                    statusCode: 409,
                    title: ex.Message);
            }
        }
    }
}