using Bogus;
using Bogus.Extensions.Brazil;
using BurgerRoyale.Auth.Domain.DTO;
using BurgerRoyale.Auth.Domain.Entities;
using BurgerRoyale.Auth.Domain.Enumerators;
using BC = BCrypt.Net.BCrypt;

namespace BurgerRoyale.Auth.UnitTests.Domain.EntitiesMocks
{
    public static class UserMock
    {
        public static User Get
        (
            string? cpf = null,
            string? name = null,
            string? email = null,
            string? phone = null,
            string? address = null,
            string? password = null,
            UserRole? userRole = null
        )
        {
            return UserFakerInstantiator(cpf, name, email, phone, address, password, userRole)
                .Generate();
        }

        public static List<User> GetList
        (
            int? quantity = null,
            UserRole? userRole = null
        )
        {
            return UserFakerInstantiator(userRole: userRole)
                .Generate(quantity ?? 3);
        }

        public static List<UserDTO> GetDtoList
        (
            int? quantity = null,
            UserRole? userRole = null
        )
        {
            return GetList(quantity, userRole)
                .Select(user => user.AsDto())
                .ToList();
        }

        private static Faker<User> UserFakerInstantiator
        (
            string? cpf = null,
            string? name = null,
            string? email = null,
            string? phone = null,
            string? address = null,
            string? password = null,
            UserRole? userRole = null
        )
        {
            return new Faker<User>()
                .CustomInstantiator(faker => new User(
                    cpf ?? faker.Person.Cpf(),
                    name ?? faker.Person.FullName,
                    email ?? faker.Person.Email,
                    phone ?? faker.Person.Phone,
                    address ?? faker.Person.Address.Street,
                    BC.HashPassword(password ?? faker.Internet.Password()),
                    userRole ?? faker.PickRandom<UserRole>()
                ));
        }
    }
}
