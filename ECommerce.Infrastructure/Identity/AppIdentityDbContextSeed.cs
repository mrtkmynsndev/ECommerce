using System.Linq;
using System.Threading.Tasks;
using ECommerce.Core.Entities.Identity;
using Microsoft.AspNetCore.Identity;

namespace ECommerce.Infrastructure.Identity
{
    public class AppIdentityDbContextSeed
    {
        public static async Task SeedUserAsync(UserManager<AppUser> manager)
        {
            if (!manager.Users.Any())
            {
                var user = new AppUser()
                {
                    DisplayName = "Mert Kimyonşen",
                    Email = "mertkimyonsen@hotmail.com",
                    UserName = "mkimyonsen",
                    Adress = new Adress
                    {
                        Name = "Mert",
                        LastName = "Kimyonşen",
                        City = "Hatay",
                        State = "Samandag",
                        Street = "10 street",
                        Zip = "31"
                    }
                };

                await manager.CreateAsync(user, "Mrt@1964**");
            }
        }
    }
}