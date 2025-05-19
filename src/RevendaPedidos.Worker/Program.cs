using Microsoft.EntityFrameworkCore;
using RevendaPedidos.DI;

var builder = Host.CreateDefaultBuilder(args)
    .ConfigureServices((hostContext, services) =>
    {
        services.AddProjectDependencies(hostContext.Configuration);

        services.AddDbContext<RevendaPedidosDbContext>(options =>
            options.UseSqlServer(hostContext.Configuration.GetConnectionString("DefaultConnection")));

        services.AddHostedService<PedidoIntegracaoWorker>();
    });

var app = builder.Build();

await app.RunAsync();