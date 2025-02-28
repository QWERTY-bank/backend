using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace Bank.Users.Api.Attributes
{
    /// <summary>
    /// Валидация номера телефона
    /// </summary>
    public class RuPhoneNumberAttribute : ValidationAttribute
    {
        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public override bool IsValid(object? value)
        {
            if (value is string phoneNumber)
            {
                string pattern = @"^(?:\+7|8)[0-9]{10}$";
                if (!Regex.IsMatch(phoneNumber, pattern))
                {
                    ErrorMessage = "The phone must start with +7 or 8 and have a length of 11 digits.";
                    return false;
                }
            }

            return true;
        }
    }
}
