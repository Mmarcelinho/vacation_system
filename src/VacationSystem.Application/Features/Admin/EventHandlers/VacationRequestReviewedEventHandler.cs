using Microsoft.Extensions.Logging;
using SharedKernel.Abstractions;
using VacationSystem.Application.Domain.Vacation.Enums;
using VacationSystem.Application.Domain.Vacation.Events;
using VacationSystem.Application.Infrastructure.Services.Messaging;

namespace VacationSystem.Application.Features.Admin.EventHandlers;

public class VacationRequestReviewedEventHandler : IDomainEventHandler<VacationRequestReviewedEvent>
{
    private readonly IRabbitMQService _rabbitMQService;
    private readonly ILogger<VacationRequestReviewedEventHandler> _logger;

    public VacationRequestReviewedEventHandler(
        IRabbitMQService rabbitMQService,
        ILogger<VacationRequestReviewedEventHandler> logger)
    {
        _rabbitMQService = rabbitMQService;
        _logger = logger;
    }

    public Task Handle(VacationRequestReviewedEvent notification, CancellationToken cancellationToken)
    {
        try
        {
            var routingKey = notification.Status == EStatus.Approved
                ? MessagingConstants.VacationRequestApprovedRoutingKey
                : MessagingConstants.VacationRequestDeniedRoutingKey;

            _logger.LogInformation(
                "Publicando evento de solicitação de férias {Status}: {VacationRequestId}",
                notification.Status,
                notification.VacationRequestId);

            _rabbitMQService.Publish<VacationRequestReviewedEvent>(
                MessagingConstants.VacationExchange,
                routingKey,
                notification);

            return Task.CompletedTask;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex,
                "Erro ao publicar evento de solicitação de férias {Status}: {VacationRequestId}",
                notification.Status,
                notification.VacationRequestId);
            throw;
        }
    }
}