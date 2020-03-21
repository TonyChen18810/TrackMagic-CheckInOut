using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using WebTrackMagic.Models;
using WebTrackMagic.Services;


namespace TrackMagic_CheckInOut_aspdotnetcore
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
            services.AddRazorPages();

            services.AddHttpContextAccessor();

            services.AddSingleton<IStaffRepository, ExtDBServiceForStaff>();
            services.AddSingleton<IVendorRepository, ExtDBServiceForVendor>();
            services.AddSingleton<IItemRepository, ExtDBServiceForItem>();
            services.AddSingleton<IZoneRepository, ExtDBServiceForZone>();
            //using Firebase.Auth;
            services.AddSingleton<IItemTrxRepository, ExtDBServiceForItemTrxs>();

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            //Original template
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapRazorPages();
            });

            //W/O razor pages -super simple
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapRazorPages();
                //endpoints.MapGet("/", async context =>
                //{
                //    await context.Response.WriteAsync("Track Magic Controller!  nVirtua Int'l  : Developer: Darryl Hill, All Net Technology!");
                //});

            });

            // ListComs();
            // ShowInfo.DoShowInfo();

            // AppConfig config = AppConfig.Load(@".\config.json");
            // MyScale = new ScaleController(config.ScaleCOM);

            // MyCamera = new CameraController(config.CameraName);
            // MyCamera.OnImageUpdate = TestSaveImage;

        }

    }
}





