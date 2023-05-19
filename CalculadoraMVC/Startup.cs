using CalculadoraMVC.Errors;
using CalculadoraMVC.Models;
using CalculadoraMVC.Repositories;
using CalculadoraMVC.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;

namespace CalculadoraMVC
{
   public class Startup
   {
      private IConfiguration _configuration;

      public Startup(IConfiguration configuration)
      {
         _configuration = configuration;
      }

      public void ConfigureServices(IServiceCollection services)
      {
         services.AddDbContext<ApplicationDbContext>(options => options.UseSqlite(_configuration.GetConnectionString("ApplicationDbContextConnection")));

         services.AddIdentity<ApplicationUser, IdentityRole>()
             .AddRoles<IdentityRole>()
             .AddEntityFrameworkStores<ApplicationDbContext>()
             .AddErrorDescriber<CustomIdentityErrorDescriber>();

         services.AddRazorPages();

         services.AddMvc(options => options.EnableEndpointRouting = false);

         services.AddScoped<IOperacionRepository, OperacionRepository>();
         services.AddScoped<IUserRepository, UserRepository>();
         services.AddScoped<IRoleRepository, RoleRepository>();
         services.AddScoped<IUnitOfWork, UnitOfWork>();

         services.AddScoped<UserService>();
         services.AddScoped<RoleService>();

         services.AddAuthorization(options =>
         {
            options.AddPolicy("UserOnly", policy => policy.RequireClaim("AdminNumber"));
            options.AddPolicy("RequireAdmin", policy => policy.RequireRole("Administrator"));
         });
      }

      public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
      {
         app.UseStaticFiles();

         app.UseStatusCodePagesWithRedirects("~/Views/Home/Error");

         app.UseAuthentication();

         app.UseAuthorization();

         app.UseStaticFiles(new StaticFileOptions
         {
            FileProvider = new PhysicalFileProvider(
                 Path.Combine(env.ContentRootPath, "node_modules")),
            RequestPath = "/node_modules"
         });

         app.UseMvc(routes =>
         {
            routes.MapRoute(
                name: "default",
                template: "{controller=Home}/{action=Index}");
         });
      }
   }
}