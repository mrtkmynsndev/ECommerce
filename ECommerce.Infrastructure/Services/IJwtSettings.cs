namespace ECommerce.Infrastructure.Services
{
    public interface IJwtSettings
    {
        string Issuer { get; set; }
        string Audience { get; set; }
        string Secret { get; set; }
        int ExpirationInDays { get; set; }

    }
}