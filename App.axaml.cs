using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Threading.Tasks;

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
        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime lifetime)
        {
            lifetime.MainWindow = new MainWindow();

            // Use this instead of Exit
            lifetime.ShutdownRequested += OnShutdownRequested;
        }

        StartWebServer();

        base.OnFrameworkInitializationCompleted();
    }

    private async void OnShutdownRequested(object? sender, ShutdownRequestedEventArgs e)
    {
        await StopWebServer();
    }

    private void StartWebServer()
    {
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
    }

    private async Task StopWebServer()
    {
        if (_webApp == null)
            return;

        try
        {
            await _webApp.StopAsync(TimeSpan.FromSeconds(3));
            await _webApp.DisposeAsync();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error while stopping web app: {ex}");
        }
    }
}
