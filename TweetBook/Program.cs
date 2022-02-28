using TweetBook.Extensions;
using TweetBook.Installers;

var builder = WebApplication.CreateBuilder(args);

builder.InstallServicesInAssembly();

builder.Services.AddAutoMapper(typeof(Program).Assembly);

var app = builder.Build();

await app.ApplyMigrationsAsync();

await app.CreateRolesAsync();

// Configure the HTTP request pipeline.

app.UseAuthentication();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
