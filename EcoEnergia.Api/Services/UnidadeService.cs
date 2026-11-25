using EcoEnergia.Api.Data;
using EcoEnergia.Api.Models;
using EcoEnergia.Api.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace EcoEnergia.Api.Services
{
    // Contrato do service (o controller conhece só a interface, facilita os testes)
    public interface IUnidadeService
    {
        Task<PaginaViewModel<UnidadeViewModel>> ListarAsync(int pagina, int tamanho);
        Task<UnidadeViewModel?> BuscarPorIdAsync(long id);
        Task<UnidadeViewModel> CriarAsync(UnidadeInputViewModel input);
        Task<UnidadeViewModel?> AtualizarAsync(long id, UnidadeInputViewModel input);
        Task<bool> RemoverAsync(long id);
    }

    public class UnidadeService : IUnidadeService
    {
        private readonly AppDbContext _context;

        // O AppDbContext chega aqui sozinho (injeção de dependência, igual ao @Autowired)
        public UnidadeService(AppDbContext context)
        {
            _context = context;
        }

        // Listagem paginada: busca só a página pedida no banco
        public async Task<PaginaViewModel<UnidadeViewModel>> ListarAsync(int pagina, int tamanho)
        {
            var consulta = _context.Unidades.AsNoTracking();

            var total = await consulta.CountAsync();

            var itens = await consulta
                .OrderBy(u => u.Id)
                .Skip((pagina - 1) * tamanho)
                .Take(tamanho)
                .Select(u => ParaViewModel(u))
                .ToListAsync();

            return new PaginaViewModel<UnidadeViewModel>
            {
                Itens = itens,
                Pagina = pagina,
                Tamanho = tamanho,
                TotalItens = total
            };
        }

        public async Task<UnidadeViewModel?> BuscarPorIdAsync(long id)
        {
            var unidade = await _context.Unidades.AsNoTracking().FirstOrDefaultAsync(u => u.Id == id);
            return unidade == null ? null : ParaViewModel(unidade);
        }

        public async Task<UnidadeViewModel> CriarAsync(UnidadeInputViewModel input)
        {
            var unidade = new Unidade
            {
                Nome = input.Nome,
                Estado = input.Estado.ToUpper(),
                Cidade = input.Cidade,
                Segmento = input.Segmento
            };

            _context.Unidades.Add(unidade);
            await _context.SaveChangesAsync();

            return ParaViewModel(unidade);
        }

        public async Task<UnidadeViewModel?> AtualizarAsync(long id, UnidadeInputViewModel input)
        {
            var unidade = await _context.Unidades.FindAsync(id);
            if (unidade == null) return null;

            unidade.Nome = input.Nome;
            unidade.Estado = input.Estado.ToUpper();
            unidade.Cidade = input.Cidade;
            unidade.Segmento = input.Segmento;

            await _context.SaveChangesAsync();
            return ParaViewModel(unidade);
        }

        public async Task<bool> RemoverAsync(long id)
        {
            var unidade = await _context.Unidades.FindAsync(id);
            if (unidade == null) return false;

            _context.Unidades.Remove(unidade);
            await _context.SaveChangesAsync();
            return true;
        }

        // Converte o Model (tabela) no ViewModel (o que a API devolve)
        private static UnidadeViewModel ParaViewModel(Unidade u) => new()
        {
            Id = u.Id,
            Nome = u.Nome,
            Estado = u.Estado,
            Cidade = u.Cidade,
            Segmento = u.Segmento,
            Status = u.Status,
            DataCadastro = u.DataCadastro
        };
    }
}
