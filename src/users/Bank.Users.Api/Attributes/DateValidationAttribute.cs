using System.ComponentModel.DataAnnotations;

namespace Bank.Users.Api.Attributes
{
    public class DateValidationAttribute : ValidationAttribute
    {
        public override bool IsValid(object? value)
        {
            if (value is DateOnly date)
            {
                if (date >= DateOnly.FromDateTime(DateTime.UtcNow))
                {
                    return false;
                }
            }

            return true;
        }
    }
}
