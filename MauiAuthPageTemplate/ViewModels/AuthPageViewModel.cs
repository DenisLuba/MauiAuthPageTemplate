using MauiAuthPageTemplate.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MauiAuthPageTemplate.Resources.Strings.AuthPageViewModelResources;
using MauiAuthPageTemplate.Dialogs;
using System.Diagnostics;
using MauiShared.Services;

namespace MauiAuthPageTemplate.ViewModels;

public partial class AuthPageViewModel(
    AuthService authService, 
    INavigationService navigation, 
    SignUpPopupViewModel signUpPopupViewModel, 
    ResetPasswordPopupViewModel resetPasswordPopupViewModel, 
    LoginWithPhoneViewModel loginWithPhoneViewModel) : ObservableObject
{
    #region Properties
    [ObservableProperty]
    string _login;

    [ObservableProperty]
    string _password;
    #endregion

    #region SignInCommand
    [RelayCommand]
    public async Task SignInAsync()
    {
        if (!string.IsNullOrWhiteSpace(Login) && !string.IsNullOrWhiteSpace(Password))
        {
            var isAuthorized = await authService.LoginWithEmailAsync(Login, Password);

            if (isAuthorized is Result.Success)
                await Shell.Current.GoToAsync(GlobalValues.MainPage);
            else if (isAuthorized is Result.Failure)
                await Shell.Current.DisplayAlert(ResourcesAuthPageViewModel.error, ResourcesAuthPageViewModel.invalid_login_or_password, "OK");
            else if (isAuthorized is Result.NoInternetConnection)
                await Shell.Current.DisplayAlert(ResourcesAuthPageViewModel.error, ResourcesAuthPageViewModel.no_internet_connection, "OK");
        }
        else
        {
            await Shell.Current.DisplayAlert(ResourcesAuthPageViewModel.error, ResourcesAuthPageViewModel.enter_login_and_password, "OK");
        }
    }
    #endregion

    #region LoginWithGoogleCommand 
    [RelayCommand]
    public async Task LoginWithGoogleAsync()
    {
        try
        {
            var isAuthorized = await authService.LoginWithGoogleAsync();

            if (isAuthorized is Result.Success)
                await Shell.Current.GoToAsync(GlobalValues.MainPage);
            else if (isAuthorized is Result.Failure)
                await Shell.Current.DisplayAlert(ResourcesAuthPageViewModel.error, ResourcesAuthPageViewModel.google_login_error, "OK");
            else if (isAuthorized is Result.NoInternetConnection)
                await Shell.Current.DisplayAlert(ResourcesAuthPageViewModel.error, ResourcesAuthPageViewModel.no_internet_connection, "OK");
        }
        catch (Exception ex)
        {
            Trace.WriteLine(ex.Message);
        }
    }
    #endregion

    #region LoginWithPhoneCommand
    [RelayCommand]
    public async Task LoginWithPhoneAsync()
    {
        var popup = new LoginWithPhonePopup(loginWithPhoneViewModel, navigation);
        await navigation.PushModalAsync(page: popup, animated: true);
    }
    #endregion

    #region LoginWithFacebookCommand
    [RelayCommand]
    public async Task LoginWithFacebookAsync()
    {
        try
        {
            var isAuthorized = await authService.LoginWithFacebookAsync();

            if (isAuthorized is Result.Success)
                await Shell.Current.GoToAsync(GlobalValues.MainPage);
            else if (isAuthorized is Result.Failure)
                await Shell.Current.DisplayAlert(ResourcesAuthPageViewModel.error, ResourcesAuthPageViewModel.facebook_login_error, "OK");
            else if (isAuthorized is Result.NoInternetConnection)
                await Shell.Current.DisplayAlert(ResourcesAuthPageViewModel.error, ResourcesAuthPageViewModel.no_internet_connection, "OK");
        }
        catch (Exception ex)
        {
            Trace.WriteLine(ex.Message);
        }
    }
    #endregion

    #region SignUpCommand
    [RelayCommand]
    public async Task SignUpAsync()
    {
        var popup = new SignUpPopup(signUpPopupViewModel, navigation);
        await navigation.PushModalAsync(page: popup, animated: true);
    }
    #endregion

    #region ResetPasswordCommand
    [RelayCommand]
    public async Task ResetPasswordAsync()
    {
        var popup = new ResetPasswordPopup(resetPasswordPopupViewModel, navigation);
        await navigation.PushModalAsync(page: popup, animated: true);
    }
    #endregion
}
