using AuthenticationMaui.Services;
using CommunityToolkit.Maui.Media;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MauiAuthPageTemplate.Pages;
using MauiAuthPageTemplate.Resources.Strings.LoginWithPhonePopupViewModelResources;
using MauiAuthPageTemplate.Services;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows.Input;

namespace MauiAuthPageTemplate.ViewModels;

public partial class LoginWithPhoneViewModel(AuthService authService)  : ObservableObject, INotifyPropertyChanged
{
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
            var result = await authService.RequestVerificationCodeAsync(PhoneNumber, GlobalValues.IS_TEST);
            if (result == Result.Success)
            {
                IsVerificationCodeDialog = true; // Switch to code entry mode
                PhoneNumber = string.Empty; // Clean the entry field
                Code = string.Empty; // Clean the entry field
            }
            else if (result == Result.NoInternetConnection)
            {
                await Shell.Current.DisplayAlert(ResourcesLoginWithPhoneViewModel.error, ResourcesLoginWithPhoneViewModel.no_internet_connection, "OK");
            }
            else 
            {
                await Shell.Current.DisplayAlert(ResourcesLoginWithPhoneViewModel.error, ResourcesLoginWithPhoneViewModel.failed_verification_code, "OK");
            }
        }
        catch (Exception ex)
        {
            Trace.WriteLine($"Error login with Phone: {ex.Message}");
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

        try
        {
            var result = await authService.LoginWithVerificationCodeAsync(Code);
            if (result is Result.Success)
            {
                IsVerificationCodeDialog = false; 
                PhoneNumber = string.Empty;
                Code = string.Empty;
                await Shell.Current.GoToAsync(GlobalValues.MainPage);
                CloseDialogEvent?.Invoke(this, true); // Notify that the dialog should close
            }
            else if (result is Result.NoInternetConnection)
            {
                await Shell.Current.DisplayAlert(ResourcesLoginWithPhoneViewModel.error, ResourcesLoginWithPhoneViewModel.no_internet_connection, "OK");
                IsVerificationCodeDialog = false; // We return to the phone number input dialog.
            }
            else
            {
                await Shell.Current.DisplayAlert(ResourcesLoginWithPhoneViewModel.error, ResourcesLoginWithPhoneViewModel.error_verification_code, "OK");
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
        IsVerificationCodeDialog = false;
        CloseDialogEvent?.Invoke(this, false);
    }
    #endregion

    #region DummyCommand
    [RelayCommand]
    public void Dummy() { }
    #endregion
}
