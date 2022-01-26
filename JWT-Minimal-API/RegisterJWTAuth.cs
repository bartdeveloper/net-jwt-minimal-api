using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;

namespace JWT_Minimal_API
{
    public static class RegisterJWTAuth
    {
        public static IServiceCollection AddAuthenticationWithJWT(this IServiceCollection services, ConfigurationManager config) {
            
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidIssuer = config["JWT:Issuer"],
                    ValidAudience = config["JWT:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["JWT:Key"])),
                    ValidateActor = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true
                };
            });

            return services;

        }

        public static IServiceCollection AddSwaggerWithJWTAuth(this IServiceCollection services)
        {

            services.AddSwaggerGen(options =>
            {

                options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Name = "Authorization",
                    Description = "Bearer Authentication with JWT Token",
                    Type = SecuritySchemeType.Http
                });

                options.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Id = "Bearer",
                                Type = ReferenceType.SecurityScheme
                            }
                        },
                        new List<string>()
                    }
                });
            });

            return services;

        }
    }
}
