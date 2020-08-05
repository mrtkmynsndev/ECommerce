using System.Collections.Generic;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;

namespace ECommerce.API.Extensions
{
    public static class SwaggerServiceExtension
    {
        public static IServiceCollection AddSwaggerDocumentation(this IServiceCollection services)
        {
            services.AddSwaggerGen(opt =>
            {
                opt.SwaggerDoc("v1", new OpenApiInfo() { Title = "ECommerce v1", Version = "1.0.0" });

                opt.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme(){
                    Description = "Jwt Bearer Token",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey
                });

                var security = new OpenApiSecurityRequirement(){
                    {
                        new OpenApiSecurityScheme(){
                            Reference = new OpenApiReference(){
                                Id = "Bearer",
                                Type = ReferenceType.SecurityScheme
                            },
                            UnresolvedReference = true
                        },
                        new List<string>()
                    }
                };

                opt.AddSecurityRequirement(security);
            });

            return services;
        }

        public static IApplicationBuilder UseSwaggerDocumentation(this IApplicationBuilder app)
        {
            app.UseSwagger();
            app.UseSwaggerUI(opt => {
                opt.SwaggerEndpoint("/swagger/v1/swagger.json", "ECommerce v1");
            });

            return app;
        }
    }
}