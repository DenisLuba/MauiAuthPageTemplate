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
    #endregion

    #region Constructor
    public AppShell(AuthService authService, SignOutPopupViewModel signOutPopupViewModel, INavigationService navigation)
    {
        InitializeComponent();

        _authService = authService;
        _signOutPopupViewModel = signOutPopupViewModel;
        _navigation = navigation;

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
        // Если пользователь уже аутентифицирован с помощью электронной почты,
        // переходим на MainPage. Иначе на AuthPage.
        var nextPage = await _authService.LoginWithEmailAsync() is Result.Success
            ? GlobalValues.MainPage
            : GlobalValues.AuthPage;

        await GoToAsync(nextPage);
    }

    public async Task ShowSignOutPopup()
    {
        var popup = new SignOutPopup(_signOutPopupViewModel, _navigation);
        await _navigation.PushModalAsync(popup, true);
    }

    private async void ShowSignOutDialog(object sender, EventArgs e)
    {
        var popup = new SignOutPopup(_signOutPopupViewModel, _navigation);
        await _navigation.PushModalAsync(popup, true);
    }
}
