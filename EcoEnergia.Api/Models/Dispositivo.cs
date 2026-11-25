using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EcoEnergia.Api.Models
{
    // Mapeia a tabela nova TB_ECO_DISPOSITIVO
    [Table("TB_ECO_DISPOSITIVO")]
    public class Dispositivo
    {
        [Key]
        [Column("CD_DISPOSITIVO")]
        public long Id { get; set; }

        // Chave estrangeira: o número da unidade dona do dispositivo
        [Column("CD_UNIDADE")]
        public long UnidadeId { get; set; }

        // Ligação com a Unidade (equivale ao @ManyToOne do Java)
        [ForeignKey(nameof(UnidadeId))]
        public Unidade? Unidade { get; set; }

        [Required]
        [MaxLength(80)]
        [Column("NM_DISPOSITIVO")]
        public string Nome { get; set; } = string.Empty;

        [Required]
        [MaxLength(40)]
        [Column("DS_TIPO_DISPOSITIVO")]
        public string Tipo { get; set; } = string.Empty;

        // Limite de consumo por hora (decimal = BigDecimal do Java)
        [Column("VL_LIMITE_KWH_HORA", TypeName = "NUMBER(10,2)")]
        public decimal LimiteKwhHora { get; set; }

        // Status padrão "O", igual ao projeto antigo
        [Required]
        [Column("ST_DISPOSITIVO", TypeName = "CHAR(1)")]
        public string Status { get; set; } = "O";

        [Column("DT_INSTALACAO", TypeName = "DATE")]
        public DateTime DataInstalacao { get; set; }
    }
}