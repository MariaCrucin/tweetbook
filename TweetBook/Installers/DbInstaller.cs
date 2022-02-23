using Microsoft.EntityFrameworkCore;
using TweetBook.Data;
using TweetBook.Services;

namespace TweetBook.Installers
{
    public class DbInstaller : IInstaller
    {
        public void InstallServices(WebApplicationBuilder builder)
        {
            var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
            builder.Services.AddDbContext<DataContext>(options => options.UseSqlite(connectionString));

            builder.Services.AddSingleton<IPostService, PostService>();
        }
    }
}
