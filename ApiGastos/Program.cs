using Microsoft.EntityFrameworkCore;
using ApiGastos.Models;
using System.Text.Json.Serialization;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// instanciar Serilog
builder.Host.UseSerilog((context, logger) =>
logger.ReadFrom.Configuration(context.Configuration));


builder.Services.AddDbContext<bdGastosContext>(opt => opt.UseSqlServer(builder.Configuration.GetConnectionString("cadenaSQL")));

// ignonar referencia ciclica
builder.Services.AddControllers().AddJsonOptions(opt =>
{
    opt.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
}
);

// inicializarCore para peticiones politicas esta parte es para aque se puedan consumir los servicios a travez de una ip publica
var reglasCoreParaConsumir = "ReglasCors";
builder.Services.AddCors(opcion =>
{
    opcion.AddPolicy(name: reglasCoreParaConsumir, builder =>
    {
        builder.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod();
    });

});

var app = builder.Build();

// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
//{
    //se comenta para hacer la publicacion en prod
//}

app.UseSwagger();
app.UseSwaggerUI();

app.UseCors(reglasCoreParaConsumir);

app.UseAuthorization();

app.MapControllers();

app.Run();
