using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EcoEnergia.Api.Models
{
    // Mapeia a tabela nova TB_ECO_CONSUMO_DIARIO
    [Table("TB_ECO_CONSUMO_DIARIO")]
    public class ConsumoDiario
    {
        [Key]
        [Column("CD_CONSUMO")]
        public long Id { get; set; }

        // Chave estrangeira: qual dispositivo gerou esse consumo
        [Column("CD_DISPOSITIVO")]
        public long DispositivoId { get; set; }

        // Ligação com o Dispositivo (equivale ao @ManyToOne do Java)
        [ForeignKey(nameof(DispositivoId))]
        public Dispositivo? Dispositivo { get; set; }

        // Dia da medição
        [Column("DT_MEDICAO", TypeName = "DATE")]
        public DateTime DataMedicao { get; set; }

        // Quanto foi consumido no dia, em kWh
        [Column("VL_KWH_CONSUMIDO", TypeName = "NUMBER(10,2)")]
        public decimal KwhConsumido { get; set; }

        // Custo estimado (kWh x tarifa), calculado no Service
        [Column("VL_CUSTO_ESTIMADO", TypeName = "NUMBER(10,2)")]
        public decimal CustoEstimado { get; set; }

        // "S" = já processado, padrão igual ao @PrePersist do Java
        [Required]
        [Column("ST_PROCESSADO", TypeName = "CHAR(1)")]
        public string Processado { get; set; } = "S";
    }
}