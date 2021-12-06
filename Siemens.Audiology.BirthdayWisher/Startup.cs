using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Siemens.Audiology.BirthdayWisher.HostedServices;
using Siemens.Audiology.BirthdayWisher.Models;
using Siemens.Audiology.BirthdayWisher.Profiles;
using Siemens.Audiology.Notification;
using Siemens.Audiology.Notification.Contract;
using System;

namespace Siemens.Audiology.BirthdayWisher
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
            var identitySettingsSection = Configuration.GetSection("BirthdaySchedulerOptions");
            services.AddSingleton<IMailer, Mailer>();
            services.Configure<BirthdaySchedulerOptions>(identitySettingsSection);
            services.AddRazorPages();
            services.AddAutoMapper(cfg => cfg.AddProfile<MappingProfile>(),
                                         AppDomain.CurrentDomain.GetAssemblies()); 
            services.AddHostedService<BirthdayHostedService>();
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

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapRazorPages();
            });
        }
    }
}
