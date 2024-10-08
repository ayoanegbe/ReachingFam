using CsvHelper;
using CsvHelper.Configuration;
using DnsClient.Internal;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using ReachingFam.Core.Models;
using System.Globalization;
using System.Reflection;

namespace ReachingFam.Core.Data
{
    public class DbInitializer
    {
        public static async Task Initialize(ApplicationDbContext context,
                                            UserManager<ApplicationUser> userManager,
                                            RoleManager<IdentityRole> roleManager,
                                            ILogger<DbInitializer> logger,
                                            IWebHostEnvironment environment)
        {
            context.Database.EnsureCreated();

            if (!context.UnitOfMeasures.Any())
            {
                await SeedUOM(context, environment, logger);
            }

            if (!context.Users.Any())
            {
                await CreateDefaultUserAndRoleForApplication(userManager, roleManager, logger);
            }
            
        }

        private static async Task CreateDefaultUserAndRoleForApplication(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, ILogger<DbInitializer> logger)
        {
            string superAdminRole = "Super Administrator";
            string email = "ayo.anegbe@gmail.com";

            await CreateDefaultAdministratorRole(roleManager, logger, superAdminRole);
            var user = await CreateDefaultUser(userManager, logger, email);
            await SetPasswordForDefaultUser(userManager, logger, email, user);
            await AddDefaultRoleToDefaultUser(userManager, logger, email, superAdminRole, user);
            await AddDefaultRoles(roleManager, logger);
        }

        private static async Task CreateDefaultAdministratorRole(RoleManager<IdentityRole> roleManager, ILogger<DbInitializer> logger, string superAdminRole)
        {
            logger.LogInformation($"Create the role '{superAdminRole}' for application");
            if (!await roleManager.RoleExistsAsync(superAdminRole))
            {
                var ir = await roleManager.CreateAsync(new IdentityRole(superAdminRole));
                if (ir.Succeeded)
                {
                    logger.LogDebug($"Created the role '{superAdminRole}' successfully");
                }
                else
                {
                    var exception = new Exception($"Default role '{superAdminRole}' cannot be created");
                    logger.LogError(GetIdentityErrorsInCommaSeperatedList(ir), exception);
                    throw exception;
                }
            }

        }

        private static async Task<ApplicationUser> CreateDefaultUser(UserManager<ApplicationUser> userManager, ILogger<DbInitializer> logger, string email)
        {
            logger.LogInformation($"Create default user with email '{email}' for application");

            var user = new ApplicationUser(email, "Super Administrator", "Reaching Family", "5878891428");
            

            var ir = await userManager.CreateAsync(user);
            if (ir.Succeeded)
            {
                logger.LogDebug($"Created default user '{email}' successfully");
            }
            else
            {
                var exception = new Exception($"Default user '{email}' cannot be created");
                logger.LogError(GetIdentityErrorsInCommaSeperatedList(ir), exception);
                throw exception;
            }

            return user;

        }

        private static async Task SetPasswordForDefaultUser(UserManager<ApplicationUser> userManager, ILogger<DbInitializer> logger, string email, ApplicationUser user)
        {
            logger.LogInformation($"Set password for default user '{email}'");
            const string password = "Password01!";
            var ir = await userManager.AddPasswordAsync(user, password);
            if (ir.Succeeded)
            {
                logger.LogTrace($"Set password '{password}' for default user '{email}' successfully");
            }
            else
            {
                var exception = new Exception($"Password for the user '{email}' cannot be set");
                logger.LogError(GetIdentityErrorsInCommaSeperatedList(ir), exception);
                throw exception;
            }
        }

        private static async Task AddDefaultRoleToDefaultUser(UserManager<ApplicationUser> userManager, ILogger<DbInitializer> logger, string email, string superAdminRole, ApplicationUser user)
        {
            logger.LogInformation($"Add default user '{email}' to role '{superAdminRole}'");
            var ir = await userManager.AddToRoleAsync(user, superAdminRole);
            if (ir.Succeeded)
            {
                logger.LogDebug($"Added the role '{superAdminRole}' to default user `{email}` successfully");
            }
            else
            {
                var exception = new Exception($"The role '{superAdminRole}' cannot be set for the user `{email}`");
                logger.LogError(GetIdentityErrorsInCommaSeperatedList(ir), exception);
                throw exception;
            }
        }

        private static async Task AddDefaultRoles(RoleManager<IdentityRole> roleManager, ILogger<DbInitializer> logger)
        {
            var admin = "Administrator";
            logger.LogInformation($"Create the role '{admin}' for application");
            if (!await roleManager.RoleExistsAsync(admin))
            {
                var role = new IdentityRole(admin);
                await roleManager.CreateAsync(role);
                logger.LogDebug($"Created the role '{admin}' successfully");
            }

            var user = "User";
            logger.LogInformation($"Create the role '{user}' for application");
            if (!await roleManager.RoleExistsAsync(user))
            {
                var role = new IdentityRole(user);
                await roleManager.CreateAsync(role);
                logger.LogDebug($"Created the role '{user}' successfully");
            }

            //var apiUser = "API User";
            //logger.LogInformation($"Create the role '{apiUser}' for application");
            //if (!await roleManager.RoleExistsAsync(user))
            //{
            //    var role = new IdentityRole(apiUser);
            //    await roleManager.CreateAsync(role);
            //    logger.LogDebug($"Created the role '{apiUser}' successfully");
            //}

        }

        private static async Task SeedUOM(ApplicationDbContext context, IWebHostEnvironment environment, ILogger<DbInitializer> logger)
        {
            try
            {
                var config = new CsvConfiguration(CultureInfo.InvariantCulture)
                {
                    HasHeaderRecord = true,
                };

                string path = Path.Combine(environment.WebRootPath, "data");

                string file = "UnitOfMeasure.csv";

                var filePath = Path.Combine(path, file);

                using (var reader = new StreamReader(filePath))
                using (var csv = new CsvReader(reader, config))
                {
                    var records = csv.GetRecords<UOMCSVMap>();
                    foreach (UOMCSVMap record in records)
                    {
                        UnitOfMeasure unitOfMeasure = new()
                        {
                            Name = record.Name,
                            Symbol = record.Symbol,
                            Description = record.Description,
                            DateAdded = DateTime.Now,
                            AddedBy = "System"
                        };

                        await context.AddAsync(unitOfMeasure);
                    }
                }

                await context.SaveChangesAsync();
            }
            catch (Exception ex) 
            {
                logger.LogDebug($"An error has occurred when trying to create UnitOfMeasure table => {ex}");
            }
            
        }

        private static string GetIdentityErrorsInCommaSeperatedList(IdentityResult ir)
        {
            string errors = null;
            foreach (var identityError in ir.Errors)
            {
                errors += identityError.Description;
                errors += ", ";
            }
            return errors;
        }
    }
}
