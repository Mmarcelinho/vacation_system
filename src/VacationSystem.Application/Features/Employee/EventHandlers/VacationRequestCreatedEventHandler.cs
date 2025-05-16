using MediatR;
using Microsoft.Extensions.Logging;
using VacationSystem.Application.Domain.Vacation.Events;
using VacationSystem.Application.Infrastructure.Services.Messaging;

namespace VacationSystem.Application.Features.Employee.EventHandlers;

public class VacationRequestCreatedEventHandler : INotificationHandler<VacationRequestCreatedEvent>
{
    private readonly IRabbitMQService _messaging;
    private readonly ILogger<VacationRequestCreatedEventHandler> _logger;

    public VacationRequestCreatedEventHandler(
        IRabbitMQService messaging, ILogger<VacationRequestCreatedEventHandler> logger)
    {
        _messaging = messaging;
        _logger = logger;
    }

    public Task Handle(VacationRequestCreatedEvent notification, CancellationToken cancellationToken)
    {
        try
        {
            _logger.LogInformation("Publicando evento de solicitação de férias criada: {VacationRequestId}",
                notification.VacationRequestId);

            _messaging.Publish<VacationRequestCreatedEvent>(
                MessagingConstants.VacationExchange,
                MessagingConstants.VacationRequestCreatedRoutingKey,
                notification);

            return Task.CompletedTask;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao publicar evento de solicitação de férias criada: {VacationRequestId}",
                notification.VacationRequestId);
            throw;
        }
    }
}
