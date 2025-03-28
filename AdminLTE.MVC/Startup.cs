using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.EntityFrameworkCore;
using AdminLTE.MVC.Data;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Authorization;
using AdminLTE.MVC.Models;
using AdminLTE.MVC.Utilites;
using AspNetCoreHero.ToastNotification;
using AdminLTE.MVC.Repository.Interface;
using AdminLTE.MVC.Repository.Services;
using AspNetCoreHero.ToastNotification.Extensions;

namespace AdminLTE.MVC
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(
                    Configuration.GetConnectionString("DefaultConnection")));
            //services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
            //    .AddEntityFrameworkStores<ApplicationDbContext>();
            services.AddIdentity<ApplicationUser, IdentityRole>(options =>
            {
                options.User.RequireUniqueEmail = false;
            })
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();
            services.AddHttpClient();
            services.AddScoped<IDbInitializer, DbInitializer>();
            services.AddTransient<ITheaterService, TheaterService>();
            services.AddTransient<IReceiptService, ReceiptService>();
            services.AddTransient<IMovieService, MovieService>();
            services.AddTransient<IBrandService, BrandService>();
            services.AddTransient<INotificationAlertService, NotificationAlertService>();
            services.AddTransient<IIRDOfficeService, IRDOfficeService>();
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());


            services.AddNotyf(config => { config.DurationInSeconds = 5 ; config.IsDismissable = true; config.Position = NotyfPosition.BottomRight; });
            services.ConfigureApplicationCookie(options =>
            {
                options.LoginPath = "/login";
                options.AccessDeniedPath = "/AccessDenied";
            });
            services.AddControllersWithViews();
            services.AddRazorPages();
            // services.AddMvc(o =>
            // {
            //     //Add Authentication to all Controllers by default.
            //     var policy = new AuthorizationPolicyBuilder()
            //         .RequireAuthenticatedUser()
            //         .Build();
            //     o.Filters.Add(new AuthorizeFilter(policy));
            // });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            DataSeeding();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
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

            app.UseAuthentication();
            app.UseAuthorization();
            app.UseNotyf();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                name: "areas",
                pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}"
                );
            });



            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
                endpoints.MapRazorPages();
            });
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "admin",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
                endpoints.MapRazorPages();
            });
            void DataSeeding()
            {
                using (var scope = app.ApplicationServices.CreateScope())
                {
                    var DbInitialize = scope.ServiceProvider.GetRequiredService<IDbInitializer>();
                    DbInitialize.Initialize();
                }
            }
        }
    }
}
