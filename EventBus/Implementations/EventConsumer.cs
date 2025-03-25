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
    //public  class EventConsumer : BackgroundService, IEventConsumer
    //{
    //    private readonly RabbitMQConnection _connection;
    //    private readonly string _exchangeName;

    //    public EventConsumer(RabbitMQConnection connection, string exchangeName = "labourlink.events")
    //    {
    //        _connection = connection;
    //        _exchangeName = exchangeName;
    //    }

    //    public void Subscribe<TEvent>(string queueName, Action<TEvent> handler) where TEvent : class
    //    {
    //        var routingKey = typeof(TEvent).Name;

    //        _connection.Channel.QueueDeclare(
    //            queue: queueName,
    //            durable: true,
    //            exclusive: false,
    //            autoDelete: false);

    //        _connection.Channel.QueueBind(
    //            queue: queueName,
    //            exchange: _exchangeName,
    //            routingKey: routingKey);

    //        var consumer = new AsyncEventingBasicConsumer(_connection.Channel);
    //        consumer.Received += async (model, ea) =>
    //        {
    //            var body = ea.Body.ToArray();
    //            var message = JsonConvert.DeserializeObject<TEvent>(Encoding.UTF8.GetString(body));
    //            handler(message);
    //            _connection.Channel.BasicAck(ea.DeliveryTag, false);
    //        };

    //        _connection.Channel.BasicConsume(
    //            queue: queueName,
    //            autoAck: false,
    //            consumer: consumer);
    //    }

    //    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    //    {
    //        while (!stoppingToken.IsCancellationRequested)
    //        {
    //            await Task.Delay(1000, stoppingToken);
    //        }
    //    }

    //}



    //public class EventConsumer : BackgroundService, IEventConsumer
    //{
    //    private readonly RabbitMQConnection _connection;
    //    private readonly string _exchangeName;

    //    public EventConsumer(RabbitMQConnection connection, string exchangeName = "labourlink.events")
    //    {
    //        _connection = connection;
    //        _exchangeName = exchangeName;
    //    }

    //    public void Subscribe<TEvent>(string queueName, Action<TEvent> handler) where TEvent : class
    //    {
    //        var routingKey = typeof(TEvent).Name;

    //        using var channel = _connection.CreateChannel(); 
    //        channel.QueueDeclare(
    //            queue: queueName,
    //            durable: true,
    //            exclusive: false,
    //            autoDelete: false);

    //        channel.QueueBind(
    //            queue: queueName,
    //            exchange: _exchangeName,
    //            routingKey: routingKey);

    //        var consumer = new AsyncEventingBasicConsumer(channel);
    //        consumer.Received += async (model, ea) =>
    //        {
    //            var body = ea.Body.ToArray();
    //            var message = JsonConvert.DeserializeObject<TEvent>(Encoding.UTF8.GetString(body));
    //            handler(message);

    //            channel.BasicAck(ea.DeliveryTag, false);
    //        };

    //        channel.BasicConsume(
    //            queue: queueName,
    //            autoAck: false,
    //            consumer: consumer);
    //    }

    //    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    //    {
    //        while (!stoppingToken.IsCancellationRequested)
    //        {
    //            await Task.Delay(1000, stoppingToken);
    //        }
    //    }
    //}

























    public class EventConsumer : BackgroundService, IEventConsumer
    {
        private readonly RabbitMQConnection _connection;
        private readonly string _exchangeName;
        private IModel _channel;
        private readonly Dictionary<string, Func<string, Task>> _handlers = new();

        public EventConsumer(RabbitMQConnection connection, string exchangeName = "labourlink.events")
        {
            _connection = connection;
            _exchangeName = exchangeName;
            _channel = _connection.CreateChannel();
        }

        public void Subscribe<TEvent>(string queueName, Action<TEvent> handler) where TEvent : class
        {
            var routingKey = typeof(TEvent).Name;
            _channel.QueueDeclare(queue: queueName, durable: true, exclusive: false, autoDelete: false);
            _channel.QueueBind(queue: queueName, exchange: _exchangeName, routingKey: routingKey);

            _handlers[queueName] = (message) =>
            {
                var @event = JsonConvert.DeserializeObject<TEvent>(message);
                if (@event != null)
                {
                    handler(@event);
                }
                return Task.CompletedTask;
            };
        }


        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            foreach (var queueName in _handlers.Keys)
            {
                var consumer = new AsyncEventingBasicConsumer(_channel);
                consumer.Received += async (model, ea) =>
                {
                    var body = Encoding.UTF8.GetString(ea.Body.ToArray());
                    if (_handlers.TryGetValue(queueName, out var handler))
                    {
                        await handler(body);
                    }

                    _channel.BasicAck(ea.DeliveryTag, false);
                };

                _channel.BasicConsume(queue: queueName, autoAck: false, consumer: consumer);
            }

            while (!stoppingToken.IsCancellationRequested)
            {
                await Task.Delay(1000, stoppingToken);
            }
        }
    }

}
