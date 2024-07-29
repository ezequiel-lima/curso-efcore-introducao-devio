using CursoEFCore.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace CursoEFCore.Data;

public class ApplicationContext : DbContext
{
    private static readonly ILoggerFactory _logger = LoggerFactory.Create(x => x.AddConsole());
    
    public DbSet<Pedido> Pedidos { get; set; }
    public DbSet<Produto> Produtos { get; set; }
    public DbSet<Cliente> Clientes { get; set; }
    
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder
            .UseLoggerFactory(_logger)
            .EnableSensitiveDataLogging()
            .UseLazyLoadingProxies()
            // configurado string de conexão, resiliencia de conexão, nome da tabela de migração e o schema dela
            .UseSqlServer("Server = localhost,1433; Database = entityIntro; User Id = sa; " +
                          "Password=SqlServer!@#; TrustServerCertificate = True;", 
                x => x.EnableRetryOnFailure(
                    maxRetryCount: 2, 
                    maxRetryDelay: TimeSpan.FromSeconds(5), 
                    errorNumbersToAdd: null)
                    .MigrationsHistoryTable("migracoes_curso_ef_core"));
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationContext).Assembly);
        MapearPropriedadesPorPadrao(modelBuilder);
    }

    // Bonus de como definir um padrão para as propriedades
    private void MapearPropriedadesPorPadrao(ModelBuilder modelBuilder)
    {
        foreach (var entity in modelBuilder.Model.GetEntityTypes())
        {
            var properties = entity.GetProperties().Where(x => x.ClrType == typeof(string));

            foreach (var property in properties)
            {
                if (string.IsNullOrEmpty(property.GetColumnType()) && !property.GetMaxLength().HasValue)
                {
                    property.SetMaxLength(100);
                }
            }
        }
    }
}