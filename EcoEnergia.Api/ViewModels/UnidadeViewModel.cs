using System.ComponentModel.DataAnnotations;

namespace EcoEnergia.Api.ViewModels
{
    // Dados que o cliente MANDA pra criar/editar uma unidade (com validação)
    public class UnidadeInputViewModel
    {
        [Required(ErrorMessage = "O nome é obrigatório.")]
        [StringLength(100, ErrorMessage = "O nome pode ter no máximo 100 caracteres.")]
        public string Nome { get; set; } = string.Empty;

        [Required(ErrorMessage = "O estado é obrigatório.")]
        [StringLength(2, MinimumLength = 2, ErrorMessage = "Use a sigla do estado, ex.: MA.")]
        public string Estado { get; set; } = string.Empty;

        [Required(ErrorMessage = "A cidade é obrigatória.")]
        [StringLength(60)]
        public string Cidade { get; set; } = string.Empty;

        [Required(ErrorMessage = "O segmento é obrigatório.")]
        [StringLength(50)]
        public string Segmento { get; set; } = string.Empty;
    }

    // Dados que a API DEVOLVE (não expõe o Model direto)
    public class UnidadeViewModel
    {
        public long Id { get; set; }
        public string Nome { get; set; } = string.Empty;
        public string Estado { get; set; } = string.Empty;
        public string Cidade { get; set; } = string.Empty;
        public string Segmento { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public DateTime DataCadastro { get; set; }
    }
}