using Microsoft.AspNetCore.Identity;
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

            // register your db context
            builder.Services.AddDbContext<DataContext>(options => options.UseSqlite(connectionString));

            // add identity and create the db
            builder.Services.AddDefaultIdentity<IdentityUser>()
                .AddEntityFrameworkStores<DataContext>();

            builder.Services.AddScoped<IPostService, PostService>();
            builder.Services.AddScoped<IFlowerService, FlowerService>();
        }
    }
}
