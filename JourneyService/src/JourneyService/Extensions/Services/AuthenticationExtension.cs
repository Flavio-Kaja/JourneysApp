using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace JourneyService.Extensions.Services
{
    public static class AuthenticationExtension
    {

        public static void AddAppAuthentication(this IServiceCollection services, IWebHostEnvironment env, IConfiguration configuration)
        {
            string encryptionKey = configuration["Auth:ClientSecret"];
            byte[] key = Encoding.ASCII.GetBytes(encryptionKey);

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.SaveToken = true;
                options.RequireHttpsMetadata = !env.IsDevelopment();
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    ValidateIssuer = true,
                    ValidateAudience = false,
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero,
                    ValidIssuer = configuration["Auth:Issuer"],
                    IssuerSigningKey = new SymmetricSecurityKey(key)
                };
            });
        }

    }

}
