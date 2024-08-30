using Serilog.Events;
using Serilog.Sinks.MSSqlServer;
using Serilog;
using System.Collections.ObjectModel;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using EmployeeRegistry.API.Services.EmployeeServices;
using EmployeeRegistry.API.Automappers;
using EmployeeRegistry.API.Models;
using EmployeeRegistry.API.DTOs;
using EmployeeRegistry.API.Validators;
using EmployeeRegistry.API.Utilities;
using EmployeeRegistry.API.Repository.EmployeeRepository;

var builder = WebApplication.CreateBuilder(args);

// Configurar Serilog para usar la consola inicialmente
Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Debug()
    .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
    .Enrich.FromLogContext()
    .WriteTo.Console()
    .CreateLogger();

builder.Host.UseSerilog();

// Setting the connection string
var connectionString = builder.Configuration.GetConnectionString("EmployeeRegistryConnection");

// Add services to the container.
// Register DbContext container services
builder.Services.AddDbContext<EmployeeContext>(options =>
   options.UseSqlServer(connectionString));

// Register the services
builder.Services.AddScoped<IEmployeeService, EmployeeService>();

// Register the Repository
builder.Services.AddScoped<IEmployeeRepository, EmployeeRepository>();

// Register the Validators
builder.Services.AddTransient<IValidator<EmployeeInsertDto>, EmployeeInsertValidator>();
builder.Services.AddTransient<IValidator<EmployeeUpdateDto>, EmployeeUpdateValidator>();
builder.Services.AddTransient<IValidator<EmployeeSearchDto>, EmployeeSearchValidator>();

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

// Aplicar migraciones al iniciar la aplicación
try
{
    using (var scope = app.Services.CreateScope())
    {
        var dbContext = scope.ServiceProvider.GetRequiredService<EmployeeContext>();
        dbContext.Database.Migrate();
    }

    // Reconfigurar Serilog para usar la base de datos después de aplicar las migraciones
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
            connectionString: connectionString,
            sinkOptions: new MSSqlServerSinkOptions { TableName = "__Logs", AutoCreateSqlTable = true },
            columnOptions: columnOptions,
            restrictedToMinimumLevel: LogEventLevel.Information)
        .CreateLogger();

    Log.Information("Serilog reconfigured to use SQL Server.");
}
catch (Exception ex)
{
    Log.Fatal(ex, "An error occurred while applying migrations.");
    throw; // Re-throw the exception to stop the application if migrations fail
}

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
