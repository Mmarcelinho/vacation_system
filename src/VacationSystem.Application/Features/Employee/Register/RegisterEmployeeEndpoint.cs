using Carter;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace VacationSystem.Application.Features.Employee.Register;

public record RegisterEmployeeRequest(string FirstName, string LastName, string Email, string Password);
public record RegisterEmployeeResponse(Guid Id);

public class RegisterEmployeeEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("/employees", async (RegisterEmployeeRequest request, ISender sender, CancellationToken cancellationToken) =>
        {
            var result = await sender.Send(new RegisterEmployeeCommand(request.FirstName, request.LastName, request.Email, request.Password), cancellationToken);

            var response = new RegisterEmployeeResponse(result.Id);

            return Results.Created($"/employees/{response.Id}", response);
        })
        .WithName("RegisterEmployee")
        .Produces<RegisterEmployeeResponse>(StatusCodes.Status201Created)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .WithSummary("Register Employee")
        .WithDescription("Register Employee");
    }
}
