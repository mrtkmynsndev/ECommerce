using ECommerce.Core.Entities.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.Infrastructure.Identity
{
    public class AppIdentityDbContext : IdentityDbContext<AppUser>
    {
        public AppIdentityDbContext(DbContextOptions<AppIdentityDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // builder.Entity<AppUser>(build =>{
            //     build.HasOne(x => x.Adress).WithOne();
            // });

            // builder.Entity<Adress>(build => {
            //     build.HasKey(x => x.Id);
            //     build.HasOne(x => x.AppUser).WithOne(x => x.Adress);
            // });
        }
    }
}