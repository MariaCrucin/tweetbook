using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Mvc;
using System.Reflection;
using TweetBook.Extensions;
using TweetBook.Filters;
using TweetBook.Installers;

var builder = WebApplication.CreateBuilder(args);

builder.InstallServicesInAssembly();

builder.Services.AddAutoMapper(typeof(Program).Assembly);

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
