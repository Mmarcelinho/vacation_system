namespace VacationSystem.Application.Infrastructure.Services.Messaging;

public interface IRabbitMQService
{
    void Publish<T>(string exchange, string routingKey, T message) where T : class;
    void Subscribe<T>(string queueName, string exchange, string routingKey, Action<T> onMessage) where T : class;
}
