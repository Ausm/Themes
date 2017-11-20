using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.Extensions.FileProviders;
using ObjectStore.Sqlite;
using ObjectStore.Identity;

namespace Ausm.ThemeWithMenuAndIdentity
{
    public static class ThemeExtensions
    {
        public static void AddTheme(this IServiceCollection services, string connectionString)
        {
            AddTheme<User, Role>(services, connectionString);
        }

        public static void AddTheme<TUser, TRole>(this IServiceCollection services, string connectionString)
            where TUser : User
            where TRole : Role
        {
            AddTheme<TUser, TRole, UserInRole<TUser, TRole>>(services, connectionString);
        }

        public static void AddTheme<TUser, TRole, TUserInRole>(this IServiceCollection services, string connectionString)
            where TUser : User
            where TRole : Role
            where TUserInRole : UserInRole<TUser, TRole>
        {
            services.AddObjectStoreWithSqlite(connectionString);
            services.AddIdentity<TUser, TRole>().AddObjectStoreUserStores<TUser, TRole, TUserInRole>().AddDefaultTokenProviders();
            services.AddTransient(typeof(IUserManagerProvider), typeof(UserManagerProvider<TUser, TRole>));


            services.Configure<RazorViewEngineOptions>(options =>
            {
                options.FileProviders.Add(new EmbeddedFileProvider(typeof(ThemeExtensions).GetTypeInfo().Assembly, typeof(ThemeExtensions).Namespace));
            });
        }

        public static void UseTheme(this IApplicationBuilder app)
        {
            app.UseIdentity();
            app.UseStaticFiles(new StaticFileOptions()
            {
                FileProvider = new EmbeddedFileProvider(typeof(ThemeExtensions).GetTypeInfo().Assembly, typeof(ThemeExtensions).Namespace + ".wwwroot")
            });
        }
    }
}
