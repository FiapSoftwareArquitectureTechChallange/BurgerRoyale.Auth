using Bogus;
using Bogus.Extensions.Brazil;
using BurgerRoyale.Auth.Domain.DTO;
using BurgerRoyale.Auth.Domain.Entities;
using BurgerRoyale.Auth.Domain.Enumerators;

namespace BurgerRoyale.Auth.UnitTests.Domain.EntitiesMocks
{
    public static class UserMock
    {
        public static User Get
        (
            string? cpf = null,
            string? name = null,
            string? email = null,
            string? password = null,
            UserType? userType = null
        )
        {
            return UserFakerInstantiator(cpf, name, email, password, userType)
                .Generate();
        }

        public static List<User> GetList
        (
            int? quantity = null,
            UserType? userType = null
        )
        {
            return UserFakerInstantiator(userType: userType)
                .Generate(quantity ?? 3);
        }

        public static List<UserDTO> GetDtoList
        (
            int? quantity = null,
            UserType? userType = null
        )
        {
            return GetList(quantity, userType)
                .Select(user => new UserDTO(user))
                .ToList();
        }

        private static Faker<User> UserFakerInstantiator
        (
            string? cpf = null,
            string? name = null,
            string? email = null,
            string? password = null,
            UserType? userType = null
        )
        {
            return new Faker<User>()
                .CustomInstantiator(faker => new User(
                    cpf ?? faker.Person.Cpf(),
                    name ?? faker.Person.FullName,
                    email ?? faker.Person.Email,
                    password ?? faker.Internet.Password(),
                    userType ?? faker.PickRandom<UserType>()
                ));
        }
    }
}
