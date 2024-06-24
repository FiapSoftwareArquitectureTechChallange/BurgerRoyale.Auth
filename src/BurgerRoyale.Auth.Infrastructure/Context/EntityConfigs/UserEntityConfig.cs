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

            builder.Property(x => x.Phone)
                .HasMaxLength(15);

			builder.Property(x => x.Address);

            builder.Property(x => x.PasswordHash)
				.IsRequired();

			builder.HasData(
				new User(
					"00000000000",
					"Admin",
					"admin@burgerroyale.com",
					string.Empty,
                    string.Empty,
                    "$2a$11$C99.K9/gfTc0RqR8XYAiu.T3BG/GvWgOt2oggKkyivz9dGpZPwpEy",
					UserRole.Admin
				),
				new User(
					"11111111111",
					"Customer",
					"customer@burgerroyale.com",
                    string.Empty,
                    string.Empty,
                    "$2a$11$mDJa/xLGCCAYzxhDpcmYve8NpaBqMeCMMfVbB9NpGqg/SpaRv3gJq",
					UserRole.Customer
				),
				new User(
					"22222222222",
					"Employee",
					"employee@burgerroyale.com",
                    string.Empty,
                    string.Empty,
                    "$2a$11$hIwenwL9SKoqgwWt0EOQgukwjCDP1tVuij0lMtHk9nGSwnIVbzbM2",
					UserRole.Employee
				)
			);
		}
	}
}
