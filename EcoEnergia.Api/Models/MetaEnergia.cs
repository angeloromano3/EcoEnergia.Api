using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace EcoEnergia.Api.Models
{
    /// Mapeia a tabela nova TB_ECO_META_ENERGIA
    [Table("TB_ECO_META_ENERGIA")]
    // Só pode existir UMA meta por unidade em cada mês/ano (igual ao @UniqueConstraint do Java)
    [Index(nameof(UnidadeId), nameof(Mes), nameof(Ano), IsUnique = true, Name = "UN_ECO_META_UNIDADE_MES_ANO")]
    public class MetaEnergia
    {
        [Key]
        [Column("CD_META")]
        public long Id { get; set; }

        // Chave estrangeira: a unidade dona da meta
        [Column("CD_UNIDADE")]
        public long UnidadeId { get; set; }

        // Ligação com a Unidade (equivale ao @ManyToOne do Java)
        [ForeignKey(nameof(UnidadeId))]
        public Unidade? Unidade { get; set; }

        // Mês da meta (1 a 12)
        [Column("NR_MES")]
        public int Mes { get; set; }

        // Ano da meta, ex.: 2026
        [Column("NR_ANO")]
        public int Ano { get; set; }

        // Limite máximo de kWh no mês
        [Column("VL_META_MAX_KWH", TypeName = "NUMBER(12,2)")]
        public decimal MetaMaxKwh { get; set; }

        // Quanto já foi consumido no mês, começa em zero
        [Column("VL_KWH_ACUMULADO", TypeName = "NUMBER(12,2)")]
        public decimal KwhAcumulado { get; set; } = 0;

        // Situação da meta, começa como EM_ANDAMENTO
        [Required]
        [MaxLength(20)]
        [Column("ST_META")]
        public string Status { get; set; } = "EM_ANDAMENTO";
    }
}