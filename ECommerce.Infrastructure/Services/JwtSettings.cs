using ECommerce.Infrastructure.Services;

namespace EECommerce.Infrastructure.Services
{
    public class JwtSettings : IJwtSettings
    {
        public string Issuer { get; set; }
        public string Secret { get; set; }
        public string Audience { get; set; }
        public int ExpirationInDays { get; set; }
    }
}