using MauiAuthPageTemplate.Dialogs;
using MauiAuthPageTemplate.Services;
using MauiAuthPageTemplate.Services.Interfaces;
using MauiAuthPageTemplate.ViewModels;

namespace MauiAuthPageTemplate;

public partial class AppShell : Shell
{
    #region Private Values
    private readonly AuthService _authService;
    private readonly SignOutPopupViewModel _signOutPopupViewModel;
    private readonly INavigationService _navigation; 
    private readonly LocalAuthPreferencesService _localAuthPreferencesService;
    #endregion

    #region Constructor
    public AppShell(AuthService authService, SignOutPopupViewModel signOutPopupViewModel, INavigationService navigation, LocalAuthPreferencesService preferencesService)
    {
        InitializeComponent();

        _authService = authService;
        _signOutPopupViewModel = signOutPopupViewModel;
        _navigation = navigation;
        _localAuthPreferencesService = preferencesService;

        //BindingContext = this;

        _ = CheckAuthenticationAsync();
        //Dispatcher.Dispatch(async () => // этот способ виснет в фоне. Надо погуглить, узнать причину
        //{
        //    await CheckAuthenticationAsync();
        //});
    } 
    #endregion

    private async Task CheckAuthenticationAsync()
    {
        // Если пользователь уже аутентифицирован,
        // переходим на страницу с методом входа, который был выбран.
        // Иначе - на AuthPage.
        // Пользователь может выбрать только сочетание методов входа,
        // один из которых должен быть должен быть ПИН-КОД или ПАТТЕРН.
        // Это основной метод входа. Откроется страница с паролем или паттерном.
        // Дополнительно может быть выбран метод входа по отпечатку пальца и/или Face ID.
        
        var methods = await _localAuthPreferencesService.GetAuthMethodAsync();
        if (methods == LocalAuthMethod.None)
        {
            await GoToAsync(GlobalValues.AuthPage);
            return;
        }

        Page popup = methods.HasFlag(LocalAuthMethod.Pattern)
            ? new PatternPopup()
            : new PinCodePopup();

        await GoToAsync(GlobalValues.MainPage)
            .ContinueWith(async _ =>
            {
                await MainThread.InvokeOnMainThreadAsync(async () =>
                {
                    await _navigation.PushModalAsync(popup, true);
                });
            });
    }

    /// <summary>
    /// Method to show the sign-out popup.
    //public async Task ShowSignOutPopup()
    //{
    //    var popup = new SignOutPopup(_signOutPopupViewModel, _navigation);
    //    await _navigation.PushModalAsync(popup, true);
    //}

    /// <summary>
    /// Метод для обработки кнопки sign out во flyout.
    private async void ShowSignOutDialog(object sender, EventArgs e)
    {
        var popup = new SignOutPopup(_signOutPopupViewModel, _navigation);
        await _navigation.PushModalAsync(popup, true);
    }
}