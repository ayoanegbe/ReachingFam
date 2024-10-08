
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.CookiePolicy;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using ReachingFam.Core.Data;
using ReachingFam.Core.Interfaces;
using ReachingFam.Core.Models;
using ReachingFam.Core.Repositories;
using Serilog;
using System.Security.Authentication;
using System.Text;

namespace ReachingFam.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            var httpClientHandler = new HttpClientHandler
            {
                SslProtocols = SslProtocols.Tls12
            };

            var _logger = new LoggerConfiguration()
            .MinimumLevel.Error()
            .WriteTo.File($"{builder.Environment.WebRootPath}\\Logs\\Log.txt", rollingInterval: RollingInterval.Day)
            .CreateLogger();

            builder.Logging.AddSerilog(_logger);

            // Add services to the container.

            var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
            builder.Services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(connectionString));

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo 
                {
                    Title = "Reaching Families API",
                    Version = "v1",
                    Contact = new OpenApiContact() { Name = "Ayo Anegbe", Email = "ayo.anegbe@gmail.com" },
                });
                c.SchemaFilter<EnumSchemaFilter>();
            });

            // Configure JWT Authentication
            //builder.Services.AddAuthentication(options =>
            //{
            //    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            //    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            //})
            //.AddJwtBearer(options =>
            //{
            //    options.TokenValidationParameters = new TokenValidationParameters
            //    {
            //        ValidateIssuer = true,
            //        ValidateAudience = true,
            //        ValidateLifetime = true,
            //        ValidateIssuerSigningKey = true,
            //        ValidIssuer = "yourdomain.com", // Replace with your issuer
            //        ValidAudience = "yourdomain.com", // Replace with your audience
            //        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("your_secret_key")) // Replace with your secret key
            //    };
            //});

            builder.Services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.Strict;
                options.Secure = CookieSecurePolicy.Always;
                options.HttpOnly = HttpOnlyPolicy.Always;
            });

            builder.Services.AddTransient<IRequestFormRepository, RequestFormRepository>();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            //app.UseAuthentication();

            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}
