using Android.App;
using Android.Content.PM;
using Android.OS;
using Microsoft.Maui.Platform;

namespace MauiAuthPageTemplate;

[Activity(
    Theme = "@style/Maui.SplashTheme", 
    MainLauncher = true, 
    LaunchMode = LaunchMode.SingleTop, 
    ConfigurationChanges = ConfigChanges.ScreenSize 
        | ConfigChanges.Orientation 
        | ConfigChanges.UiMode 
        | ConfigChanges.ScreenLayout 
        | ConfigChanges.SmallestScreenSize 
        | ConfigChanges.Density)]
public class MainActivity : MauiAppCompatActivity
{
    protected override void OnCreate(Android.OS.Bundle? savedInstanceState)
    {
        base.OnCreate(savedInstanceState);

        // Установка флага для полноэкранного режима, чтобы скрыть статус-бар
        //Window?.AddFlags(Android.Views.WindowManagerFlags.Fullscreen);


        // Либо можно использовать следующий код для установки цвета статус-бара:
        
        var color = Microsoft.Maui.Controls.Application.Current?.Resources["StatusBar"] as Color;

        if (color is not null)
        {
            SetStatusBarColor(color);
        }
        else
        {
            // Если цвет не задан, можно установить цвет по умолчанию
            SetStatusBarColor(Colors.Transparent);
        }
        
    }

    private void SetStatusBarColor(Color color)
    {
        if (Window is not null)
        {
            var androidColor = color.ToPlatform();
            if ((int)Build.VERSION.SdkInt >= 28 && (int)Build.VERSION.SdkInt < 35)
                Window.SetStatusBarColor(androidColor); // Устанавливаем цвет статус-бара для Android 9 и ниже
        }
    }
}

// класс для обработки обратного вызова аутентификации через браузер (для авторизации через OAuth2)
[Activity(NoHistory = true, LaunchMode = LaunchMode.SingleTop, Exported = true)]
[IntentFilter(
    [Android.Content.Intent.ActionView],
    Categories = [Android.Content.Intent.CategoryDefault, Android.Content.Intent.CategoryBrowsable],
    DataScheme = GlobalValues.CALLBACK_SCHEME)]
public class WebAuthenticationCallbackActivity : Microsoft.Maui.Authentication.WebAuthenticatorCallbackActivity
{
}
