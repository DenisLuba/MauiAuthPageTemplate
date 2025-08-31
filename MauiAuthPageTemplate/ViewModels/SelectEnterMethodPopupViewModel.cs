using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MauiAuthPageTemplate.Services;
using MauiAuthPageTemplate.Services.Interfaces;

namespace MauiAuthPageTemplate.ViewModels;

public partial class SelectEnterMethodPopupViewModel(
    LocalAuthPreferencesService preferencesService,
    SecurityService securityService,
    INavigationService navigationService) : ObservableObject
{
    #region Events
    public EventHandler? LoginMethodSelected;
    #endregion

    #region Observable Properties
    [ObservableProperty]
    private bool _isPatternSelected;
    [ObservableProperty]
    private bool _isPinSelected = true;
    [ObservableProperty]
    private bool _isFaceIdSelected;
    [ObservableProperty]
    private bool _isFingerprintSelected;
    #endregion

    #region ApplyAsyncCommand
    [RelayCommand]
    public async Task ApplyAsync()
    {
        var methods =
            (IsPatternSelected ? LocalAuthMethod.Pattern : LocalAuthMethod.None) |
            (IsPinSelected ? LocalAuthMethod.PinCode : LocalAuthMethod.None) |
            (IsFingerprintSelected ? LocalAuthMethod.Fingerprint : LocalAuthMethod.None) |
            (IsFaceIdSelected ? LocalAuthMethod.FaceId : LocalAuthMethod.None);

        preferencesService.ClearAuthMethod();
        await preferencesService.SetAuthMethodAsync(methods);
        CloseDialog();
    }
    #endregion

    #region ToogleFingerprintCommand
    [RelayCommand]
    public void ToggleFingerprint()
    {
        IsFingerprintSelected = !IsFingerprintSelected;
    }
    #endregion

    #region ToogleFaceIdCommand
    [RelayCommand]
    public void ToggleFaceId()
    {
        IsFaceIdSelected = !IsFaceIdSelected;
    }
    #endregion

    #region CloseDialog
    private void CloseDialog()
    {
        LoginMethodSelected?.Invoke(this, EventArgs.Empty);
    }
    #endregion
}
