using alderam.stocks.api.Models;
using Microsoft.EntityFrameworkCore;

namespace alderam.stocks.api.Database
{
    public class DatabaseContext : DbContext
    {
        public DatabaseContext(DbContextOptions dbContextOptions) : base(dbContextOptions)
        {
        }

        public DbSet<Setor> Setores { get; set; }
        public DbSet<Ativo> Ativos { get; set; }
        public DbSet<Operacao> Operacoes { get; set; }
        public DbSet<Boleta> Boletas { get; set; }
        public DbSet<Acompanhamento> Acompanhamentos { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Ativo>()
                .HasIndex(b => b.Codigo)
                .IsUnique();
        }
    }
}
