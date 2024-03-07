using CentaurosBackAPI.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

var builder = WebApplication.CreateBuilder(args);
var misReglasCORS = "MiCors";

// Add services to the container.

builder.Services.AddControllers();

builder.Services.AddDbContext<CentaurosContext>(options =>
{
    var connectionString = builder.Configuration.GetConnectionString("CentaurosDBContext");
    options.UseMySql(connectionString, Microsoft.EntityFrameworkCore.ServerVersion.Parse("8.0.34-mysql"));
});

builder.Services.AddCors(option =>
    option.AddPolicy(name: misReglasCORS,
        builder =>
        {
            //Permite cualquier origen, cualquier cabecera y cualquier metodo o verbo HTTP
            builder.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod();
        }
    )
);

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors(misReglasCORS); //Llamado a la politica de CORS

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
