using System.Linq;
using System.Security.Claims;

namespace ECommerce.API.Extensions
{
    public static class CalimsPrincipalExtension
    {
        public static string GetUserName(this ClaimsPrincipal claimsPrincipal)
        {
            return claimsPrincipal?.Claims?.FirstOrDefault(x => x.Type == ClaimTypes.Name).Value;
        }
    }
}