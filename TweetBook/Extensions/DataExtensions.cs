using Microsoft.EntityFrameworkCore;
using TweetBook.Data;

namespace TweetBook.Extensions
{
    public static class DataExtensions
    {
        public static async Task<WebApplication> ApplyMigrationsAsync(this WebApplication app)
        {
            using var scope = app.Services.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<DataContext>();
            await dbContext.Database.MigrateAsync();
            return app;
        }
    }
}
