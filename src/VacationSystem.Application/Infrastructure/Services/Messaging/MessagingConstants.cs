namespace VacationSystem.Application.Infrastructure.Services.Messaging;

public static class MessagingConstants
{
    // Exchanges
    public const string VacationExchange = "vacation_exchange";

    // Routing Keys
    public const string VacationRequestCreatedRoutingKey = "vacation.request.created";
    public const string VacationRequestApprovedRoutingKey = "vacation.request.approved";
    public const string VacationRequestDeniedRoutingKey = "vacation.request.denied";

    // Queues
    public const string VacationRequestCreatedQueue = "vacation_request_created_queue";
    public const string VacationRequestApprovedQueue = "vacation_request_approved_queue";
    public const string VacationRequestDeniedQueue = "vacation_request_denied_queue";
}