using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using VacationSystem.Application.Common.Configuration;

namespace VacationSystem.Application.Infrastructure.Services.Messaging;

public class RabbitMQService : IRabbitMQService, IDisposable
{
    private readonly IConnection _connection;
    private readonly IModel _channel;
    private readonly ILogger<RabbitMQService> _logger;
    private readonly RabbitMQSettings _settings;

    public RabbitMQService(IOptions<RabbitMQSettings> settings, ILogger<RabbitMQService> logger)
    {
        _logger = logger;
        _settings = settings.Value;
        try
        {
            var factory = new ConnectionFactory
            {
                HostName = _settings.HostName,
                Port = _settings.Port,
                UserName = _settings.UserName,
                Password = _settings.Password
            };

            _connection = factory.CreateConnection();
            _channel = _connection.CreateModel();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to establish RabbitMQ connection");
            throw;
        }
    }
    public void Publish<T>(string exchange, string routingKey, T message) where T : class
    {
        try
        {
            _channel.ExchangeDeclare(exchange, ExchangeType.Direct, durable: true);

            var json = JsonSerializer.Serialize(message);
            var body = Encoding.UTF8.GetBytes(json);

            _channel.BasicPublish(
                exchange: exchange,
                routingKey: routingKey,
                basicProperties: null,
                body: body);

            _logger.LogInformation("Message published to {Exchange} with routing key {RoutingKey}", exchange, routingKey);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to publish message to {Exchange} with routing key {RoutingKey}", exchange, routingKey);
            throw;
        }
    }

    public void Subscribe<T>(string queue, string exchange, string routingKey, Action<T> onMessage) where T : class
    {
        try
        {
            _channel.ExchangeDeclare(exchange, ExchangeType.Direct, durable: true);
            _channel.QueueDeclare(queue, durable: true, exclusive: false, autoDelete: false);
            _channel.QueueBind(queue, exchange, routingKey);

            var consumer = new EventingBasicConsumer(_channel);

            consumer.Received += (_, ea) =>
            {
                try
                {
                    var body = ea.Body.ToArray();
                    var message = Encoding.UTF8.GetString(body);
                    var deserializedMessage = JsonSerializer.Deserialize<T>(message);

                    if (deserializedMessage != null)
                    {
                        onMessage(deserializedMessage);
                        _channel.BasicAck(ea.DeliveryTag, false);
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error processing message from queue {Queue}", queue);
                    _channel.BasicNack(ea.DeliveryTag, multiple: false, requeue: true);
                }
            };

            _channel.BasicConsume(queue, autoAck: false, consumer);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to subscribe to {Queue} bound to {Exchange} with routing key {RoutingKey}", queue, exchange, routingKey);
            throw;
        }
    }

    public void Dispose()
    {
        _channel?.Dispose();
        _connection?.Dispose();
    }
}
