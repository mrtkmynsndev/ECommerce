using ECommerce.Core.Entities.Identity;

namespace ECommerce.Core.Interfaces
{
    public interface ITokenService
    {
         string CreateToken(AppUser user);
    }
}