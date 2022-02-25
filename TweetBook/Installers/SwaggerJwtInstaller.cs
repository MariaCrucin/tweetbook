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
            // if the class were named JwtSettingModel that nameof(jwtSettings) would be required to corespund with "JwtSettings" from appettings.json

            var jwtSettings = new JwtSettings();
            builder.Configuration.Bind(nameof(jwtSettings), jwtSettings);
            builder.Services.AddSingleton(jwtSettings);

            builder.Services.AddScoped<IIdentityService, IdentityService>();

            builder.Services.AddAuthentication(authenticationOption =>
            {
                authenticationOption.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                authenticationOption.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                authenticationOption.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(jwtBearerOption =>
            {
                jwtBearerOption.SaveToken = true;
                jwtBearerOption.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(jwtSettings.Secret ?? "")),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    RequireExpirationTime = false,
                    ValidateLifetime = true
                };
            });

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
                    // BearerFormat = "JWT",
                    // Scheme = "bearer"
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