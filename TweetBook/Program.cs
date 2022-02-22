
using Microsoft.EntityFrameworkCore;
using TweetBook.Data;
using TweetBook.Installers;

var builder = WebApplication.CreateBuilder(args);
////var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

// Add services to the container.

builder.InstallServicesInAssembly();

//builder.Services.AddDbContext<DataContext>(options => options.UseSqlite(connectionString));

//builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
////builder.Services.AddEndpointsApiExplorer();
//builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
