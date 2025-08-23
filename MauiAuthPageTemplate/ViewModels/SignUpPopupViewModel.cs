using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MauiAuthPageTemplate.Resources.Strings.SignUpPageViewModelResources;
using MauiAuthPageTemplate.Services;
using System.Diagnostics;

namespace MauiAuthPageTemplate.ViewModels;

public partial class SignUpPopupViewModel(AuthService authService) : ObservableObject
{
    #region Properties
    [ObservableProperty]
    private string _username;

    [ObservableProperty]
    private string _email;

    [ObservableProperty]
    private string _password;

    [ObservableProperty]
    private string _confirmPassword;
    #endregion

    #region Events
    public event EventHandler<bool>? RequestClose;
    #endregion

    #region SignUpCommand
    [RelayCommand]
    private async Task SignUpAsync()
    {
        if (string.IsNullOrWhiteSpace(Username) || string.IsNullOrWhiteSpace(Email) || string.IsNullOrWhiteSpace(Password) || string.IsNullOrWhiteSpace(ConfirmPassword))
        {
            await Shell.Current.DisplayAlert(ResourceSignUpPageViewModel.error, ResourceSignUpPageViewModel.fill_in_fields, "OK");
            return;
        }
        if (Password != ConfirmPassword)
        {
            await Shell.Current.DisplayAlert(ResourceSignUpPageViewModel.error, ResourceSignUpPageViewModel.passwords_do_not_match, "OK");
            return;
        }
        if (Password.Length < 6)
        {
            await Shell.Current.DisplayAlert(ResourceSignUpPageViewModel.error, ResourceSignUpPageViewModel.short_password, "OK");
            return;
        }

        var result = await authService.RegisterWithEmailAsync(Username, Email, Password);
        if (result is Result.Success)
        {
            await Shell.Current.GoToAsync(GlobalValues.MainPage);
            RequestClose?.Invoke(this, true);
        }
        else if (result is Result.Failure)
            await Shell.Current.DisplayAlert(ResourceSignUpPageViewModel.error, ResourceSignUpPageViewModel.registration_error_by_email, "OK");
        else if (result is Result.NoInternetConnection)
            await Shell.Current.DisplayAlert(ResourceSignUpPageViewModel.error, ResourceSignUpPageViewModel.no_internet_connection, "OK");
    }
    #endregion

    #region ClosePageCommand 
    [RelayCommand]
    private async Task ClosePageAsync()
    {
        if (Application.Current is not null && Application.Current.Windows.Count > 0)
        {
            var mainPage = Application.Current.Windows[0].Page;
            if (mainPage is not null)
            {
                await mainPage.Navigation.PopToRootAsync();
            }
        }
    }
    #endregion
}
