using MassTransit;
using RevendaPedidos.Application.DTOs;
using RevendaPedidos.DI;
using Microsoft.Extensions.Hosting;
using Microsoft.EntityFrameworkCore;

var builder = Host.CreateDefaultBuilder(args)
    .ConfigureServices((hostContext, services) =>
    {
        services.AddProjectDependencies(hostContext.Configuration);

        services.AddDbContext<RevendaPedidosDbContext>(options =>
            options.UseSqlServer(hostContext.Configuration.GetConnectionString("DefaultConnection")));

        services.AddMassTransit(x =>
        {
            x.AddConsumer<PedidoFilaConsumer>();

            x.UsingRabbitMq((context, cfg) =>
            {
                cfg.Host("rabbitmq", "/", h =>
                {
                    h.Username("guest");
                    h.Password("guest");
                });

                cfg.ReceiveEndpoint("fila_pedidos", e =>
                {
                    e.ConfigureConsumer<PedidoFilaConsumer>(context);

                    e.UseMessageRetry(r => r.Interval(5, TimeSpan.FromSeconds(60)));
                });
            });
        });
    });

await builder.Build().RunAsync();
