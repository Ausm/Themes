using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using System.IO;
using System.Reflection;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.Extensions.FileProviders;

namespace Ausm.EmptyTheme
{
    public static class ThemeExtensions
    {
        public static void AddTheme(this IServiceCollection services)
        {
            services.Configure<RazorViewEngineOptions>(options =>
            {
                options.FileProviders.Add(new EmbeddedFileProvider(typeof(ThemeExtensions).GetTypeInfo().Assembly, nameof(Theme)));
            });
        }


        public static void UseTheme(this IApplicationBuilder app)
        {
            app.UseStaticFiles(new StaticFileOptions()
            {
                FileProvider = new EmbeddedFileProvider(typeof(ThemeExtensions).GetTypeInfo().Assembly, nameof(Theme) + ".wwwroot")
            });
        }
    }
}
