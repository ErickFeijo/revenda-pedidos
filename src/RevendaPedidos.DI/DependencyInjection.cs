using Microsoft.Extensions.DependencyInjection;
using RevendaPedidos.Application.Interfaces.Services;
using RevendaPedidos.Application.Impl.Services;
using RevendaPedidos.Domain.Interfaces;
using RevendaPedidos.Infra.Repositories;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;
using RevendaPedidos.Infra.Messaging;

namespace RevendaPedidos.DI;

public static class DependencyInjection
{
    public static IServiceCollection AddProjectDependencies(this IServiceCollection services, IConfiguration configuration)
    {
        // Application services
        services.AddScoped<IRevendaService, RevendaService>();
        services.AddScoped<IPedidoService, PedidoService>();
        services.AddScoped<IPedidoIntegracaoService, PedidoIntegracaoService>();
        services.AddScoped<IIntegradorFornecedorService, IntegradorFornecedorMockService>();

        // Infra repositories
        services.AddScoped<IRevendaRepository, RevendaRepository>();
        services.AddScoped<IPedidoRepository, PedidoRepository>();

        services.AddScoped<IPedidoFilaPublisher, PedidoFilaPublisher>();

        // DbContext
        services.AddDbContext<RevendaPedidosDbContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"))
        );

        return services;
    }
}
