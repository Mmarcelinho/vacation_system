namespace VacationSystem.Application.Common.Configuration;

public class RabbitMQSettings
{
    public string HostName { get; set; } = string.Empty;
    public int Port { get; set; }
    public string UserName { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public string VirtualHost { get; set; } = string.Empty;
    public string ExchangeName { get; set; } = string.Empty;
    public string QueueName { get; set; } = string.Empty;
}