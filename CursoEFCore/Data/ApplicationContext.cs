using CursoEFCore.Domain;
using Microsoft.EntityFrameworkCore;

namespace CursoEFCore.Data;

public class ApplicationContext : DbContext
{
    public DbSet<Pedido> Pedidos { get; set; }
    
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer("Server = localhost,1433; Database = entityIntro; User Id = sa; " +
                                    "Password=Quiel3386; TrustServerCertificate = True;");
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationContext).Assembly);
    }
}