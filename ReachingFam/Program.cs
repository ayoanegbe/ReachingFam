using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using ReachingFam.Core.Data;
using ReachingFam.Core.Interfaces;
using ReachingFam.Core.Models;
using ReachingFam.Core.Repositories;
using ReachingFam.Core.Services;
using Serilog;
using System.Security.Authentication;

namespace ReachingFam
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var httpClientHandler = new HttpClientHandler
            {
                SslProtocols = SslProtocols.Tls12
            };

            var builder = WebApplication.CreateBuilder(args);

            var _logger = new LoggerConfiguration()
            .MinimumLevel.Error()
            .WriteTo.File($"{builder.Environment.WebRootPath}\\Logs\\Log.txt", rollingInterval: RollingInterval.Day)
            .CreateLogger();

            builder.Logging.AddSerilog(_logger);

            // Add services to the container.
            var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
            builder.Services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(connectionString));
            builder.Services.AddDatabaseDeveloperPageExceptionFilter();

            builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options => options.SignIn.RequireConfirmedAccount = false)
                .AddEntityFrameworkStores<ApplicationDbContext>();
            builder.Services.AddControllersWithViews();

            builder.Services.AddSingleton<UniqueCode>();
            builder.Services.AddSingleton<CustomIDataProtection>();
            builder.Services.Configure<RecaptchaSettings>(builder.Configuration.GetSection("GoogleRecaptchaV3"));
            builder.Services.Configure<EmailSettings>(builder.Configuration.GetSection("EmailSettings"));

            builder.Services.AddTransient<IReportData, ReportDataRepository>();
            builder.Services.AddTransient<IFileService, FileService>();
            builder.Services.AddTransient<IApprovalService, ApprovalService>();
            builder.Services.AddTransient<IResolverService, ResolverService>();

            builder.Services.AddSingleton<RecaptchaService>();

            builder.Services.AddHttpClient();
            builder.Services.AddDistributedMemoryCache();
            builder.Services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromMinutes(30);
                options.Cookie.Name = ".ReachingFamily";
                options.Cookie.Path = "/";
                //options.Cookie.HttpOnly = true;
                options.Cookie.IsEssential = true;
            });

            builder.Services.AddMvc();

            var app = builder.Build();

            using (IServiceScope scope = app.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                try
                {
                    var context = services.GetRequiredService<ApplicationDbContext>();
                    var userManager = services.GetRequiredService<UserManager<ApplicationUser>>();
                    var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();

                    var dbInitializerLogger = services.GetRequiredService<ILogger<DbInitializer>>();
                    DbInitializer.Initialize(context, userManager, roleManager, dbInitializerLogger).Wait();
                }
                catch (Exception)
                {
                    var logger = services.GetRequiredService<ILogger<Program>>();
                    logger.LogError("An error occurred while seeding the database.");
                }
            }

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseMigrationsEndPoint();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();
            app.UseSession();

            app.Use(async (context, next) =>
            {
                context.Response.Headers.Append("X-XSS-Protection", "1; mode=block");
                context.Response.Headers.Append("X-Frame-Options", "DENY");
                context.Response.Headers.Append("X-Content-Type-Options", "nosniff");
                //context.Response.Headers.Add("Strict-Transport-Security", "max-age=86400");
                context.Response.Headers.Append("Cache-Control", "no-cache");
                context.Response.Headers.Append("Referrer-Policy", "no-referrer");
                context.Response.Headers.Append("X-Permitted-Cross-Domain-Policies", "none");
                context.Response.Headers.Remove("X-Powered-By");

                await next();
            });

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");
            app.MapRazorPages();

            app.Run();
        }
    }
}
