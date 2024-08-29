using Serilog.Events;
using Serilog.Sinks.MSSqlServer;
using Serilog;
using System.Collections.ObjectModel;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using EmployeeRegistry.API.Services.EmployeeServices;
using EmployeeRegistry.API.Automappers;
using EmployeeRegistry.API.Repository;
using EmployeeRegistry.API.Models;
using EmployeeRegistry.API.DTOs;
using EmployeeRegistry.API.Validators;
using EmployeeRegistry.API.Utilities;
using EmployeeRegistry.API.Repository.EmployeeRepository;


var builder = WebApplication.CreateBuilder(args);

// Configure Serilog
var columnOptions = new ColumnOptions
{
    AdditionalColumns = new Collection<SqlColumn>
    {
        new SqlColumn { ColumnName = "UserName", DataType = System.Data.SqlDbType.NVarChar, DataLength = 50 }
    }
};

Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Debug()
    .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
    .Enrich.FromLogContext()
    .WriteTo.Console()
    .WriteTo.MSSqlServer(
        connectionString: builder.Configuration.GetConnectionString("SimpleInvoiceConnection"),
        sinkOptions: new MSSqlServerSinkOptions { TableName = "__Logs", AutoCreateSqlTable = true },
        columnOptions: columnOptions,
        restrictedToMinimumLevel: LogEventLevel.Information)
    .CreateLogger();

builder.Host.UseSerilog();

// Add services to the container.
// Register DbContext container services
builder.Services.AddDbContext<EmployeeContext>(options =>
   options.UseSqlServer(builder.Configuration.GetConnectionString("SimpleInvoiceConnection")));

// Register the services
builder.Services.AddScoped<IEmployeeService, EmployeeService>();

// Register the Repository
builder.Services.AddScoped<IEmployeeRepository, EmployeeRepository>();

// Register the Validators
builder.Services.AddTransient<IValidator<EmployeeInsertDto>, EmployeeInsertValidator>();
builder.Services.AddTransient<IValidator<EmployeeUpdateDto>, EmployeeUpdateValidator>();

// Register the AutoMapper Profile
builder.Services.AddAutoMapper(typeof(MappingProfile));


builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Configure CORS to allow any origin
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAllOrigins",
        builder => builder
            .AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader());
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.UseDeveloperExceptionPage();
}

// Register custom exception handling middleware
app.UseMiddleware<ExceptionMiddleware>();

// Use CORS
app.UseCors("AllowAllOrigins");

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
