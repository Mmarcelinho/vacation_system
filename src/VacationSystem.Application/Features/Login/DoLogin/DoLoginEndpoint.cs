using Carter;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace VacationSystem.Application.Features.Login.DoLogin;

public record DoLoginRequest(string Email, string Password);
public record DoLoginResponse(string Token);

public class DoLoginEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("/login", async (DoLoginRequest request, ISender sender, CancellationToken cancellationToken) =>
        {
            var result = await sender.Send(new DoLoginCommand(request.Email, request.Password), cancellationToken);

            var response = new DoLoginResponse(result.Token);

            return Results.Created("/login", response);
        })
        .WithName("DoLogin")
        .Produces<DoLoginResponse>(StatusCodes.Status201Created)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .WithSummary("Do Login")
        .WithDescription("Do Login");
    }
}
