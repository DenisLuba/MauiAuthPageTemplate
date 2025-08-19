using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MauiAuthPageTemplate.Resources.Strings.SignOutPopupViewModelResources;
using MauiAuthPageTemplate.Services;

namespace MauiAuthPageTemplate.ViewModels;

public partial class SignOutPopupViewModel(AuthService authService, LocalAuthPreferencesService preferencesService) : ObservableObject
{
    #region Events
    public EventHandler? CloseDialogEvent; 
    #endregion

    #region SignOutCommand
    [RelayCommand]
    public async Task SignOutAsync()
    {
        try
        {
            var result = await authService.LogoutAsync();
            preferencesService.ClearAuthMethod();

            if (result.Equals(Result.NoInternetConnection))
                await Shell.Current.DisplayAlert(ResourcesSignOutPopupViewModel.error, ResourcesSignOutPopupViewModel.no_internet_connection, "OK");
            else if (result.Equals(Result.Failure) || result.Equals(Result.UnknownError))
                await Shell.Current.DisplayAlert(ResourcesSignOutPopupViewModel.error, ResourcesSignOutPopupViewModel.error_while_signing_out, "OK");
        }
        finally
        {
            CloseDialogEvent?.Invoke(this, EventArgs.Empty);
            await Shell.Current.GoToAsync(GlobalValues.AuthPage);
        }
    }
    #endregion

    #region CloseDialogCommand
    [RelayCommand]
    public void CloseDialog()
    {
        CloseDialogEvent?.Invoke(this, EventArgs.Empty);
    }
    #endregion

    #region DummyCommand
    [RelayCommand]
    public void Dummy() { } 
    #endregion
}
