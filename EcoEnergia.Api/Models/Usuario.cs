using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EcoEnergia.Api.Models
{
    // Mapeia a tabela nova TB_ECO_USUARIO (usada no login com JWT)
    [Table("TB_ECO_USUARIO")]
    public class Usuario
    {
        // Aqui a chave primária é o próprio nome de usuário (texto), não um número
        [Key]
        [MaxLength(80)]
        [Column("NM_USUARIO")]
        public string NomeUsuario { get; set; } = string.Empty;

        // Senha guardada como hash (nunca a senha pura)
        [Required]
        [MaxLength(255)]
        [Column("DS_SENHA_HASH")]
        public string SenhaHash { get; set; } = string.Empty;

        // "S" = usuário ativo
        [Required]
        [Column("ST_ATIVO", TypeName = "CHAR(1)")]
        public string Ativo { get; set; } = "S";
    }
}