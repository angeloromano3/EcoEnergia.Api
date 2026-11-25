using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EcoEnergia.Api.Models
{
    // Mapeia a tabela nova TB_ECO_UNIDADE
    [Table("TB_ECO_UNIDADE")]
    public class Unidade
    {
        // Chave primária (o Oracle gera o número sozinho)
        [Key]
        [Column("CD_UNIDADE")]
        public long Id { get; set; }

        [Required]
        [MaxLength(100)]
        [Column("NM_UNIDADE")]
        public string Nome { get; set; } = string.Empty;

        // Sigla do estado, ex.: MA
        [Required]
        [Column("SG_ESTADO", TypeName = "CHAR(2)")]
        public string Estado { get; set; } = string.Empty;

        [Required]
        [MaxLength(60)]
        [Column("NM_CIDADE")]
        public string Cidade { get; set; } = string.Empty;

        [Required]
        [MaxLength(50)]
        [Column("DS_SEGMENTO")]
        public string Segmento { get; set; } = string.Empty;

        // "A" = ativa, já vem como padrão (igual ao @PrePersist do Java)
        [Required]
        [Column("ST_UNIDADE", TypeName = "CHAR(1)")]
        public string Status { get; set; } = "A";

        // Data de cadastro preenchida automaticamente
        [Column("DT_CADASTRO", TypeName = "DATE")]
        public DateTime DataCadastro { get; set; } = DateTime.Now;
    }
}