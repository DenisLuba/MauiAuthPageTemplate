using CommunityToolkit.Mvvm.ComponentModel;
using MauiAuthPageTemplate.Services;
using MauiAuthPageTemplate.Services.Interfaces;
using MauiAuthPageTemplate.Resources.Strings.LocalAuthDialogViewModelResources;
using CommunityToolkit.Mvvm.Input;

namespace MauiAuthPageTemplate.ViewModels;

public partial class LocalAuthDialogViewModel(
        LocalAuthPreferencesService authPreferencesService,
        SecurityService securityService,
        INavigationService navigationService) : ObservableObject
{
    #region Observable Properties
    [ObservableProperty] private bool _isPattern;
    [ObservableProperty] private bool _isPinCode;

    [ObservableProperty] private string _currentAuthTemplateKey = string.Empty;
    #endregion

    #region Constants
    private const string PATTERN_HASH_KEY = "HASH KEY";
    private const string PIN_CODE_HASH_KEY = "PIN CODE HASH KEY";

    public const string PATTERN_TEMPLATE = "PATTERN TEMPLATE";
    public const string PIN_CODE_TEMPLATE = "PIN CODE TEMPLATE";
    #endregion

    #region Private Fields
    private string? _firstInputCache; // временное хранилище для первого ввода
    private bool _isFirstSetup; // это первый вход?
    #endregion

    #region InitializeAsync Method
    /// <summary>
    /// Загружает настройки методов локальной аутентификации и обновляет свойства IsPattern и IsPinCode.
    /// </summary>
    public async Task InitializeAsync()
    {
        var methods = await authPreferencesService.GetAuthMethodAsync();

        IsPattern = methods.HasFlag(LocalAuthMethod.Pattern);
        IsPinCode = methods.HasFlag(LocalAuthMethod.PinCode);

        // Если хэш не установлен, значит это первый вход
        _isFirstSetup = !(await securityService.HasHashAsync(
            IsPattern ? PATTERN_HASH_KEY : PIN_CODE_HASH_KEY));

        CurrentAuthTemplateKey = IsPattern ?
            PATTERN_TEMPLATE : IsPinCode ? 
            PIN_CODE_TEMPLATE : string.Empty;
    }
    #endregion

    #region HandlePatternInputAsync Method
    /// <summary>
    /// Обрабатывает ввод графического узора.
    /// </summary>
    public async Task HandlePatternInputAsync(string pattern)
    {
        if (_isFirstSetup)
        {
            if (_firstInputCache is null)
            {
                // Сохраняем первый ввод во временное хранилище
                _firstInputCache = pattern;
                await Shell.Current.DisplayAlert(ResourcesLocalAuthDialogViewModel.confirmation, ResourcesLocalAuthDialogViewModel.repeat_pattern, "OK");
            }
            else
            {
                // Сверяем второй ввод с первым
                if (_firstInputCache == pattern)
                {
                    var hash = securityService.ComputeHash(pattern);
                    await securityService.SaveHashAsync(PATTERN_HASH_KEY, hash);
                    await navigationService.PopModalAsync(true);
                }
                else
                {
                    _firstInputCache = null; // Сбрасываем кэш
                    await Shell.Current.DisplayAlert(ResourcesLocalAuthDialogViewModel.error, ResourcesLocalAuthDialogViewModel.invalid_pattern, "OK");
                }
            }
        }
        else
        {
            await PatternCompletedAsync(pattern);
        }
    }
    #endregion

    #region PatternCompletedAsync Method
    /// <summary>
    /// Обрабатывает завершение ввода графического узора.
    /// </summary>
    public async Task PatternCompletedAsync(string pattern)
    {
        var hash = securityService.ComputeHash(pattern);
        var isValid = await securityService.CheckHashAsync(PATTERN_HASH_KEY, hash);

        if (isValid)
        {
            await navigationService.PopModalAsync(true);
        }
        else
        {
            await Shell.Current.DisplayAlert(ResourcesLocalAuthDialogViewModel.error, ResourcesLocalAuthDialogViewModel.invalid_pattern, "OK");
        }
    }
    #endregion

    public async Task HandlePinInputAsync(string pinCode)
    {
        if (_isFirstSetup)
        {
            if (_firstInputCache is null)
            {
                // Сохраняем первый ввод во временное хранилище
                _firstInputCache = pinCode;
                await Shell.Current.DisplayAlert(ResourcesLocalAuthDialogViewModel.confirmation, ResourcesLocalAuthDialogViewModel.repeat_pin, "OK");
            }
            else
            {
                if (_firstInputCache == pinCode)
                {
                    var hash = securityService.ComputeHash(pinCode);
                    await securityService.SaveHashAsync(PIN_CODE_HASH_KEY, hash);
                    await navigationService.PopModalAsync(true);
                }
                else
                {
                    _firstInputCache = null; // Сбрасываем кэш
                    await Shell.Current.DisplayAlert(ResourcesLocalAuthDialogViewModel.error, ResourcesLocalAuthDialogViewModel.invalid_pin_code, "OK");
                }
            }
        }
        else
        {
            await PinCodeCompletedAsync(pinCode);
        }
    }

    #region PinCodeCompletedCommand
    /// <summary>
    /// Обрабатывает завершение ввода PIN-кода.
    /// </summary>
    [RelayCommand]
    public async Task PinCodeCompletedAsync(string pinCode)
    {
        var hash = securityService.ComputeHash(pinCode);
        var isValid = await securityService.CheckHashAsync(PIN_CODE_HASH_KEY, hash);

        if (isValid)
        {
            await navigationService.PopModalAsync(true);
        }
        else
        {
            await Shell.Current.DisplayAlert(ResourcesLocalAuthDialogViewModel.error, ResourcesLocalAuthDialogViewModel.invalid_pin_code, "OK");
        }
    }
    #endregion

    [RelayCommand]
    public async Task CloseAsync()
    {
        await Shell.Current.GoToAsync(GlobalValues.AuthPage);
        await navigationService.PopModalAsync(true);
    }
}
