using System.Reflection;
using Microsoft.EntityFrameworkCore;
using SharedKernel.Abstractions;
using VacationSystem.Application.Domain.Admin;
using VacationSystem.Application.Domain.Employee;
using VacationSystem.Application.Domain.Vacation;

namespace VacationSystem.Application.Infrastructure.Persistence;

public class VacationSystemDbContext(DbContextOptions<VacationSystemDbContext> options) : DbContext(options)
{
    public DbSet<User> Users { get; set; } = null!;

    public DbSet<Admin> Admins { get; set; } = null!;

    public DbSet<Employee> Employees { get; set; } = null!;

    public DbSet<VacationRequest> VacationRequests { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

        base.OnModelCreating(builder);
    }
}