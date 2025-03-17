namespace ProfileService.Services.RabbitMQ
{
    public interface IRabbitMqService
    {
        void PublishProfileCompleted(Guid userId);
    }
}
