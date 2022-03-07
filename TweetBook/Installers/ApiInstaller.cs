using FluentValidation.AspNetCore;
using System.Reflection;
using TweetBook.Filters;
using TweetBook.Services;

namespace TweetBook.Installers
{
    public class ApiInstaller : IInstaller
    {
        public void InstallServices(WebApplicationBuilder builder)
        {
            builder.Services.AddControllers()
                .ConfigureApiBehaviorOptions(options =>
                {
                    options.SuppressModelStateInvalidFilter = true;
                });

            builder.Services.AddControllers(options =>
            {
                options.Filters.Add<ValidationFilter>();
            }).AddFluentValidation(options =>
            {
                // Validate child properties and root collection elements
                options.ImplicitlyValidateChildProperties = true;
                options.ImplicitlyValidateRootCollectionElements = true;
                // Automatic registration of validators in assembly
                options.RegisterValidatorsFromAssembly(Assembly.GetExecutingAssembly());
            });

            builder.Services.AddEndpointsApiExplorer();

            builder.Services.AddSingleton<IUriService>(provider =>
            {
                var accessor = provider.GetRequiredService<IHttpContextAccessor>();
                var request = accessor.HttpContext?.Request;
                var absoluteUri = string.Concat(request?.Scheme, "://", request?.Host.ToUriComponent(), "/");
                return new UriService(absoluteUri);
            });
        }
    }
}
