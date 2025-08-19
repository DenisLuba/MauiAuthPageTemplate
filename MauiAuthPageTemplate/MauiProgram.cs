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
                    ApiKey = GlobalValues.API_KEY, // Ваш Web API Key из Firebase Console (Firebase Console > Project Settings > General > "Web API Key")
                    AuthDomain = GlobalValues.AUTH_DOMAIN, // Обычно это your-project-id.firebaseapp.com (Firebase Console > Authentication > Settings > "Authorized domains")
                    GoogleClientId = GlobalValues.GOOGLE_CLIENT_ID, // Ваш Google Client ID (Firebase Console > Authentication > Sign-in method > Google > Web SDK configuration > "Web client ID")
                    GoogleRedirectUri = GlobalValues.REDIRECT_URI, // Обычно в Google Cloud Console изначально это "https://your-project-id.firebaseapp.com/__/auth/handler",
                                                                   // но "__/auth/handler" меняем на "redirect.html",
                                                                   // чтобы получилось "https://your-project-id.firebaseapp.com/redirect.html"
                                                                   // (Google Cloud Console > APIs & Services > Credentials > Auth 2.0 Client IDs > Web client (auto created by Google Service) > Authorized redirect URIs)
                    CallbackScheme = GlobalValues.CALLBACK_SCHEME, // Схема обратного вызова для аутентификации через Google.
                                                                   // Например, "myapp" для myapp:// (но можно и myapp:// - это будет отредактировано в конструкторе класса FirebaseLoginService).
                                                                   // Можно использовать "your_project_id" - имя вашего проекта
                    SecretKey = GlobalValues.SECRET_KEY, // секретный ключ, который выдает Google при регистрации reCAPTCHA (используется только на сервере для проверки токена).
                    FacebookAppId = GlobalValues.FACEBOOK_APP_ID, // Ваш Facebook App ID (Facebook for Developers > My Apps > [Your App] > Settings > Basic > App ID)
                    FacebookRedirectUri = GlobalValues.REDIRECT_URI // Обычно в Google Cloud Console изначально это "https://your-project-id.firebaseapp.com/__/auth/handler",
                                                                    // но "__/auth/handler" меняем на "redirect.html",
                                                                    // чтобы получилось "https://your-project-id.firebaseapp.com/redirect.html"
                                                                    // (Meta for Developers > Панель > Настройте сценарий "Аутентификация и запрос данных у пользователей с помощью функции "Вход через Facebook" > настройки > Действительные URI перенаправления для OAuth)
                });
        });

        builder.Services.AddSingleton<AuthService>();
        builder.Services.AddSingleton<INavigationService, NavigationService>();
        builder.Services.AddSingleton<LocalAuthPreferencesService>();

        builder.Services.AddSingleton<AuthPageViewModel>();
        builder.Services.AddSingleton<SignUpPopupViewModel>();
        builder.Services.AddSingleton<ResetPasswordPopupViewModel>();
        builder.Services.AddSingleton<LoginWithPhoneViewModel>();
        builder.Services.AddSingleton<SignOutPopupViewModel>();
        builder.Services.AddSingleton<SelectEnterMethodPopupViewModel>();

        return builder;
    }
}
