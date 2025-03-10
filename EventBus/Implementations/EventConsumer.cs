using EventBus.Abstractions;
using Microsoft.Extensions.Hosting;
using RabbitMQ.Client.Events;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace EventBus.Implementations
{
    public  class EventConsumer : BackgroundService, IEventConsumer
    {
        private readonly RabbitMQConnection _connection;
        private readonly string _exchangeName;

        public EventConsumer(RabbitMQConnection connection, string exchangeName = "labourlink.events")
        {
            _connection = connection;
            _exchangeName = exchangeName;
        }

        public void Subscribe<TEvent>(string queueName, Action<TEvent> handler) where TEvent : class
        {
            var routingKey = typeof(TEvent).Name;

            _connection.Channel.QueueDeclare(
                queue: queueName,
                durable: true,
                exclusive: false,
                autoDelete: false);

            _connection.Channel.QueueBind(
                queue: queueName,
                exchange: _exchangeName,
                routingKey: routingKey);

            var consumer = new AsyncEventingBasicConsumer(_connection.Channel);
            consumer.Received += async (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var message = JsonConvert.DeserializeObject<TEvent>(Encoding.UTF8.GetString(body));
                handler(message);
                _connection.Channel.BasicAck(ea.DeliveryTag, false);
            };

            _connection.Channel.BasicConsume(
                queue: queueName,
                autoAck: false,
                consumer: consumer);
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                await Task.Delay(1000, stoppingToken);
            }
        }

    }
}
