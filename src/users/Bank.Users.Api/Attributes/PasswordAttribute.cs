using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace Bank.Users.Api.Attributes
{
    /// <summary>
    /// Валидация пароля
    /// </summary>
    public class PasswordAttribute : ValidationAttribute
    {
        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public override bool IsValid(object? value)
        {
            if (value is string password)
            {
                string pattern = @"^(?=.*[A-Z])(?=.*\d).{6,}$";
                if (!Regex.IsMatch(password, pattern))
                {
                    return false;
                }
            }

            return true;
        }
    }
}
