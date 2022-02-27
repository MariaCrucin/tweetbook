using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;
using TweetBook.Options;
using TweetBook.Services;

namespace TweetBook.Installers
{
    public class SwaggerJwtInstaller : IInstaller
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
                options.AddPolicy("FlowerViewer", builder => builder.RequireClaim("flowers.view", "true"))
            );

            builder.Services.AddSwaggerGen(swagerGenOptions =>
            {
                swagerGenOptions.SwaggerDoc("v1", new OpenApiInfo { Title = "Tweetbook API", Version = "v1" });

                // configuring Authorization with Swagger - Accepting Bearer 
                swagerGenOptions.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    In = ParameterLocation.Header,
                    Description = "Please enter token",
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey,
                });

                swagerGenOptions.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        Array.Empty<string>()
                    }
                });
            });
        }
    }
}