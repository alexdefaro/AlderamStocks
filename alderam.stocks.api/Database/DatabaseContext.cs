using alderam.stocks.api.Models;
using Microsoft.EntityFrameworkCore;

namespace alderam.stocks.api.Database
{
    public class DatabaseContext : DbContext
    {
        public DatabaseContext(DbContextOptions dbContextOptions) : base(dbContextOptions)
        {
        }
        public DbSet<Ativo> Ativo { get; set; }
        public DbSet<Operacao> Operacoes { get; set; }
    }
}
