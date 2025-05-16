using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SharedKernel.Abstractions;
using SharedKernel.Constants;

namespace VacationSystem.Application.Infrastructure.Persistence.Configurations;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable("Users");

        builder.HasKey(user => user.Id);

        builder.Property(user => user.Id)
            .HasColumnName("Id")
            .IsRequired();

        builder.Property(user => user.DateCreatedUtc)
        .HasColumnName("CreatedAt")
        .HasColumnType("datetime")
        .IsRequired();

        builder.HasDiscriminator<string>("Role")
        .HasValue<Domain.Admin.Admin>(Roles.Admin)
        .HasValue<Domain.Employee.Employee>(Roles.Employee);

        builder.OwnsOne(user => user.Email, navigationBuilder =>
        {
            navigationBuilder.Property(email => email.Value)
                .HasColumnName("Email")
                .HasMaxLength(255)
                .IsRequired();

            navigationBuilder.HasIndex(e => e.Value).IsUnique();
        });

        builder.OwnsOne(user => user.Password, navigationBuilder =>
        {
            navigationBuilder.Property(password => password.Value)
            .HasColumnName("Password")
            .HasMaxLength(2000)
            .IsRequired();
        });

        builder.Property(user => user.Role)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(user => user.Active)
            .HasColumnName("Active")
            .IsRequired();

        builder.Property(user => user.LastLogin)
            .HasColumnName("UpdatedAt")
            .HasColumnType("datetime");
    }
}
