using Carter;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using VacationSystem.Application.Domain.Vacation.Enums;

namespace VacationSystem.Application.Features.Admin.ReviewVacation;

public record ReviewVacationRequest(EStatus Status);

public class ReviewVacationRequestEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPut("/admin/vacation-requests/{id}/review", async (ReviewVacationRequest command, Guid id, ISender sender, CancellationToken cancellationToken) =>
        {
            await sender.Send(new ReviewVacationRequestCommand(id, command.Status), cancellationToken);

            return Results.NoContent();
        })
        .WithName("ReviewVacation")
        .Produces(StatusCodes.Status204NoContent)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .WithSummary("Review Vacation")
        .WithDescription("Review Vacation");
    }
}
