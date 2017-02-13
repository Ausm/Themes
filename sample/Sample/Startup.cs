using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Ausm.ThemeWithMenuAndIdentity;
using Microsoft.Extensions.Configuration;
using Sample.Entities;
using System;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;

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

                themeOptions.SetDynamicMenuItemExpression(async (IHttpContextAccessor httpContextAccessor, UserManager<User> userManager) => {
                    List<MenuItem> returnValue = new List<MenuItem>();

                    User user = await userManager.GetUserAsync(httpContextAccessor.HttpContext.User);
                    if (user != null)
                        returnValue.Add(new MenuItem(user.Name, url: "www.google.com"));

                    if (httpContextAccessor.HttpContext.User.IsInRole("Admin"))
                        returnValue.Add(new MenuItem("Is In Admin", url: "http://www.google.com"));

                    if (httpContextAccessor.HttpContext.User.Identity.IsAuthenticated)
                        returnValue.Add(new MenuItem("CheckValue", controller: "Home", action: nameof(HomeController.CheckValue), routeValues: new { id = httpContextAccessor.HttpContext.User.Identity.Name }));
                    else
                        returnValue.Add(new MenuItem("Not logged in", "http://www.google.at"));

                    returnValue.Add(new MenuItem(DateTime.Today.ToString(), "http://www.google.at"));
                    return returnValue;
                });

                themeOptions.AccountSettingAction = nameof(HomeController.AccountSettings);
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
