using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MauiAuthPageTemplate.Resources.Strings.ResetPasswordPopupViewModelResources;
using MauiAuthPageTemplate.Services;

namespace MauiAuthPageTemplate.ViewModels;

public partial class ResetPasswordPopupViewModel(AuthService authService) : ObservableObject
{

    #region Observable Properties
    [ObservableProperty]
    private string _email;
    #endregion

    #region Events
    public EventHandler? EmailConfirmed;
    #endregion

    #region ResetPasswordCommand
    [RelayCommand]
    public async Task ResetPasswordAsync()
    {
        var isDroped = await authService.SendPasswordResetEmailAsync(Email);
        if (isDroped is Result.Success)
            EmailConfirmed?.Invoke(this, EventArgs.Empty);
        else if (isDroped is Result.Failure)
            await Shell.Current.DisplayAlert(ResourcesResetPasswordPopupViewModel.error, ResourcesResetPasswordPopupViewModel.password_reset_error, "OK");
        else if (isDroped is Result.NoInternetConnection)
            await Shell.Current.DisplayAlert(ResourcesResetPasswordPopupViewModel.error, ResourcesResetPasswordPopupViewModel.no_internet_connection, "OK");
    }
    #endregion

    #region CloseDialogCommand
    [RelayCommand]
    public void CloseDialog()
    {
        EmailConfirmed?.Invoke(this, EventArgs.Empty);
    } 
    #endregion
}
