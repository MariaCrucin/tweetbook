using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using TweetBook.Domain;

namespace TweetBook.Data
{
    public class DataContext : IdentityDbContext
    {
        public DataContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<Post> Posts { get; set; }
        public DbSet<Flower> Flowers { get; set; }

        public DbSet<RefreshToken> RefreshTokens { get; set; }
    }
}
