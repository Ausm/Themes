# Themes
ASP.Net Core MVC6 themes

Travis CI:
[![Build Status](https://travis-ci.org/Ausm/Themes.svg?branch=master)](https://travis-ci.org/Ausm/Themes)
Appveyor:
[![Build status](https://ci.appveyor.com/api/projects/status/3ppghqrpyal0f45i/branch/master?svg=true)](https://ci.appveyor.com/project/Ausm/themes/branch/master)
MyGet:
[![MyGet CI](https://img.shields.io/myget/themes/v/EmptyTheme.svg)](https://www.myget.org/feed/themes/package/nuget/EmptyTheme)

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
