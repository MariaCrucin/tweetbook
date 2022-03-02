using Microsoft.AspNetCore.Identity;

namespace TweetBook.Extensions
{
    public static class RoleExtensions
    {
        public static async Task<WebApplication> CreateRolesAsync(this WebApplication app)
        { 
            using var scope = app.Services.CreateScope();
            var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

            if (!await roleManager.RoleExistsAsync("Admin"))
            {
                var adminRole = new IdentityRole("Admin");
                await roleManager.CreateAsync(adminRole);
            }

            if (!await roleManager.RoleExistsAsync("Poster"))
            {
                var adminRole = new IdentityRole("Poster");
                await roleManager.CreateAsync(adminRole);
            }

            return app;
        }
    }
}
