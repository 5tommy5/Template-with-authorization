using EmailService;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Template.Domain.Configuration;
using Users.Application.Services;
using Users.Domain.Configuration;
using Users.Infrastructure.Services;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace Users.Infrastructure
{
    public static class ServiceCollectionExtension
    {
        public static void AddUsersAndAuthentication(this IServiceCollection services, IConfiguration Configuration)
        {
            services.AddTransient<ITokenService, TokenService>();
            services.AddTransient<IUserService, UserService>();
            services.AddTransient<IEmailSender, EmailSender>();

            services.Configure<GoogleProviderConfiguration>(Configuration.GetSection("GoogleAuthSettings"));
            services.Configure<JwtConfiguration>(Configuration.GetSection("JWT"));
            services.Configure<SendGridConfiguration>(Configuration.GetSection("SendGrid"));
            services.Configure<SendGridTemplates>(Configuration.GetSection("SendGridTemplates"));

            var jwtSettings = new JwtConfiguration();

            Configuration.GetSection("JWT").Bind(jwtSettings);

            services.AddAuthentication(opt =>
            {
                opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = jwtSettings.ValidIssuer,
                    ValidAudience = jwtSettings.ValidAudience,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8
                        .GetBytes(jwtSettings.Secret))
                };
            });
        }
    }
}
