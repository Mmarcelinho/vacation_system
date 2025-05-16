using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VacationSystem.Application.Domain.Vacation;

namespace VacationSystem.Application.Infrastructure.Persistence.Configurations;

public class VacationRequestConfiguration : IEntityTypeConfiguration<VacationRequest>
{
    public void Configure(EntityTypeBuilder<VacationRequest> builder)
    {
        builder.ToTable("VacationRequests");

        builder.HasKey(vacationRequest => vacationRequest.Id);

        builder.Property(vacationRequest => vacationRequest.Id)
            .HasColumnName("Id")
            .IsRequired();

        builder.Property(vacationRequest => vacationRequest.RequestDate)
            .HasColumnName("RequestDate")
            .HasColumnType("datetime")
            .IsRequired();

        builder.Property(vacationRequest => vacationRequest.StartDate)
            .HasColumnName("StartDate")
            .HasColumnType("datetime")
            .IsRequired();

        builder.Property(vacationRequest => vacationRequest.EndDate)
            .HasColumnName("EndDate")
            .HasColumnType("datetime")
            .IsRequired();

        builder.Property(vacationRequest => vacationRequest.Days)
            .HasColumnName("Days")
            .IsRequired();

        builder.Property(vacationRequest => vacationRequest.Status)
            .HasColumnName("Status")
            .HasConversion<int>()
            .IsRequired();

        builder.HasOne(vacationRequest => vacationRequest.Employee)
            .WithMany(e => e.VacationRequests)
            .HasForeignKey(vacationRequest => vacationRequest.EmployeeId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(vacationRequest => vacationRequest.Admin)
            .WithMany(a => a.VacationRequests)
            .HasForeignKey(vacationRequest => vacationRequest.AdminId)
            .IsRequired(false)
            .OnDelete(DeleteBehavior.Restrict);
    }
}