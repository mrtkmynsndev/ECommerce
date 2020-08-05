using Microsoft.AspNetCore.Identity;

namespace ECommerce.Core.Entities.Identity
{
    public class AppUser : IdentityUser
    {
        public string DisplayName { get; set; }
        public Adress Adress { get; set; }
    }
}