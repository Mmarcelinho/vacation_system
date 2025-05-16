using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VacationSystem.Application.Domain.Employee;

namespace VacationSystem.Application.Infrastructure.Persistence.Configurations;

public class EmployeeConfiguration : IEntityTypeConfiguration<Employee>
{
    public void Configure(EntityTypeBuilder<Employee> builder)
    {
        builder.Property(employee => employee.StartDate)
            .HasColumnName("StartDate")
            .HasColumnType("datetime")
            .IsRequired();

        builder.Property(employee => employee.LastVacationDate)
            .HasColumnName("LastVacationDate")
            .HasColumnType("datetime");

        builder.Property(employee => employee.DepartmentId)
            .HasColumnName("DepartmentId")
            .IsRequired();

        builder.OwnsOne(user => user.Name, navigationBuilder =>
        {
            navigationBuilder.Property(name => name.Firstname)
                .HasColumnName("FirstName")
                .HasMaxLength(50)
                .IsRequired();

            navigationBuilder.Property(name => name.Lastname)
                .HasColumnName("LastName")
                .HasMaxLength(50)
                .IsRequired();
        });
    }
}