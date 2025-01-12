using Microsoft.EntityFrameworkCore;
using Prometheus.SystemMetrics;
using Prometheus;
using TechChallenge.Api.Extensions;
using TechChallenge.Application.Interfaces;
using TechChallenge.Domain.Entities;
using TechChallenge.Infrastructure.Data;
using TechChallenge.Infrastructure.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.EnableAnnotations();
});

// Adiciona os serviços da aplicação
builder.Services.AddApplicationServices();
builder.Services.AddInfrastructureServices(builder.Configuration);
builder.Services.AddSystemMetrics();

// Registro do serviço de leitura CSV
builder.Services.AddScoped<ICsvReaderService, CsvReaderService>();

// Configuração do banco de dados em memória
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseInMemoryDatabase("InMemoryDb"));

var app = builder.Build();

// Carregar dados do CSV e popular o banco de dados
await SeedDatabaseAsync(app);

app.UseSwagger();
app.UseSwaggerUI();
app.UseHttpsRedirection();
app.UseHttpMetrics();
app.MapMetrics();
app.UseAuthorization();
app.MapControllers();

app.Run();

/// <summary>
/// Método responsável por carregar dados do CSV e populá-los no banco de dados.
/// </summary>
async Task SeedDatabaseAsync(WebApplication app)
{
    // Carregar e importar dados do CSV no escopo do app
    using (var scope = app.Services.CreateScope())
    {
        var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        var csvReaderService = scope.ServiceProvider.GetRequiredService<ICsvReaderService>();

        context.Database.EnsureDeleted();  
        context.Database.EnsureCreated(); 

        string filePath = Path.Combine(Directory.GetCurrentDirectory(), "Files", "movielist.csv");

        await csvReaderService.ImportMoviesFromCsvAsync(filePath);

        await SeedDefaultDataAsync(context);
    }
}

/// <summary>
/// Método responsável por adicionar dados iniciais (Seed) ao banco de dados.
/// </summary>
async Task SeedDefaultDataAsync(ApplicationDbContext context)
{
    //// Adiciona dados de exemplo para a entidade Movie
    //if (!context.Movies.Any())
    //{
    //    context.Movies.AddRange(
    //        new Movie { Id = Guid.NewGuid(), Name = "João Silva", PhoneNumber = "991778080", Ddd = "016", Email = "test@test.com" },
    //        new Movie { Id = Guid.NewGuid(), Name = "Pedro Henrique", PhoneNumber = "98776655", Ddd = "016", Email = "test2@test.com" },
    //        new Movie { Id = Guid.NewGuid(), Name = "Manuel Cardoso", PhoneNumber = "909010201", Ddd = "051", Email = "test3@test.com" }
    //    );

    //    await context.SaveChangesAsync();
    //}
}
