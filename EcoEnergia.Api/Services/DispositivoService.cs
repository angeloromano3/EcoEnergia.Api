using EcoEnergia.Api.Data;
using EcoEnergia.Api.Models;
using EcoEnergia.Api.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace EcoEnergia.Api.Services
{
    // Contrato das operações disponíveis.
    public interface IDispositivoService
    {
        Task<PaginaViewModel<DispositivoViewModel>> ListarAsync(
            int pagina, int tamanho);

        Task<DispositivoViewModel?> BuscarPorIdAsync(long id);

        Task<DispositivoViewModel> CriarAsync(
            DispositivoInputViewModel input);

        Task<DispositivoViewModel?> AtualizarAsync(
            long id, DispositivoInputViewModel input);

        Task<bool> RemoverAsync(long id);
    }

    // Exceções específicas das regras de negócio.
    public class DispositivoValidacaoException : Exception
    {
        public DispositivoValidacaoException(string mensagem)
            : base(mensagem)
        {
        }
    }

    public class DispositivoConflitoException : Exception
    {
        public DispositivoConflitoException(string mensagem)
            : base(mensagem)
        {
        }
    }

    public class DispositivoService : IDispositivoService
    {
        private readonly AppDbContext _context;

        public DispositivoService(AppDbContext context)
        {
            _context = context;
        }

        // Listagem paginada e otimizada.
        public async Task<PaginaViewModel<DispositivoViewModel>>
            ListarAsync(int pagina, int tamanho)
        {
            var consulta = _context.Dispositivos.AsNoTracking();

            var total = await consulta.CountAsync();

            var itens = await consulta
                .OrderBy(d => d.Id)
                .Skip((pagina - 1) * tamanho)
                .Take(tamanho)
                .ToListAsync();

            return new PaginaViewModel<DispositivoViewModel>
            {
                Itens = itens.Select(ParaViewModel).ToList(),
                Pagina = pagina,
                Tamanho = tamanho,
                TotalItens = total
            };
        }

        // Busca um dispositivo pelo identificador.
        public async Task<DispositivoViewModel?>
            BuscarPorIdAsync(long id)
        {
            var dispositivo = await _context.Dispositivos
                .AsNoTracking()
                .FirstOrDefaultAsync(d => d.Id == id);

            return dispositivo == null
                ? null
                : ParaViewModel(dispositivo);
        }

        // Cadastro de um novo dispositivo.
        public async Task<DispositivoViewModel> CriarAsync(
            DispositivoInputViewModel input)
        {
            await ValidarUnidadeAsync(input.UnidadeId);

            var dispositivo = new Dispositivo();

            PreencherDados(dispositivo, input);

            _context.Dispositivos.Add(dispositivo);

            await _context.SaveChangesAsync();

            return ParaViewModel(dispositivo);
        }

        // Atualização de um dispositivo existente.
        public async Task<DispositivoViewModel?> AtualizarAsync(
            long id, DispositivoInputViewModel input)
        {
            var dispositivo = await _context.Dispositivos
                .FindAsync(id);

            if (dispositivo == null)
                return null;

            await ValidarUnidadeAsync(input.UnidadeId);

            // Evita transferir o histórico energético
            // para uma unidade diferente.
            if (dispositivo.UnidadeId != input.UnidadeId &&
                await PossuiHistoricoAsync(id))
            {
                throw new DispositivoConflitoException(
                    "Não é possível alterar a unidade de um " +
                    "dispositivo com consumos ou alertas registrados.");
            }

            PreencherDados(dispositivo, input);

            await _context.SaveChangesAsync();

            return ParaViewModel(dispositivo);
        }

        // Exclusão com proteção do histórico energético.
        public async Task<bool> RemoverAsync(long id)
        {
            var dispositivo = await _context.Dispositivos
                .FindAsync(id);

            if (dispositivo == null)
                return false;

            if (await PossuiHistoricoAsync(id))
            {
                throw new DispositivoConflitoException(
                    "Não é possível excluir um dispositivo " +
                    "com consumos ou alertas registrados.");
            }

            _context.Dispositivos.Remove(dispositivo);

            await _context.SaveChangesAsync();

            return true;
        }

        // Verifica a existência da unidade relacionada.
        private async Task ValidarUnidadeAsync(long unidadeId)
        {
            var quantidade = await _context.Unidades
                .CountAsync(u => u.Id == unidadeId);

            if (quantidade == 0)
            {
                throw new DispositivoValidacaoException(
                    "A unidade informada não existe.");
            }
        }
        // Verifica a existência de registros associados.
        private async Task<bool> PossuiHistoricoAsync(long id)
        {
            var totalConsumos = await _context.Consumos
                .CountAsync(c => c.DispositivoId == id);

            if (totalConsumos > 0)
                return true;

            var totalAlertas = await _context.Alertas
                .CountAsync(a => a.DispositivoId == id);

            return totalAlertas > 0;
        }
        // Preenche os dados permitidos para gravação.
        private static void PreencherDados(
            Dispositivo dispositivo,
            DispositivoInputViewModel input)
        {
            dispositivo.UnidadeId = input.UnidadeId;
            dispositivo.Nome = input.Nome.Trim();
            dispositivo.Tipo = input.Tipo.Trim();
            dispositivo.LimiteKwhHora = input.LimiteKwhHora;
            dispositivo.DataInstalacao = input.DataInstalacao!.Value;
        }

        // Converte a entidade no contrato de saída.
        private static DispositivoViewModel ParaViewModel(
            Dispositivo dispositivo)
        {
            return new DispositivoViewModel
            {
                Id = dispositivo.Id,
                UnidadeId = dispositivo.UnidadeId,
                Nome = dispositivo.Nome,
                Tipo = dispositivo.Tipo,
                LimiteKwhHora = dispositivo.LimiteKwhHora,
                Status = dispositivo.Status,
                DataInstalacao = dispositivo.DataInstalacao
            };
        }
    }
}