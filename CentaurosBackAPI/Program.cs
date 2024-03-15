using CentaurosBackAPI.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);
var misReglasCORS = "MiCors";

builder.Services.AddControllers();

builder.Services.AddDbContext<CentaurosContext>(options =>
{
    var connectionString = builder.Configuration.GetConnectionString("CentaurosDBContext");
    options.UseMySql(connectionString, Microsoft.EntityFrameworkCore.ServerVersion.Parse("8.0.34-mysql"));
});

//CONFIGURACION JWT
builder.Configuration.AddJsonFile("appsettings.json");
var secretkey = builder.Configuration.GetSection("JwtSettings").GetSection("SecretKey").ToString();
var keyBytes = Encoding.UTF8.GetBytes(secretkey);
builder.Services.AddAuthentication(config =>
{
    config.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    config.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(config =>
{
    config.RequireHttpsMetadata = true; //Si requiere HTTPS
    config.SaveToken = true;
    config.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(keyBytes),
        ValidateIssuer = false,
        ValidateAudience = false
    };
});

//CONFIGURACION CORS
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

app.UseCors(misReglasCORS); //CORS

app.UseHttpsRedirection();

app.UseAuthentication(); //JWT

app.UseAuthorization();

app.MapControllers();

app.Run();
