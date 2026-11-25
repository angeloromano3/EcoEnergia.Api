using EcoEnergia.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace EcoEnergia.Api.Data
{
    // Classe que liga os Models ao banco (herda de DbContext do Entity Framework)
    public class AppDbContext : DbContext
    {
        // Recebe as configurações do banco (connection string) pelo Program.cs
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        // Cada DbSet vira uma tabela no banco
        public DbSet<Unidade> Unidades { get; set; }
        public DbSet<Dispositivo> Dispositivos { get; set; }
        public DbSet<ConsumoDiario> Consumos { get; set; }
        public DbSet<MetaEnergia> Metas { get; set; }
        public DbSet<AlertaIncidente> Alertas { get; set; }
        public DbSet<Usuario> Usuarios { get; set; }
    }
}