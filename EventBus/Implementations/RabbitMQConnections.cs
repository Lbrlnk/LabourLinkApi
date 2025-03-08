//using Microsoft.Extensions.Configuration;
//using RabbitMQ.Client;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;


//"Host": "localhost",
//    "Username": "admin",
//    "Password": "admin123"

//namespace EventBus.Implementations
//{




//public sealed class RabbitMQConnection : IDisposable
//{
//    private readonly IConnection _connection;
//    public IModel Channel { get; }

//    //public RabbitMQConnection(IConfiguration config)
//    ////public RabbitMQConnection()
//    //{
//    //    var factory = new ConnectionFactory
//    //    {
//    //        HostName = config["RabbitMQ:Host"],
//    //        UserName = config["RabbitMQ:Username"],
//    //        Password = config["RabbitMQ:Password"],
//    //        //HostName = "localhost",
//    //        //UserName = "admin",
//    //        //Password = "admin123",
//    //        DispatchConsumersAsync = true
//    //    };

//    //    _connection = factory.CreateConnection();
//    //    Channel = _connection.CreateModel();

//    //    // Declare default exchange
//    //    Channel.ExchangeDeclare(
//    //        exchange: "labourlink.events",
//    //        type: ExchangeType.Direct,
//    //        durable: true,
//    //        autoDelete: false);
//    //}

//    public RabbitMQConnection(IConfiguration config)
//    {
//        var factory = new ConnectionFactory
//        {
//            HostName = config["RabbitMQ:Host"],
//            UserName = config["RabbitMQ:Username"],
//            Password = config["RabbitMQ:Password"],
//            DispatchConsumersAsync = true
//        };

//        _connection = factory.CreateConnection();
//        Channel = _connection.CreateModel();
//    }


//    public void DeclareExchange(string exchangeName, string exchangeType = ExchangeType.Direct)
//    {
//        Channel.ExchangeDeclare(
//            exchange: exchangeName,
//            type: exchangeType,
//            durable: true,
//            autoDelete: false);
//    }


//    public void DeclareExchanges(IEnumerable<(string Name, string Type)> exchanges)
//    {
//        foreach (var exchange in exchanges)
//        {
//            DeclareExchange(exchange.Name, exchange.Type);
//        }
//    }

//    public void Dispose()
//    {
//        Channel?.Dispose();
//        _connection?.Dispose();
//    }
//}
//}
using Microsoft.Extensions.Configuration;
using RabbitMQ.Client;
using System;

namespace EventBus.Implementations
{
    public sealed class RabbitMQConnection : IDisposable
    {
        private readonly IConnection _connection;
        public IModel Channel { get; }

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
            Channel = _connection.CreateModel();
        }

        public void DeclareExchange(string exchangeName, string exchangeType = ExchangeType.Direct)
        {
            Channel.ExchangeDeclare(
                exchange: exchangeName,
                type: exchangeType,
                durable: true,
                autoDelete: false);
        }

        public void Dispose()
        {
            Channel?.Dispose();
            _connection?.Dispose();
        }
    }
}