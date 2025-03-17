
//using Microsoft.Extensions.Configuration;
//using RabbitMQ.Client;
//using System;

//namespace EventBus.Implementations
//{
//    public sealed class RabbitMQConnection : IDisposable
//    {
//        private readonly IConnection _connection;

//        public RabbitMQConnection(IConfiguration config)
//        {
//            var factory = new ConnectionFactory
//            {

//                HostName = Environment.GetEnvironmentVariable("RabbitMQ-Host"),
//                UserName = Environment.GetEnvironmentVariable("RabbitMQ-Username"),
//                Password = Environment.GetEnvironmentVariable("RabbitMQ-Password"),
//                // Remove local config fallback for production
//                VirtualHost = Environment.GetEnvironmentVariable("RabbitMQ-VirtualHost") ?? "/",
//                DispatchConsumersAsync = true,
//                AutomaticRecoveryEnabled = true,
//                NetworkRecoveryInterval = TimeSpan.FromSeconds(10)
//            };

//            _connection = factory.CreateConnection();
//        }


//        public IModel CreateChannel()
//        {
//            return _connection.CreateModel();
//        }

//        public void DeclareExchange(string exchangeName, string exchangeType = ExchangeType.Direct)
//        {
//            using var channel = CreateChannel();
//            channel.ExchangeDeclare(
//                exchange: exchangeName,
//                type: exchangeType,
//                durable: true,
//                autoDelete: false);
//        }

//        public void Dispose()
//        {
//            _connection?.Dispose();
//        }
//    }
//}

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

            var host = Environment.GetEnvironmentVariable("RABBITMQ-HOST") ?? "localhost";
            //var port = int.Parse(Environment.GetEnvironmentVariable("RABBITMQ-PORT") ?? "5672");
            var username = Environment.GetEnvironmentVariable("RABBITMQ-USERNAME") ?? "admin";
            var password = Environment.GetEnvironmentVariable("RABBITMQ-PASSWORD") ?? "admin123";
            var factory = new ConnectionFactory
            {
                HostName = host,
                UserName = username,
                Password = password,
                VirtualHost = Environment.GetEnvironmentVariable("RabbitMQ-VirtualHost") ?? "/",
                DispatchConsumersAsync = true,
                AutomaticRecoveryEnabled = true,
                NetworkRecoveryInterval = TimeSpan.FromSeconds(10)
            };

            _connection = factory.CreateConnection();
        }

     
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