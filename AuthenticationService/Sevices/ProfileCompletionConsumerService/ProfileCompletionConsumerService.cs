

using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using Newtonsoft.Json;
using AuthenticationService.Repositories;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using LabourlinkApi.Events;

namespace AuthenticationService.Services.ProfileCompletionConsumerService
{
    public class ProfileCompletionConsumerService : BackgroundService  //Runs as a long-running background task in ASP.NET Core.
    {
        private readonly IConnection _connection;
        private readonly IModel _channel;
        private readonly IServiceProvider _services;
        private const string QueueName = "auth-profile-completed";
        private const string ExchangeName = "labourlink.events";
        private const string RoutingKey = "profile.completed";
        private bool _disposed;

        public ProfileCompletionConsumerService(IConfiguration config, IServiceProvider services)
        {
            _services = services;
            var factory = new ConnectionFactory
            {
                HostName = config["RABBITMQ-HOST"],
                UserName = config["RABBITMQ-USERNAME"],
                Password = config["RABBITMQ-PASSWORD"],
                DispatchConsumersAsync = true // Enable async processing
            };

            _connection = factory.CreateConnection();
            _channel = _connection.CreateModel();

            // Declare the exchange (same as publisher)
            _channel.ExchangeDeclare(
                exchange: ExchangeName,
                type: ExchangeType.Direct,
                durable: true,
                autoDelete: false,
                arguments: null
            );

            // Declare the queue with proper arguments
            _channel.QueueDeclare(
                queue: QueueName,
                durable: true,    // Match exchange durability
                exclusive: false,
                autoDelete: false,
                arguments: null
            );

            // Bind queue to exchange
            _channel.QueueBind(
                queue: QueueName,
                exchange: ExchangeName,
                routingKey: RoutingKey
            );
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var consumer = new AsyncEventingBasicConsumer(_channel);
            consumer.Received += async (model, ea) =>
            {
                try
                {
                    var body = ea.Body.ToArray();
                    var message = JsonConvert.DeserializeObject<ProfileCompletedEvent>(
                        Encoding.UTF8.GetString(body)
                    );

                    using var scope = _services.CreateScope();
                    var authRepo = scope.ServiceProvider.GetRequiredService<IAuthRepository>();
                    await authRepo.MarkProfileAsCompleted(message.UserId);

                    // Manual acknowledgment
                    _channel.BasicAck(ea.DeliveryTag, false);
                }
                catch (Exception ex)
                {
                    // Handle error (e.g., log and requeue)
                    Console.WriteLine($"Error processing message: {ex.Message}");
                    _channel.BasicNack(ea.DeliveryTag, false, true);
                }
            };

            // Start consuming with manual ack
            _channel.BasicConsume(
                queue: QueueName,
                autoAck: false,  // Crucial for reliability , Ensures messages are not lost if an error occurs
                consumer: consumer
            );

            return Task.CompletedTask;
        }

        public override void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
            base.Dispose();
        }

        protected virtual void Dispose(bool disposing)
        {
            if (_disposed) return;

            if (disposing)
            {
                _channel?.Dispose();  // Proper resource cleanup
                _connection?.Dispose();
            }
            _disposed = true;
        }
    }
}

