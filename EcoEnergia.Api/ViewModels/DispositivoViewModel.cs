using System.ComponentModel.DataAnnotations;

namespace EcoEnergia.Api.ViewModels
{
    // Dados recebidos para cadastro e atualização.
    public class DispositivoInputViewModel : IValidatableObject
    {
        [Range(typeof(long), "1", "9223372036854775807",
            ErrorMessage = "Informe uma unidade válida.")]
        public long UnidadeId { get; set; }

        [Required(ErrorMessage = "O nome é obrigatório.")]
        [StringLength(80,
            ErrorMessage = "O nome pode ter no máximo 80 caracteres.")]
        public string Nome { get; set; } = string.Empty;

        [Required(ErrorMessage = "O tipo é obrigatório.")]
        [StringLength(40,
            ErrorMessage = "O tipo pode ter no máximo 40 caracteres.")]
        public string Tipo { get; set; } = string.Empty;

        [Range(typeof(decimal), "0.01", "99999999.99",
            ParseLimitsInInvariantCulture = true,
            ConvertValueInInvariantCulture = true,
            ErrorMessage = "Informe um limite de consumo válido.")]
        public decimal LimiteKwhHora { get; set; }

        [Required(ErrorMessage = "A data de instalação é obrigatória.")]
        public DateTime? DataInstalacao { get; set; }

        // Validações complementares.
        public IEnumerable<ValidationResult> Validate(
            ValidationContext validationContext)
        {
            if (decimal.Round(LimiteKwhHora, 2) != LimiteKwhHora)
            {
                yield return new ValidationResult(
                    "O limite deve possuir até duas casas decimais.",
                    new[] { nameof(LimiteKwhHora) });
            }

            if (DataInstalacao.HasValue &&
                DataInstalacao.Value == DateTime.MinValue)
            {
                yield return new ValidationResult(
                    "Informe uma data de instalação válida.",
                    new[] { nameof(DataInstalacao) });
            }
        }
    }

    // Dados retornados pela API.
    public class DispositivoViewModel
    {
        public long Id { get; set; }
        public long UnidadeId { get; set; }
        public string Nome { get; set; } = string.Empty;
        public string Tipo { get; set; } = string.Empty;
        public decimal LimiteKwhHora { get; set; }
        public string Status { get; set; } = string.Empty;
        public DateTime DataInstalacao { get; set; }
    }
}