# Themes
ASP.Net Core MVC6 themes

##How to Use?


**Simply write a startup class like:**

```C#
public class Startup
{
    public void ConfigureServices(IServiceCollection services)
    {
        services.AddMvc();
        services.AddTheme();
    }

    public void Configure(IApplicationBuilder app)
    {
        app.UseTheme();
        app.UseStaticFiles();

        app.UseMvcWithDefaultRoute();
    }
}
```

And your mvc application acts, like you have the client side packages, controller and views incl. the layout set up.
