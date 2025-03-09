
//using Microsoft.Extensions.Configuration;
//using RabbitMQ.Client;
//using System;

//namespace EventBus.Implementations
//{
//    public sealed class RabbitMQConnection : IDisposable
//    {
//        private readonly IConnection _connection;
//        //public IModel Channel { get; }
//        public IModel CreateChannel() => _connection.CreateModel();
//        public RabbitMQConnection(IConfiguration config)
//        {
//            var factory = new ConnectionFactory
//            {
//                HostName = config["RabbitMQ:Host"],
//                UserName = config["RabbitMQ:Username"],
//                Password = config["RabbitMQ:Password"],
//                DispatchConsumersAsync = true
//            };

//            _connection = factory.CreateConnection();
//            Channel = _connection.CreateModel();
//        }

//        public void DeclareExchange(string exchangeName, string exchangeType = ExchangeType.Direct)
//        {
//            Channel.ExchangeDeclare(
//                exchange: exchangeName,
//                type: exchangeType,
//                durable: true,
//                autoDelete: false);
//        }

//        public void Dispose()
//        {
//            Channel?.Dispose();
//            _connection?.Dispose();
//        }
//    }
//}




//using Microsoft.Extensions.Configuration;
//using RabbitMQ.Client;
//using System;

//namespace EventBus.Implementations
//{
//    public sealed class RabbitMQConnection : IDisposable
//    {
//        private readonly IConnection _connection;
//        public IModel Channel { get; } 

//        public RabbitMQConnection(IConfiguration config)
//        {
//            var factory = new ConnectionFactory
//            {
//                HostName = config["RabbitMQ:Host"],
//                UserName = config["RabbitMQ:Username"],
//                Password = config["RabbitMQ:Password"],
//                DispatchConsumersAsync = true
//            };

//            _connection = factory.CreateConnection();
//            Channel = _connection.CreateModel(); 
//        }

//        public void DeclareExchange(string exchangeName, string exchangeType = ExchangeType.Direct)
//        {
//            Channel.ExchangeDeclare(
//                exchange: exchangeName,
//                type: exchangeType,
//                durable: true,
//                autoDelete: false);
//        }

//        public void Dispose()
//        {
//            Channel?.Dispose();
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
            var factory = new ConnectionFactory
            {
                HostName = config["RabbitMQ:Host"],
                UserName = config["RabbitMQ:Username"],
                Password = config["RabbitMQ:Password"],
                DispatchConsumersAsync = true
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
