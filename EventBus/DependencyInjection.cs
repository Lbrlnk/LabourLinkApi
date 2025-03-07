using EventBus.Implementations;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace EventBus
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplicationDI(this IServiceCollection services)
        {

            services.AddSingleton<RabbitMQConnection>(sp =>
            {
                var configuration = sp.GetRequiredService<IConfiguration>();
                var connection = new RabbitMQConnection(configuration);
                // Declare your required exchanges here:
                connection.DeclareExchange("labourlink.events", ExchangeType.Direct);
                // If you need to declare more exchanges, call connection.DeclareExchange for each.
                return connection;
            });
            return services;
        }
    }
}
