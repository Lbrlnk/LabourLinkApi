using Microsoft.Extensions.Configuration;
using RabbitMQ.Client;
using System;

namespace EventBus.Implementations
{
    public sealed class RabbitMQConnection : IDisposable
    {
        private readonly IConnection _connection;

        public RabbitMQConnection(IConfiguration config)
        {
            var factory = new ConnectionFactory
            {
                HostName = config["RabbitMQ:Host"],
                UserName = config["RabbitMQ:Username"],
                Password = config["RabbitMQ:Password"],
                DispatchConsumersAsync = true,
                VirtualHost = config["RabbitMQ:VirtualHost"] ?? "/",
                AutomaticRecoveryEnabled = true,
                NetworkRecoveryInterval = TimeSpan.FromSeconds(10)
            };

            _connection = factory.CreateConnection();
        }

        // ✅ Create a new channel for each operation instead of using a single instance
        public IModel CreateChannel()
        {
            return _connection.CreateModel();
        }

        public void DeclareExchange(string exchangeName, string exchangeType = ExchangeType.Direct)
        {
            using var channel = CreateChannel();
            channel.ExchangeDeclare(
                exchange: exchangeName,
                type: exchangeType,
                durable: true,
                autoDelete: false);
        }

        public void Dispose()
        {
            _connection?.Dispose();
        }
    }
}
