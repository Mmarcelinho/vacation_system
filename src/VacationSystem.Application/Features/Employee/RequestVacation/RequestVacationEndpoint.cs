using Carter;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace VacationSystem.Application.Features.Employee.RequestVacation;

public record VacationRequest(DateTime StartDate, int Days);
public record RequestVacationResponse(Guid Id);

public class RequestVacationEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("/employees/vacation-requests", async (VacationRequest request, ISender sender, CancellationToken cancellationToken) =>
        {
            var result = await sender.Send(new RequestVacationCommand(request.StartDate, request.Days), cancellationToken);

            var response = new RequestVacationResponse(result.Id);

            return Results.Created($"/employees/vacation-requests/{response.Id}", response);
        })
        .WithName("RequestVacation")
        .Produces<RequestVacationResponse>(StatusCodes.Status201Created)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .WithSummary("Request Vacation")
        .WithDescription("Request Vacation");
    }
}
