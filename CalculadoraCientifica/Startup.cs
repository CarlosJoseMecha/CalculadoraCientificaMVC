using Microsoft.Extensions.FileProviders;
using CalculadoraCientifica.Repositories;
using CalculadoraCientifica.Models;
using Microsoft.EntityFrameworkCore;
using CalculadoraCientifica.Errors;

namespace CalculadoraCientifica
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
         services.AddDbContext<ApplicationDbContext>(options =>
                    options.UseSqlite(
                        _configuration.GetConnectionString("ApplicationDbContextConnection")));

         services.AddDefaultIdentity<ApplicationUser>(options => options.SignIn.RequireConfirmedAccount = false)
             .AddEntityFrameworkStores<ApplicationDbContext>()
             .AddErrorDescriber<CustomIdentityErrorDescriber>();

         services.AddRazorPages();
         services.AddMvc(options => options.EnableEndpointRouting = false);

         services.AddScoped<IOperacionRepository, OperacionRepository>();

         services.AddAuthorization();
      }

      public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
      {
         app.UseStaticFiles();
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
