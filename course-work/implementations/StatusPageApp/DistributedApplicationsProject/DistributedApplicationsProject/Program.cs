using AutoMapper;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using StatusPageData;
using StatusPageRepo;
using StatusPageRepo.Implementations;
using StatusPageServices.Interfaces;
using StatusPageServices.Services;
using StatusPageServices.Validation.Creates;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddValidatorsFromAssemblyContaining<IncidentCreateValidator>();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Connection string
var connectionString = builder.Configuration.GetConnectionString("StatusPageDb");

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(connectionString));

builder.Services.AddScoped(typeof(IRepo<>), typeof(BaseImplementation<>));
// Register concrete services
builder.Services.AddScoped<IIncidentsService, IncidentService>();
builder.Services.AddScoped<IEngineersService, EngineersService>();
builder.Services.AddScoped<IServicesService, ServicesService>();
builder.Services.AddScoped<IServiceCategoriesService, ServiceCategoriesService>();
builder.Services.AddScoped<IIncidentUpdatesService, IncidentUpdatesService>();
builder.Services.AddScoped<IServiceChecksService, ServiceChecksService>();
builder.Services.AddScoped<IUserService, UserService>();

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
