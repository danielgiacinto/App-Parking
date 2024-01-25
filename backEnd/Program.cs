using System.Reflection;
using AutoMapper;
using backEnd.Configs;
using backEnd.Models;
using FluentValidation.AspNetCore;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// para que llegue peticiones http
builder.Services.AddCors();

// agregar los servicios
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// conexion con postgresql
builder.Services.AddEntityFrameworkNpgsql().AddDbContext<DbEstacionamientoContext>(options => 
options.UseNpgsql(builder.Configuration.GetConnectionString("DBconnection")));

// agregar mediatR
builder.Services.AddMediatR((config) =>
{
    config.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
});

// agregar el fluentValidation
builder.Services.AddFluentValidation(config =>
{
    config.RegisterValidatorsFromAssembly(Assembly.GetExecutingAssembly());
});

// agregar el AutoMapper
builder.Services.AddAutoMapper(Assembly.GetExecutingAssembly());

// para el manejo de fechas
AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

var app = builder.Build();

app.UseCors(c =>
{
    c.AllowAnyHeader();
    c.AllowAnyMethod();
    c.WithOrigins("http://localhost:4200");
});

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();