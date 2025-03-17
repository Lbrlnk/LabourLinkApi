using EventBus.Abstractions;
using Newtonsoft.Json;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

//namespace EventBus.Implementations
//{
//    public  class EventPublisher : IEventPublisher, IDisposable
//    {
//        private readonly RabbitMQConnection _connection;
//        private readonly string _exchangeName;

//        public EventPublisher(RabbitMQConnection connection, string exchangeName = "labourlink.events")
//        {
//            _connection = connection;
//            _exchangeName = exchangeName;

//        }

//        public void Publish<TEvent>(TEvent @event) where TEvent : class
//        {
//            var routingKey = typeof(TEvent).Name;
//            var body = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(@event));

//            _connection.Channel.BasicPublish(
//                exchange: _exchangeName,
//                routingKey: routingKey,
//                basicProperties: null,
//                body: body);
//        }

//        public void Dispose() => _connection.Dispose();

//    }
//}



namespace EventBus.Implementations
{
    public class EventPublisher : IEventPublisher, IDisposable
    {
        private readonly RabbitMQConnection _connection;
        private readonly string _exchangeName;

        public EventPublisher(RabbitMQConnection connection, string exchangeName = "labourlink.events")
        {
            _connection = connection;
            _exchangeName = exchangeName;
        }

        public void Publish<TEvent>(TEvent @event) where TEvent : class
        {
            var routingKey = typeof(TEvent).Name;
            var body = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(@event));

            using var channel = _connection.CreateChannel(); // 🔥 Create a new channel for each operation
            channel.BasicPublish(
                exchange: _exchangeName,
                routingKey: routingKey,
                basicProperties: null,
                body: body);
        }

        public void Dispose() => _connection.Dispose();
    }
}
