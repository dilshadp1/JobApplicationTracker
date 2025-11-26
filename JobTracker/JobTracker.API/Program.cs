using System.Reflection;
using FluentValidation;
using JobTracker.Application.Behaviors;
using JobTracker.Application.Interfaces;
using JobTracker.Infrastructure.Data;
using JobTracker.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

try
{
    var appAssembly = Assembly.Load("JobTracker.Application");

    builder.Services.AddMediatR(cfg =>
    {
        cfg.RegisterServicesFromAssembly(appAssembly);
        cfg.AddOpenBehavior(typeof(ValidationBehavior<,>));
    });
    builder.Services.AddValidatorsFromAssembly(appAssembly);
}
catch (FileNotFoundException)
{
    Console.WriteLine("CRITICAL ERROR: Could not find JobTracker.Application.dll");
}

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
