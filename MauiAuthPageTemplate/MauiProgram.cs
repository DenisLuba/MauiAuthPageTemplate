using AuthenticationMaui.Services;
using CommunityToolkit.Maui;
using MauiAuthPageTemplate.Services;
using MauiAuthPageTemplate.Services.Interfaces;
using MauiAuthPageTemplate.ViewModels;
using Microsoft.Extensions.Logging;

namespace MauiAuthPageTemplate;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>()
            .UseMauiCommunityToolkit()
            .AddServices()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
            });

#if DEBUG
        builder.Logging.AddDebug();
#endif

        return builder.Build();
    }

    static MauiAppBuilder AddServices(this MauiAppBuilder builder)
    {
        builder.Services.AddSingleton<IUserStorageService, UserSecureStorageService>();
        builder.Services.AddSingleton<ILoginService>(provider =>
        {
            var userStorageService = provider.GetRequiredService<IUserStorageService>();
            return new FirebaseLoginService(
                new()
                {
                    UserStorageService = userStorageService,
                    ApiKey = GlobalValues.API_KEY,
                    AuthDomain = GlobalValues.AUTH_DOMAIN,
                    GoogleClientId = GlobalValues.GOOGLE_CLIENT_ID,
                    GoogleRedirectUri = GlobalValues.GOOGLE_REDIRECT_URI,
                    CallbackScheme = GlobalValues.CALLBACK_SCHEME
                });
        });

        builder.Services.AddSingleton<AuthService>();
        builder.Services.AddSingleton<INavigationService, NavigationService>();

        builder.Services.AddSingleton<AuthPageViewModel>();
        builder.Services.AddSingleton<SignUpPopupViewModel>();
        builder.Services.AddSingleton<ResetPasswordPopupViewModel>();
        builder.Services.AddSingleton<LoginWithPhoneViewModel>();
        builder.Services.AddSingleton<SignOutPopupViewModel>();

        return builder;
    }
}
