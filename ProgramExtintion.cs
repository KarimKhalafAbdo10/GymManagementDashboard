using GemMangement;
using GymMangement.DAL.Data.DataSeed;
using GymMangement.DAL.Data.DbContexts;
using GymMangement.DAL.Data.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace GymMangement.PL
{
    public static class ProgramExtintion
    {
        public static async Task MigrateAndSeedAsync(this WebApplication app)
        {
          using  var scope = app.Services.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<GymDbContext>();
            var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();
            var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
            var seedFolderPath = Path.Combine(app.Environment.ContentRootPath, "wwwroot", "Files");
            var pendingMigrations =await dbContext.Database.GetPendingMigrationsAsync();

            if(pendingMigrations.Any())
            {
                await dbContext.Database.MigrateAsync();
            }

          await  GymDataSeeding.SeedAsync(dbContext, seedFolderPath, logger);

           await IdentityDataSeeding.SeedIdentityDataAsync(roleManager,userManager,logger);
        }


    }
}
