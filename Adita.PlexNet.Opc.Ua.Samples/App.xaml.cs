using Adita.PlexNet.Opc.Ua.Builders;
using Adita.PlexNet.Opc.Ua.Constants;
using Adita.PlexNet.Opc.Ua.Identities;
using Adita.PlexNet.Opc.Ua.Samples.Activation;
using Adita.PlexNet.Opc.Ua.Samples.Contracts.Services;
using Adita.PlexNet.Opc.Ua.Samples.Core.Contracts.Services;
using Adita.PlexNet.Opc.Ua.Samples.Core.Services;
using Adita.PlexNet.Opc.Ua.Samples.Helpers;
using Adita.PlexNet.Opc.Ua.Samples.Services;
using Adita.PlexNet.Opc.Ua.Samples.ViewModels;
using Adita.PlexNet.Opc.Ua.Samples.Views;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.UI.Xaml;

namespace Adita.PlexNet.Opc.Ua.Samples;

// To learn more about WinUI 3, see https://docs.microsoft.com/windows/apps/winui/winui3/.
public partial class App : Application
{
    // The .NET Generic Host provides dependency injection, configuration, logging, and other services.
    // https://docs.microsoft.com/dotnet/core/extensions/generic-host
    // https://docs.microsoft.com/dotnet/core/extensions/dependency-injection
    // https://docs.microsoft.com/dotnet/core/extensions/configuration
    // https://docs.microsoft.com/dotnet/core/extensions/logging
    public IHost Host
    {
        get;
    }

    public static T GetService<T>()
        where T : class
    {
        if ((App.Current as App)!.Host.Services.GetService(typeof(T)) is not T service)
        {
            throw new ArgumentException($"{typeof(T)} needs to be registered in ConfigureServices within App.xaml.cs.");
        }

        return service;
    }

    public static WindowEx MainWindow { get; } = new MainWindow();

    public static UIElement? AppTitlebar { get; set; }

    public App()
    {
        InitializeComponent();

        Host = Microsoft.Extensions.Hosting.Host.
        CreateDefaultBuilder().
        UseContentRoot(AppContext.BaseDirectory).
        ConfigureServices((context, services) =>
        {
            // Default Activation Handler
            services.AddTransient<ActivationHandler<LaunchActivatedEventArgs>, DefaultActivationHandler>();

            // Other Activation Handlers

            // Services
            services.AddTransient<INavigationViewService, NavigationViewService>();

            services.AddSingleton<IActivationService, ActivationService>();
            services.AddSingleton<IPageService, PageService>();
            services.AddSingleton<INavigationService, NavigationService>();

            // Core Services
            services.AddSingleton<IFileService, FileService>();

            // Views and ViewModels
            services.AddTransient<SecondaryViewModel>();
            services.AddTransient<SecondaryPage>();
            services.AddTransient<MainViewModel>();
            services.AddTransient<MainPage>();
            services.AddTransient<ShellPage>();
            services.AddTransient<ShellViewModel>();

            // Configuration
        }).
        Build();

        UnhandledException += App_UnhandledException;

        //var uaApp = new UaApplicationBuilder()
        //   .SetApplicationUri("urn:WeQ-Adi:Weq.Test")
        //   .SetDirectoryStore("C:\\ProgramData\\Weq.Test\\pki")
        //   .SetIdentity(new UserNameIdentity("weq-adi", "admin"))
        //   .AddMappedEndpoint("Main", new EndpointDescription()
        //   {
        //       EndpointUrl = "opc.tcp://WeQ-Adi:4840",
        //       SecurityMode = MessageSecurityMode.SignAndEncrypt,
        //       SecurityPolicyUri = SecurityPolicyUris.Basic256Sha256
        //   }).Build();

        var services = new ServiceCollection();
        services.AddLogging(builder => builder.AddDebug());
        var serviceProvider = services.BuildServiceProvider();
        var loggerFactory = serviceProvider.GetRequiredService<ILoggerFactory>();
        var uaApp = new UaApplicationBuilder()
         .SetApplicationUri("urn:WeQ-Adi:Weq.Test")
         .SetDirectoryStore("C:\\ProgramData\\Weq.Test\\pki")
         .SetIdentity(new AnonymousIdentity())
         .SetLoggerFactory(loggerFactory)
         .AddMappedEndpoint("Main", new EndpointDescription()
         {
             EndpointUrl = "opc.tcp://192.168.5.99:4840",
             SecurityMode = MessageSecurityMode.None,
             SecurityPolicyUri = SecurityPolicyUris.None
         }).Build();
        uaApp.Run();
    }

    private void App_UnhandledException(object sender, Microsoft.UI.Xaml.UnhandledExceptionEventArgs e)
    {
        // TODO: Log and handle exceptions as appropriate.
        // https://docs.microsoft.com/windows/windows-app-sdk/api/winrt/microsoft.ui.xaml.application.unhandledexception.
    }

    protected async override void OnLaunched(LaunchActivatedEventArgs args)
    {
        base.OnLaunched(args);

        await App.GetService<IActivationService>().ActivateAsync(args);
    }
}
