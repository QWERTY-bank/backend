using System.ComponentModel;

namespace Bank.Common.Models.Auth
{
    public enum RoleType
    {
        [Description("Пользователь")]
        Default = 1,

        [Description("Администратор")]
        Admin = 50
    }
}
