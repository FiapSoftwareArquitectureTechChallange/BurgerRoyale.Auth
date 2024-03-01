using System.ComponentModel;

namespace BurgerRoyale.Auth.Domain.Enumerators
{
    public enum UserRole
    {
        [Description("Admin")]
        Admin = 0,

        [Description("Cliente")]
        Customer = 1,

        [Description("Funcionário")]
        Employee = 2
    }
}
