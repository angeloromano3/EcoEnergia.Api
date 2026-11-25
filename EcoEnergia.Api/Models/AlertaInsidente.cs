using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EcoEnergia.Api.Models
{
    // Mapeia a tabela nova TB_ECO_ALERTA_INCIDENTE
    [Table("TB_ECO_ALERTA_INCIDENTE")]
    public class AlertaIncidente
    {
        [Key]
        [Column("CD_ALERTA")]
        public long Id { get; set; }

        // Chave estrangeira obrigatória: a unidade do alerta
        [Column("CD_UNIDADE")]
        public long UnidadeId { get; set; }

        [ForeignKey(nameof(UnidadeId))]
        public Unidade? Unidade { get; set; }

        // Chave estrangeira OPCIONAL (long? aceita vazio): o dispositivo, se o alerta for de um específico
        [Column("CD_DISPOSITIVO")]
        public long? DispositivoId { get; set; }

        [ForeignKey(nameof(DispositivoId))]
        public Dispositivo? Dispositivo { get; set; }

        // Data do alerta, preenchida automaticamente
        [Column("DT_ALERTA", TypeName = "DATE")]
        public DateTime DataAlerta { get; set; } = DateTime.Now;

        // Gravidade do alerta, ex.: BAIXA, MEDIA, ALTA
        [Required]
        [MaxLength(15)]
        [Column("TP_SEVERIDADE")]
        public string Severidade { get; set; } = string.Empty;

        // Explicação do que aconteceu
        [Required]
        [MaxLength(400)]
        [Column("DS_MOTIVO")]
        public string Motivo { get; set; } = string.Empty;

        // "P" = pendente; vira "R" quando for resolvido
        [Required]
        [Column("ST_RESOLUCAO", TypeName = "CHAR(1)")]
        public string Resolucao { get; set; } = "P";
    }
}