using FreelancerManager.Infrastructure.Persistence;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using FreelancerManager.Application.Interfaces.Repositories;
using FreelancerManager.Infrastructure.Persistence.Repositories;
using FreelancerManager.API.Middlewares;
using FreelancerManager.Application.Services;

var builder = WebApplication.CreateBuilder(args);

// ---------------------------------------------------------------------------
// 1) Serviços
// ---------------------------------------------------------------------------
builder.Services.AddControllers();
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// ---------------------------------------------------------------------------
// 2) DbContext + SQLite (com caminho ABSOLUTO — banco sempre na pasta da API)
// ---------------------------------------------------------------------------
var rawConnectionString = builder.Configuration.GetConnectionString("DefaultConnection")
    ?? "Data Source=freelancer.db";

var connectionStringBuilder = new SqliteConnectionStringBuilder(rawConnectionString);
if (!Path.IsPathRooted(connectionStringBuilder.DataSource))
{
    connectionStringBuilder.DataSource = Path.Combine(
        builder.Environment.ContentRootPath,
        connectionStringBuilder.DataSource);
}

builder.Services.AddDbContext<FreelancerManagerDbContext>(options =>
    options.UseSqlite(connectionStringBuilder.ToString()));

// Injeção de Dependência do Repositório
builder.Services.AddScoped<IReceiptRepository, ReceiptRepository>();
builder.Services.AddScoped<IReceiptService, ReceiptService>();
builder.Services.AddScoped<ITaxCalculationStrategy, StandardTaxCalculationStrategy>();


var app = builder.Build();

app.UseCors("AllowAll");
app.UseMiddleware<ExceptionHandlingMiddleware>();

// ---------------------------------------------------------------------------
// 3) Aplica migrations pendentes na inicialização.
//    Na primeira execução, cria o arquivo freelancer.db automaticamente.
// ---------------------------------------------------------------------------
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<FreelancerManagerDbContext>();
    await db.Database.MigrateAsync();
}

// ---------------------------------------------------------------------------
// 4) Pipeline HTTP
// ---------------------------------------------------------------------------
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();