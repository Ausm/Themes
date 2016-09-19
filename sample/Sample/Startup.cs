using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Ausm.ThemeWithMenuAndIdentity;
using Microsoft.Extensions.Configuration;
using Sample.Entities;

namespace Sample
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            Configuration = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddEnvironmentVariables()
                .Build();
        }

        IConfigurationRoot Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<ThemeOptions>(themeOptions => {
                themeOptions.LogoUrl = "/logo.png";
                themeOptions.StaticMenuItems = new IMenuItem[] {
                    new MenuItem("Links",
                        new MenuItem("Google", url:"http://www.google.at"),
                        new MenuItem("Disabled", url:"http://www.youtube.com") { IsEnabled = false },
                        new SeparatorItem(),
                        new MenuItem("Facebook", url:"http://www.facebook.com"))
                };

                themeOptions.DynamicMenuItems = () => null;
            });

            services.AddTheme<User, Role, UserInRole>(Configuration.GetConnectionString("DefaultConnection"));
            services.AddMvc();
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            app.UseDeveloperExceptionPage();
            app.UseTheme();
            app.UseStaticFiles();
            app.UseMvcWithDefaultRoute();
        }
    }
}
