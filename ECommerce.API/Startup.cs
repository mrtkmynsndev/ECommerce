using AutoMapper;
using ECommerce.API.Engine;
using ECommerce.API.Extensions;
using ECommerce.API.Middleware;
using ECommerce.Infrastructure.Data;
using ECommerce.Infrastructure.Identity;
using EECommerce.Infrastructure.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using StackExchange.Redis;

namespace ECommerce.API
{
    public class Startup
    {
        private readonly IConfiguration _configuration;

        public Startup(IConfiguration configuration)
        {
            _configuration = configuration;
        }


        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers().AddNewtonsoftJson(options => {
                options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
            });

            services.AddHttpContextAccessor();

            services.AddLogging();

            services.AddAutoMapper(typeof(MappingProfile));

            services.Configure<JwtSettings>(_configuration.GetSection("Jwt"));

            services.AddDbContext<ECommerceContext>(option => option.UseSqlite(_configuration.GetConnectionString(nameof(ECommerceContext))));

            services.AddDbContext<AppIdentityDbContext>(option =>
            {
                option.UseSqlite(_configuration.GetConnectionString(nameof(AppIdentityDbContext)));
            });

            #region Extension Services

            services.AddApplicationServices();

            services.AddIdentityServices(_configuration.GetSection("Jwt").Get<JwtSettings>());

            services.AddSwaggerDocumentation();

            #endregion

            services.AddSingleton<IConnectionMultiplexer>(opt =>
            {
                var configuration = ConfigurationOptions.Parse(_configuration.GetConnectionString("Redis"), true);
                return ConnectionMultiplexer.Connect(configuration);
            });

            services.AddCors(opt =>
            {
                opt.AddPolicy("CorsPolicy", policy =>
                {
                    policy.AllowAnyHeader().AllowAnyMethod().WithOrigins("https://localhost:4200");
                });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostEnvironment env)
        {
            // if (env.IsDevelopment())
            // {
            //     app.UseDeveloperExceptionPage();
            // }
            // else
            // {
            //     // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
            //     app.UseHsts();
            // }

            app.UseMiddleware<ExceptionMiddleware>();

            app.UseStatusCodePagesWithReExecute("/errors/{0}");

            app.UseRouting();

            app.UseStaticFiles();

            app.UseCors("CorsPolicy");

            app.UseAuthentication();

            app.UseAuthorization();

            app.UseSwaggerDocumentation();

            //app.UseHttpsRedirection();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}