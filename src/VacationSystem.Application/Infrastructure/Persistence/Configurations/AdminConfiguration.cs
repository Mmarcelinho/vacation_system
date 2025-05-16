using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VacationSystem.Application.Domain.Admin;

namespace VacationSystem.Application.Infrastructure.Persistence.Configurations;

public class AdminConfiguration : IEntityTypeConfiguration<Admin>
{
    public void Configure(EntityTypeBuilder<Admin> builder)
    {
        builder.Property(admin => admin.Position)
            .HasColumnName("Position")
            .HasMaxLength(255)
            .IsRequired();

        builder.Property(admin => admin.DepartmentId)
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