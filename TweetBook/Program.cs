using Microsoft.EntityFrameworkCore;
using TweetBook.Data;
using TweetBook.Extensions;
using TweetBook.Installers;

var builder = WebApplication.CreateBuilder(args);

builder.InstallServicesInAssembly();

var app = builder.Build();

await app.ApplyMigrations();

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
