using System.ComponentModel.DataAnnotations;

namespace EcoEnergia.Api.ViewModels
{
    // Dados recebidos no registro de consumo.
    public class ConsumoInputViewModel : IValidatableObject
    {
        [Range(typeof(long), "1", "9223372036854775807",
            ErrorMessage = "Informe um dispositivo válido.")]
        public long DispositivoId { get; set; }

        [Required(ErrorMessage = "A data da medição é obrigatória.")]
        public DateTime? DataMedicao { get; set; }

        [Range(typeof(decimal), "0.01", "99999999.99",
            ParseLimitsInInvariantCulture = true,
            ConvertValueInInvariantCulture = true,
            ErrorMessage = "O consumo deve ser positivo e estar dentro do limite permitido.")]
        public decimal KwhConsumido { get; set; }

        public IEnumerable<ValidationResult> Validate(
            ValidationContext validationContext)
        {
            if (decimal.Round(KwhConsumido, 2) != KwhConsumido)
            {
                yield return new ValidationResult(
                    "O consumo deve possuir até duas casas decimais.",
                    new[] { nameof(KwhConsumido) });
            }

            if (DataMedicao.HasValue)
            {
                if (DataMedicao.Value == DateTime.MinValue)
                {
                    yield return new ValidationResult(
                        "Informe uma data de medição válida.",
                        new[] { nameof(DataMedicao) });
                }
                else if (DataMedicao.Value.Date > DateTime.Today)
                {
                    yield return new ValidationResult(
                        "A data da medição não pode ser futura.",
                        new[] { nameof(DataMedicao) });
                }
            }
        }
    }

    // Dados retornados pela API.
    public class ConsumoViewModel
    {
        public long Id { get; set; }

        public long DispositivoId { get; set; }

        public DateTime DataMedicao { get; set; }

        public decimal KwhConsumido { get; set; }

        public decimal CustoEstimado { get; set; }

        public string Processado { get; set; } = string.Empty;
    }
}