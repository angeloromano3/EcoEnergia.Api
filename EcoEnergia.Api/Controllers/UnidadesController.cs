using EcoEnergia.Api.Services;
using EcoEnergia.Api.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace EcoEnergia.Api.Controllers
{
    // Rota base: /api/unidades
    [ApiController]
    [Route("api/[controller]")]
    public class UnidadesController : ControllerBase
    {
        private readonly IUnidadeService _service;

        // O service chega aqui sozinho (registrado no Program.cs)
        public UnidadesController(IUnidadeService service)
        {
            _service = service;
        }

        // GET /api/unidades?pagina=1&tamanho=10
        [HttpGet]
        public async Task<IActionResult> Listar([FromQuery] int pagina = 1, [FromQuery] int tamanho = 10)
        {
            // Proteção: página mínima 1 e no máximo 50 itens por página
            if (pagina < 1) pagina = 1;
            if (tamanho < 1 || tamanho > 50) tamanho = 10;

            var resultado = await _service.ListarAsync(pagina, tamanho);
            return Ok(resultado);
        }

        // GET /api/unidades/5
        [HttpGet("{id}")]
        public async Task<IActionResult> BuscarPorId(long id)
        {
            var unidade = await _service.BuscarPorIdAsync(id);
            return unidade == null ? NotFound() : Ok(unidade);
        }

        // POST /api/unidades
        [HttpPost]
        public async Task<IActionResult> Criar([FromBody] UnidadeInputViewModel input)
        {
            var criada = await _service.CriarAsync(input);
            // Devolve 201 Created com o endereço da nova unidade
            return CreatedAtAction(nameof(BuscarPorId), new { id = criada.Id }, criada);
        }

        // PUT /api/unidades/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Atualizar(long id, [FromBody] UnidadeInputViewModel input)
        {
            var atualizada = await _service.AtualizarAsync(id, input);
            return atualizada == null ? NotFound() : Ok(atualizada);
        }

        // DELETE /api/unidades/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Remover(long id)
        {
            var removida = await _service.RemoverAsync(id);
            return removida ? NoContent() : NotFound();
        }
    }
}