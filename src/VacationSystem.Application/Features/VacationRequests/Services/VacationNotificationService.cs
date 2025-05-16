using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using VacationSystem.Application.Domain.Vacation.Events;
using VacationSystem.Application.Infrastructure.Services.Messaging;

namespace VacationSystem.Application.Features.VacationRequests.Services;

public class VacationNotificationService : BackgroundService
{
    private readonly IRabbitMQService _rabbitMQService;
    private readonly ILogger<VacationNotificationService> _logger;

    public VacationNotificationService(
        IRabbitMQService rabbitMQService,
        ILogger<VacationNotificationService> logger)
    {
        _rabbitMQService = rabbitMQService;
        _logger = logger;
    }

    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        SetupSubscriptions();
        return Task.CompletedTask;
    }

    private void SetupSubscriptions()
    {
        _rabbitMQService.Subscribe<VacationRequestCreatedEvent>(
            MessagingConstants.VacationRequestCreatedQueue,
            MessagingConstants.VacationExchange,
            MessagingConstants.VacationRequestCreatedRoutingKey,
            HandleVacationRequestCreated);

        _rabbitMQService.Subscribe<VacationRequestReviewedEvent>(
            MessagingConstants.VacationRequestApprovedQueue,
            MessagingConstants.VacationExchange,
            MessagingConstants.VacationRequestApprovedRoutingKey,
            HandleVacationRequestApproved);

        _rabbitMQService.Subscribe<VacationRequestReviewedEvent>(
            MessagingConstants.VacationRequestDeniedQueue,
            MessagingConstants.VacationExchange,
            MessagingConstants.VacationRequestDeniedRoutingKey,
            HandleVacationRequestDenied);
    }

    private void HandleVacationRequestCreated(VacationRequestCreatedEvent @event)
    {
        _logger.LogInformation(
            "Nova solicitação de férias criada: {VacationRequestId} - Funcionário: {EmployeeId} - " +
            "Período: {StartDate:dd/MM/yyyy} a {EndDate:dd/MM/yyyy} ({Days} dias)",
            @event.VacationRequestId,
            @event.EmployeeId,
            @event.StartDate,
            @event.EndDate,
            @event.Days);
    }

    private void HandleVacationRequestApproved(VacationRequestReviewedEvent @event)
    {
        _logger.LogInformation(
            "Solicitação de férias aprovada: {VacationRequestId} - Aprovada pelo Admin: {AdminId}",
            @event.VacationRequestId,
            @event.AdminId);
    }

    private void HandleVacationRequestDenied(VacationRequestReviewedEvent @event)
    {
        _logger.LogInformation(
            "Solicitação de férias negada: {VacationRequestId} - Negada pelo Admin: {AdminId}",
            @event.VacationRequestId,
            @event.AdminId);
    }
}