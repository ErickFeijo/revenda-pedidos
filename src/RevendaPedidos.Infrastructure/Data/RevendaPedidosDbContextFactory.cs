using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System.IO;

public class RevendaPedidosDbContextFactory : IDesignTimeDbContextFactory<RevendaPedidosDbContext>
{
    public RevendaPedidosDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<RevendaPedidosDbContext>();

        optionsBuilder.UseSqlServer("Server=localhost;Database=RevendaPedidosDb;User Id=sa;Password=SuaSenhaAqui;");

        return new RevendaPedidosDbContext(optionsBuilder.Options);
    }
}
