using MauiAuthPageTemplate.Dialogs;
using MauiAuthPageTemplate.ViewModels;
using MauiShared.Services;

namespace MauiAuthPageTemplate;

public partial class AppShell : Shell
{
    #region Private Values
    private readonly SignOutPopupViewModel _signOutPopupViewModel;
    private readonly INavigationService _navigation;
    #endregion

    #region Constructor
    public AppShell(
        SignOutPopupViewModel signOutPopupViewModel, 
        INavigationService navigation)
    {
        InitializeComponent();

        _signOutPopupViewModel = signOutPopupViewModel;
        _navigation = navigation;
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
}