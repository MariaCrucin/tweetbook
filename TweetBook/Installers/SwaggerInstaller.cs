using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Filters;
using System.Reflection;

namespace TweetBook.Installers
{
    public class SwaggerInstaller : IInstaller
    {
        public void InstallServices(WebApplicationBuilder builder)
        {
            builder.Services.AddSwaggerGen(swagerGenOptions =>
            {
                swagerGenOptions.SwaggerDoc("v1", new OpenApiInfo { Title = "Tweetbook API", Version = "v1" });

                swagerGenOptions.ExampleFilters();

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

                // using System.Reflection;
                var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                swagerGenOptions.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
            });

            builder.Services.AddSwaggerExamplesFromAssemblyOf<Program>();
        }
    }
}
