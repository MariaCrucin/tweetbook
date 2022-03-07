using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using TweetBook.Authorization;
using TweetBook.Options;
using TweetBook.Services;

namespace TweetBook.Installers
{
    public class JwtInstaller : IInstaller
    {
        public void InstallServices(WebApplicationBuilder builder)
        {
            JwtSettings jwtSettings = builder.Configuration.GetSection("JwtSettings").Get<JwtSettings>();
            builder.Services.AddSingleton(jwtSettings);

            builder.Services.AddScoped<IIdentityService, IdentityService>();

            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(jwtSettings.Secret)),
                ValidateIssuer = false,
                ValidateAudience = false,
                RequireExpirationTime = false,
                ValidateLifetime = true
            };

            builder.Services.AddSingleton(tokenValidationParameters);

            builder.Services.AddAuthentication(authenticationOption =>
            {
                authenticationOption.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(jwtBearerOption =>
            {
                jwtBearerOption.SaveToken = true;
                jwtBearerOption.TokenValidationParameters = tokenValidationParameters;
            });

            builder.Services.AddAuthorization(options =>
                options.AddPolicy("MustWorkForChapsas", policy =>
                {
                    policy.AddRequirements(new WorksForCompanyRequirement("chapsas.com"));
                })
            );

            builder.Services.AddSingleton<IAuthorizationHandler, WorksForCompanyHandler>();
        }
    }
}