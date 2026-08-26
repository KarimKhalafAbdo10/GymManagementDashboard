
using GymMangement.BLL;
using GymMangement.BLL.Services.AttchmentService;
using GymMangement.BLL.Services.Classes;
using GymMangement.BLL.Services.Interfaces;
using GymMangement.DAL.Data.DbContexts;



using GymMangement.DAL.Repositories.Classes;
using GymMangement.DAL.Repositories.Interfaces;
using GymMangement.PL;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Logging;
using GymMangement.DAL.Data.Models;
using Microsoft.AspNetCore.Identity;

namespace GemMangement
{
    public class Program
    {

        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Configure logging
            //builder.Logging.ClearProviders();
            //builder.Logging.AddConsole();
            //builder.Logging.AddDebug();
            //builder.Logging.SetMinimumLevel(LogLevel.Information);

            builder.Services.AddControllersWithViews();

            builder.Services.AddScoped(typeof(IGenericRepository<>),typeof(GenericRepository<>));
            builder.Services.AddScoped<IMemberService, MemberService>();
            builder.Services.AddScoped<IPlanService, PlanService>();
            builder.Services.AddScoped<ITrainerService,TrainerService>();
            builder.Services.AddScoped<IUnitOfWork,UnitOfWork>();
            builder.Services.AddScoped<ISessionRepository,SessionRepository>();
            builder.Services.AddScoped<ISessionService,SessionSerivce>();
            builder.Services.AddScoped<IMemberShipReposiotry, MemberShipRepository>();
            builder.Services.AddScoped<IMemberShipService, MemebrShipService>();
            builder.Services.AddScoped<IBookingRepository, BookingRepositroy>();
            builder.Services.AddScoped<IBookingService, BookingService>();
            builder.Services.AddScoped<IAnalyticsService,AnalyticsService>();
            builder.Services.AddScoped<IAttachmentService,AttachmentService>();
            builder.Services.AddAutoMapper(m=>m.AddProfile(new MappingProfile()));
            builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
            {
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(2);
                options.Lockout.MaxFailedAccessAttempts = 5;
            }).AddEntityFrameworkStores<GymDbContext>();
           
            builder.Services.AddDbContext<GymDbContext>(Options =>
            {
                Options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
            });

            var app = builder.Build();
           await app.MigrateAndSeedAsync();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Account}/{action=Login}/{id?}");

            app.Run();
        }
    }
}
