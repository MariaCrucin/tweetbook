using Microsoft.EntityFrameworkCore;

namespace TweetBook.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions options) : base(options)
        {
        }

        // aici vom defini prop DbSet 
        // public DbSet<Entitate> Tabela_asociata { get; set; }
    }
}
