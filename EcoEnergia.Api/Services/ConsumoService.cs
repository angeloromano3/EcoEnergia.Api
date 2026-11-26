using EcoEnergia.Api.Data;
using EcoEnergia.Api.Models;
using EcoEnergia.Api.ViewModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace EcoEnergia.Api.Services
{
    // Contrato das operações de consumo.
    public interface IConsumoService
    {
        Task<PaginaViewModel<ConsumoViewModel>> ListarAsync(
            int pagina,
            int tamanho,
            long? dispositivoId = null);

        Task<ConsumoViewModel?> BuscarPorIdAsync(long id);

        Task<ConsumoViewModel> CriarAsync(
            ConsumoInputViewModel input);
    }

    // Exceção específica para validações de negócio.
    public class ConsumoValidacaoException : Exception
    {
        public ConsumoValidacaoException(string mensagem)
            : base(mensagem)
        {
        }
    }

    public class ConsumoService : IConsumoService
    {
        private readonly AppDbContext _context;
        private readonly IConfiguration _config;

        private const decimal TarifaProvisoria = 0.85m;
        private const decimal ValorMaximoBanco = 99999999.99m;

        public ConsumoService(
            AppDbContext context,
            IConfiguration config)
        {
            _context = context;
            _config = config;
        }

        // Listagem paginada com filtro opcional.
        public async Task<PaginaViewModel<ConsumoViewModel>>
            ListarAsync(
                int pagina,
                int tamanho,
                long? dispositivoId = null)
        {
            var consulta = _context.Consumos.AsNoTracking();

            if (dispositivoId.HasValue)
            {
                consulta = consulta.Where(
                    c => c.DispositivoId == dispositivoId.Value);
            }

            var total = await consulta.CountAsync();

            var itens = await consulta
                .OrderBy(c => c.Id)
                .Skip((pagina - 1) * tamanho)
                .Take(tamanho)
                .Select(c => new ConsumoViewModel
                {
                    Id = c.Id,
                    DispositivoId = c.DispositivoId,
                    DataMedicao = c.DataMedicao,
                    KwhConsumido = c.KwhConsumido,
                    CustoEstimado = c.CustoEstimado,
                    Processado = c.Processado
                })
                .ToListAsync();

            return new PaginaViewModel<ConsumoViewModel>
            {
                Itens = itens,
                Pagina = pagina,
                Tamanho = tamanho,
                TotalItens = total
            };
        }

        // Busca um consumo pelo identificador.
        public async Task<ConsumoViewModel?>
            BuscarPorIdAsync(long id)
        {
            var consumo = await _context.Consumos
                .AsNoTracking()
                .FirstOrDefaultAsync(c => c.Id == id);

            return consumo == null
                ? null
                : ParaViewModel(consumo);
        }

        // Registra um novo consumo energético.
        public async Task<ConsumoViewModel> CriarAsync(
            ConsumoInputViewModel input)
        {
            await ValidarDispositivoAsync(
                input.DispositivoId);

            var custoEstimado = CalcularCusto(
                input.KwhConsumido);

            var consumo = new ConsumoDiario
            {
                DispositivoId = input.DispositivoId,
                DataMedicao = input.DataMedicao!.Value,
                KwhConsumido = input.KwhConsumido,
                CustoEstimado = custoEstimado,
                Processado = "S"
            };

            _context.Consumos.Add(consumo);

            await _context.SaveChangesAsync();

            // TODO - Integração com a Parte 3:
            // Chamar VerificarMetaAsync após definir
            // a interface do serviço de alertas.
            // Coordenar a persistência em uma transação
            // para evitar atualizações parciais.

            return ParaViewModel(consumo);
        }

        // Verifica se o dispositivo existe.
        private async Task ValidarDispositivoAsync(
            long dispositivoId)
        {
            var quantidade = await _context.Dispositivos
                .CountAsync(d => d.Id == dispositivoId);

            if (quantidade == 0)
            {
                throw new ConsumoValidacaoException(
                    "O dispositivo informado não existe.");
            }
        }

        // Calcula o custo energético automaticamente.
        private decimal CalcularCusto(decimal kwh)
        {
            var tarifa = _config.GetValue<decimal>(
                "Energia:TarifaKwh",
                TarifaProvisoria);

            if (tarifa <= 0)
            {
                throw new InvalidOperationException(
                    "A tarifa de energia deve ser positiva.");
            }

            decimal custo;

            try
            {
                custo = decimal.Round(
                    kwh * tarifa,
                    2,
                    MidpointRounding.AwayFromZero);
            }
            catch (OverflowException)
            {
                throw new ConsumoValidacaoException(
                    "O custo estimado ultrapassa o limite permitido.");
            }

            if (custo > ValorMaximoBanco)
            {
                throw new ConsumoValidacaoException(
                    "O custo estimado ultrapassa o limite permitido.");
            }

            return custo;
        }

        // Converte a entidade em dados de saída.
        private static ConsumoViewModel ParaViewModel(
            ConsumoDiario consumo)
        {
            return new ConsumoViewModel
            {
                Id = consumo.Id,
                DispositivoId = consumo.DispositivoId,
                DataMedicao = consumo.DataMedicao,
                KwhConsumido = consumo.KwhConsumido,
                CustoEstimado = consumo.CustoEstimado,
                Processado = consumo.Processado
            };
        }
    }
}