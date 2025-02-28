using System.ComponentModel.DataAnnotations;

namespace Bank.Users.Api.Attributes
{
    /// <summary>
    /// Валидация даты, что она не позже текущей
    /// </summary>
    public class DataEarlierThenCurrentAttribute : ValidationAttribute
    {
        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public override bool IsValid(object? value)
        {
            if (value is DateOnly date)
            {
                if (date >= DateOnly.FromDateTime(DateTime.UtcNow))
                {
                    ErrorMessage = $"Error: {date} >= {DateOnly.FromDateTime(DateTime.UtcNow)}. The date must be earlier than the current one.";
                    return false;
                }
            }

            return true;
        }
    }
}
