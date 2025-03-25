
using Newtonsoft.Json;
using RabbitMQ.Client;
using System.Text;
using LabourlinkApi.Events;



namespace ProfileService.Services.RabbitMQ
{
    public class RabbitMqService : IRabbitMqService, IDisposable
    {
        private readonly IConnection _connection;
        private readonly IModel _channel;
        private const string ExchangeName = "labourlink.events";
        private const string RoutingKey = "profile.completed";
        private bool _disposed;






        public RabbitMqService(IConfiguration config)
        {
            var factory = new ConnectionFactory                  // connection setup
            {
                HostName = config["RABBITMQ-HOST"],
                UserName = config["RABBITMQ-USERNAME"],
                Password = config["RABBITMQ-PASSWORD"],
                DispatchConsumersAsync = true // Enable async consumer support
            };

            _connection = factory.CreateConnection(); // establish a channel to send messages 
            _channel = _connection.CreateModel(); // create a channel to send messages 



            // Declare durable exchange (idempotent operation) , Declaring an exchange in RabbitMQ means creating a routing system where messages are sent before reaching a queue. Exchanges receive messages from producers and route them to queues based on certain rules.
            // In RabbitMQ, messages cannot go directly from a producer to a queue.Instead:
            _channel.ExchangeDeclare(
                exchange: ExchangeName,
                type: ExchangeType.Direct,   //Direct exchange type (routes messages based on a specific routing key).
                durable: true, // Ensures the exchange persists even if RabbitMQ restarts.
                autoDelete: false,  // The exchange will not be deleted automatically when no queues are using it.
                arguments: null // No additional configurations.
            );
        }




        public void PublishProfileCompleted(Guid userId)
        {
            try
            {
                var message = new ProfileCompletedEvent { UserId = userId };
                var body = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(message));

                // Ensure channel is still open
                if (_channel.IsOpen)
                {
                    _channel.BasicPublish(
                        exchange: ExchangeName,
                        routingKey: RoutingKey,
                        basicProperties: null,
                        body: body
                    );
                }
            }
            catch (Exception ex)
            {
                // Add logging here
                Console.WriteLine($"Failed to publish message: {ex.Message}");
                throw;
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (_disposed) return;

            if (disposing)
            {
                _channel?.Dispose(); // Use Dispose() instead of Close()
                _connection?.Dispose();
            }

            _disposed = true;
        }


    }
}
