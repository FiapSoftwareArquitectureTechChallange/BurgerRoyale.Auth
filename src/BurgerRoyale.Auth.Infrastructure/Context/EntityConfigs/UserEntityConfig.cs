using BurgerRoyale.Auth.Domain.Entities;
using BurgerRoyale.Auth.Domain.Enumerators;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Diagnostics.CodeAnalysis;

namespace BurgerRoyale.Auth.Infrastructure.Context.EntityConfigs
{
    [ExcludeFromCodeCoverage]
    public class UserEntityConfig : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder
                .HasKey(x => x.Id);

            builder
                .Property(x => x.Id)
                .ValueGeneratedOnAdd();

            builder.Property(x => x.Name)
                .IsRequired();

            builder.Property(x => x.Cpf)
                .IsRequired()
                .HasMaxLength(15);

            builder.Property(x => x.Email)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(x => x.PasswordHash)
                .IsRequired();

            builder.HasData(
                new User(
                    "00000000000",
                    "Admin",
                    "admin@burgerroyale.com",
                    "$2a$11$Hm3GUkwCnSTCFwqT1ntowe/C/rvm2lery.SP3tUVe0.qdMyknR5PG",
                    UserRole.Admin
                )
            );
        }
    }
}
