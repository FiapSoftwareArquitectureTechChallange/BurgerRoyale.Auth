using System.ComponentModel;

namespace BurgerRoyale.Auth.Domain.Enumerators
{
    public enum UserRole
    {
        [Description("Admin")]
        Admin = 0,

        [Description("Customer")]
        Customer = 1,

        [Description("Employee")]
        Employee = 2
    }
}
