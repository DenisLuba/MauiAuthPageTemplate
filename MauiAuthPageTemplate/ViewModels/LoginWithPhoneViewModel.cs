using AuthenticationMaui.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Windows.Input;
using MauiAuthPageTemplate.Resources.Strings.LoginWithPhonePopupViewModelResources;
using MauiAuthPageTemplate.Services;

namespace MauiAuthPageTemplate.ViewModels;

public partial class LoginWithPhoneViewModel(AuthService authService): ObservableObject
{
    #region Private Properties
    private string? SessionInfo { get; set; }
    #endregion

    #region Observable Properties
    [ObservableProperty]
    private bool _isVerificationCodeDialog;
    [ObservableProperty]
    private string _phoneNumber;
    [ObservableProperty]
    private string _code; 
    #endregion

    #region Events
    public EventHandler<bool>? CloseDialogEvent { get; set; } 
    #endregion

    #region RequestVerificationCodeCommand 
    [RelayCommand]
    public async Task RequestVerificationCodeAsync()
    {
        if (string.IsNullOrWhiteSpace(PhoneNumber))
        {
            await Shell.Current.DisplayAlert(ResourcesLoginWithPhoneViewModel.error, ResourcesLoginWithPhoneViewModel.phone_is_empty, "OK");
            return;
        }
        try
        {
            var sessionResult = await authService.RequestVerificationCodeAsync(PhoneNumber);
            if (sessionResult != null)
            {
                IsVerificationCodeDialog = true; // Switch to code entry mode
                SessionInfo = sessionResult.SessionInfo; // Store session info for later use
            }
            else
            {
                await Shell.Current.DisplayAlert(ResourcesLoginWithPhoneViewModel.error, ResourcesLoginWithPhoneViewModel.failed_verification_code, "OK");
            }
        }
        catch (Exception)
        {
            await Shell.Current.DisplayAlert(ResourcesLoginWithPhoneViewModel.error, ResourcesLoginWithPhoneViewModel.failed_verification_code, "OK");
        }
    }
    #endregion

    #region LoginWithCodeCommand
    [RelayCommand]
    public async Task LoginWithCodeAsync()
    {
        if (string.IsNullOrWhiteSpace(Code))
        {
            await Shell.Current.DisplayAlert(ResourcesLoginWithPhoneViewModel.error, ResourcesLoginWithPhoneViewModel.code_is_empty, "OK");
            return;
        }
        if (string.IsNullOrWhiteSpace(SessionInfo))
        {
            await Shell.Current.DisplayAlert(ResourcesLoginWithPhoneViewModel.error, ResourcesLoginWithPhoneViewModel.failed_verification_code, "OK");
            return;
        }
        try
        {
            var isAuthorized = await authService.LoginWithVerificationCodeAsync(SessionInfo, Code);
            if (isAuthorized is Result.Success)
            {
                IsVerificationCodeDialog = false; // Close the dialog
                await Shell.Current.GoToAsync(GlobalValues.MainPage);
                CloseDialogEvent?.Invoke(this, true); // Notify that the dialog should close
            }
            else if (isAuthorized is Result.Failure)
            {
                await Shell.Current.DisplayAlert(ResourcesLoginWithPhoneViewModel.error, ResourcesLoginWithPhoneViewModel.error_verification_code, "OK");
                IsVerificationCodeDialog = false; // We return to the phone number input dialog.
            }
            else if (isAuthorized is Result.NoInternetConnection)
            {
                await Shell.Current.DisplayAlert(ResourcesLoginWithPhoneViewModel.error, ResourcesLoginWithPhoneViewModel.no_internet_connection, "OK");
                IsVerificationCodeDialog = false; // We return to the phone number input dialog.
            }
        }
        catch (Exception)
        {
            await Shell.Current.DisplayAlert(ResourcesLoginWithPhoneViewModel.error, ResourcesLoginWithPhoneViewModel.error_verification_code, "OK");
        }
    }
    #endregion

    #region NavigateToPhoneNumberEntryCommand
    [RelayCommand]
    public void NavigateToPhoneNumberEntry()
    {
        IsVerificationCodeDialog = false; // Switch to phone number entry mode
        PhoneNumber = string.Empty; // Clear the phone number field
        Code = string.Empty; // Clear the code field
    } 
    #endregion

    #region CloseDialogCommand
    [RelayCommand]
    public void CloseDialog()
    {
        CloseDialogEvent?.Invoke(this, false);
    }
    #endregion
}
