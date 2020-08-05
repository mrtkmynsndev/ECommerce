using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using ECommerce.Core.Entities.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.API.Extensions
{
    public static class UserManagerExtension
    {
        public static async Task<AppUser> FindUserByCalimsPrincipleWithAdressAsync(this UserManager<AppUser> userManager, ClaimsPrincipal claimsPrincipal)
        {
            var userName = claimsPrincipal.Claims?.FirstOrDefault(x => x.Type == ClaimTypes.Name)?.Value;

            return await userManager.Users.Include(x => x.Adress).SingleOrDefaultAsync(x => x.UserName == userName);
        }

        public static async Task<AppUser> FindByUserNameFromClaimsPrinciple(this UserManager<AppUser> userManager, ClaimsPrincipal claimsPrincipal)
        {
            var userName = claimsPrincipal.Claims?.FirstOrDefault(x => x.Type == ClaimTypes.Name)?.Value;

            return await userManager.FindByNameAsync(userName);
        }
    }
}