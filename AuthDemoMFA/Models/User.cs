namespace AuthDemo.Models
{
    // Models/User.cs
    public class User
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string Username { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty;
        public bool IsMfaEnabled { get; set; }
        public string? MfaSecretKey { get; set; }
    }
}