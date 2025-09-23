using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;

using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Hosting;
using System;
using Microsoft.AspNetCore.SignalR;  // For ConfigureWebHostDefaults


namespace DesktopApp;

public partial class App : Application
{
    private WebApplication? _webApp;
    public static IHubContext<QuizHub>? HubContext;
    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);
    }

    public override void OnFrameworkInitializationCompleted()
    {
        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            desktop.MainWindow = new MainWindow();

            desktop.Exit += OnAppExit;
        }

        var builder = WebApplication.CreateBuilder(new WebApplicationOptions
        {
            ContentRootPath = AppContext.BaseDirectory,
            WebRootPath = "wwwroot"
        });

        builder.Services.AddSignalR();

        var app = builder.Build();

        app.UseDefaultFiles();
        app.UseStaticFiles();
        app.MapHub<QuizHub>("/quizhub");

        app.Urls.Add("http://0.0.0.0:5000");
        app.Start();

        HubContext = app.Services.GetRequiredService<IHubContext<QuizHub>>();

        _webApp = app;

        base.OnFrameworkInitializationCompleted();
    }

    private void OnAppExit(object? sender, ControlledApplicationLifetimeExitEventArgs e)
    {
        if (_webApp is null) return;
        _webApp.StopAsync(TimeSpan.FromSeconds(3)).GetAwaiter().GetResult();
        _webApp.DisposeAsync().AsTask().GetAwaiter().GetResult();
    }
}