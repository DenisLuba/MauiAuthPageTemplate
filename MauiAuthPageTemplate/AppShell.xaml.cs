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
    private readonly SecurityService _securityService;
    private readonly LocalAuthPreferencesService _localAuthPreferencesService;
    #endregion

    #region Constructor
    public AppShell(
        AuthService authService, 
        SignOutPopupViewModel signOutPopupViewModel, 
        INavigationService navigation, 
        SecurityService securityService,
        LocalAuthPreferencesService preferencesService)
    {
        InitializeComponent();

        _authService = authService;
        _signOutPopupViewModel = signOutPopupViewModel;
        _navigation = navigation;
        _securityService = securityService;
        _localAuthPreferencesService = preferencesService;
    }
    #endregion

    #region OnNavigatedTo Override Method
    protected override async void OnNavigatedTo(NavigatedToEventArgs args)
    {
        base.OnNavigatedTo(args);

        // Если пользователь уже аутентифицирован,
        // переходим на страницу с методом входа, который был выбран.
        // Иначе - на AuthPage.
        // Пользователь может выбрать только сочетание методов входа,
        // один из которых должен быть должен быть ПИН-КОД или ПАТТЕРН.
        // Это основной метод входа. Откроется страница с паролем или паттерном.
        // Дополнительно может быть выбран метод входа по отпечатку пальца и/или Face ID.

        var methods = await _localAuthPreferencesService.GetAuthMethodAsync();

        if (methods == LocalAuthMethod.None)
            // Если нет уже выбраных методов входа, значит пользователь открыл приложение в первый раз,
            // тогда переходим на страницу аутентификации
            await GoToAsync(GlobalValues.AuthPage);
        else await GoToAsync(GlobalValues.MainPage)
            .ContinueWith(async _ =>
            {
                await MainThread.InvokeOnMainThreadAsync(async () =>
                {
                    if (methods.HasFlag(LocalAuthMethod.PinCode) || methods.HasFlag(LocalAuthMethod.Pattern))
                        await _navigation.PushModalAsync(new LocalAuthDialog(_localAuthPreferencesService, _securityService, _navigation), true);
                });
            });
    } 
    #endregion

    #region ShowSignOutDialog Method
    /// <summary>
    /// Метод для обработки кнопки sign out во flyout.
    private async void ShowSignOutDialog(object sender, EventArgs e)
    {
        var popup = new SignOutPopup(_signOutPopupViewModel, _navigation);
        await _navigation.PushModalAsync(popup, true);
    } 
    #endregion

    /// <summary>
    /// Method to show the sign-out popup.
    //public async Task ShowSignOutPopup()
    //{
    //    var popup = new SignOutPopup(_signOutPopupViewModel, _navigation);
    //    await _navigation.PushModalAsync(popup, true);
    //}
}