namespace Bank.Users.Application.Settings.Models
{
    public class ClientUserSettingsDto
    {   
        public Guid Id { get; set; }
        public bool IsNightTheme { get; set; }
        public long[] HiddenAccounts { get; set; } = null!;
    }
}
