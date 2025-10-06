using MauiAuthPageTemplate.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MauiAuthPageTemplate.Resources.Strings.AuthPageViewModelResources;
using MauiAuthPageTemplate.Dialogs;
using System.Diagnostics;
using MauiShared.Services;
using MauiLocalAuth.ViewModels;
using MauiLocalAuth.Dialogs;

namespace MauiAuthPageTemplate.ViewModels;

public partial class AuthPageViewModel : ObservableObject
{
    #region Properties
    [ObservableProperty]
    string _login;

    [ObservableProperty]
    string _password;
    #endregion

    #region Events
    public static event EventHandler<CompletingAuthEventArgs>? AuthenticationEvent;
    #endregion

    #region Private Variables
    private AuthService _authService;
    private INavigationService _navigation;
    private SignUpPopupViewModel _signUpPopupViewModel;
    private ResetPasswordPopupViewModel _resetPasswordPopupViewModel;
    private LoginWithPhoneViewModel _loginWithPhoneViewModel;
    #endregion

    public AuthPageViewModel(
        AuthService authService,
        INavigationService navigation,
        SignUpPopupViewModel signUpPopupViewModel,
        ResetPasswordPopupViewModel resetPasswordPopupViewModel,
        LoginWithPhoneViewModel loginWithPhoneViewModel)
    {
        _authService = authService;
        _navigation = navigation;
        _signUpPopupViewModel = signUpPopupViewModel;
        _resetPasswordPopupViewModel = resetPasswordPopupViewModel;
        _loginWithPhoneViewModel = loginWithPhoneViewModel;

        AuthenticationEvent += CompletingAuthentication;
    }

    #region SignInCommand
    [RelayCommand]
    public async Task SignInAsync()
    {
        if (!string.IsNullOrWhiteSpace(Login) && !string.IsNullOrWhiteSpace(Password))
        {
            var authResponse = await _authService.LoginWithEmailAsync(Login, Password);
            OnAuthenticationEvent(authResponse.Result, ResourcesAuthPageViewModel.invalid_login_or_password);
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
            var authResponse = await _authService.LoginWithGoogleAsync();
            OnAuthenticationEvent(authResponse.Result, ResourcesAuthPageViewModel.google_login_error);
        }
        catch (Exception ex)
        {
            Trace.WriteLine(ex.Message);
        }
    }
    #endregion

    #region LoginWithFacebookCommand
    [RelayCommand]
    public async Task LoginWithFacebookAsync()
    {
        try
        {
            var authResponse = await _authService.LoginWithFacebookAsync();
            OnAuthenticationEvent(authResponse.Result, ResourcesAuthPageViewModel.facebook_login_error);
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
        var popup = new LoginWithPhonePopup(_loginWithPhoneViewModel, _navigation);
        await _navigation.PushModalAsync(page: popup, animated: true);
    }
    #endregion

    #region SignUpCommand
    [RelayCommand]
    public async Task SignUpAsync()
    {
        var popup = new SignUpPopup(_signUpPopupViewModel, _navigation);
        await _navigation.PushModalAsync(page: popup, animated: true);
    }
    #endregion

    #region ResetPasswordCommand
    [RelayCommand]
    public async Task ResetPasswordAsync()
    {
        var popup = new ResetPasswordPopup(_resetPasswordPopupViewModel, _navigation);
        await _navigation.PushModalAsync(page: popup, animated: true);
    }
    #endregion

    #region OnAuthenticationEvent Method
    public static void OnAuthenticationEvent(Result result, string failure) =>
        AuthenticationEvent?.Invoke(null, new(result, failure)); 
    #endregion

    #region MoveToMainPage Method
    private async void CompletingAuthentication(object? _, CompletingAuthEventArgs args)
    {
        var isAuthorized = args.Result;

        if (isAuthorized is Result.Success)
        {
            await Shell.Current.GoToAsync(GlobalValues.MainPage);

            // Если используется ПИН-КОД или ПАТТЕРН
            if (GlobalValues.USE_PIN_OR_PATTERN)
            {
                var serviceProvider = Application.Current?.Handler?.MauiContext?.Services ?? throw new Exception("Service is null");

                var localAuthDialogViewModel = serviceProvider.GetService<LocalAuthDialogViewModel>();
                var preferencesService = serviceProvider.GetService<LocalAuthPreferencesService>();
                var enterMethodPopupViewModel = serviceProvider.GetService<SelectEnterMethodPopupViewModel>();

                if (preferencesService is not null &&
                    enterMethodPopupViewModel is not null &&
                    _navigation is not null &&
                    localAuthDialogViewModel is not null)
                {

                    await _navigation.PushModalAsync(new SelectEnterMethodPopup(
                        enterMethodPopupViewModel,
                        localAuthDialogViewModel,
                        _navigation));
                }
            }
        }
        else if (isAuthorized is Result.Failure)
            await Shell.Current.DisplayAlert(ResourcesAuthPageViewModel.error, args.Failure, "OK");
        else if (isAuthorized is Result.NoInternetConnection)
            await Shell.Current.DisplayAlert(ResourcesAuthPageViewModel.error, ResourcesAuthPageViewModel.no_internet_connection, "OK");
    }
    #endregion
}

public class CompletingAuthEventArgs(Result result, string? failure) : EventArgs
{
    public Result Result { get; } = result;
    public string? Failure { get; } = failure;
}
