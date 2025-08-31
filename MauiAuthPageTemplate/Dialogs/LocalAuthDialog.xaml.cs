using MauiAuthPageTemplate.Services;
using MauiAuthPageTemplate.Services.Interfaces;

namespace MauiAuthPageTemplate.Dialogs;

public partial class LocalAuthDialog : ContentPage
{
    private readonly LocalAuthPreferencesService _authPreferencesService;
    private readonly SecurityService _securityService;
    private readonly INavigationService _navigationService;
    private bool _isInitialized;
    private const string PATTERN_HASH_KEY = "HASH KEY";
    private const string PIN_CODE_HASH_KEY = "PIN CODE HASH KEY";

    public bool IsPattern { get; set; }
    public bool IsPinCode { get; set; }

    public LocalAuthDialog(
        LocalAuthPreferencesService authPreferencesService,
        SecurityService securityService,
        INavigationService navigationService)
    {
        InitializeComponent();

        _authPreferencesService = authPreferencesService;
        _securityService = securityService;
        _navigationService = navigationService;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        _isInitialized = true;

        var methods = await _authPreferencesService.GetAuthMethodAsync();

        var isPattern = methods.HasFlag(LocalAuthMethod.Pattern);
        var isPinCode = methods.HasFlag(LocalAuthMethod.PinCode);

        if (isPattern)
        {
            IsPattern = true;
            PatternView.IsVisible = true;
            PatternView.IsEnabled = true;
            PinCodeView.IsVisible = false;
            PinCodeView.IsEnabled = false;
            PatternView.Clear();
        }
        else if (isPinCode)
        {
            IsPinCode = true;
            PinCodeView.IsVisible = true;
            PinCodeView.IsEnabled = true;
            PatternView.IsVisible = false;
            PatternView.IsEnabled = false;
            PinCodeView.Clear();
        }

        PatternView.PatternCompleted += async (_, pattern) =>
        {
            if (!_isInitialized) return;

            var hash = _securityService.ComputeHash(pattern);
            var isValid = await _securityService.CheckHash(PATTERN_HASH_KEY, hash);

            if (isValid)
            {
                // Close the dialog
                await _navigationService.PopModalAsync(true);
            }
            else
            {
                // Show error message
                await DisplayAlert("Error", "Invalid Pattern. Please try again.", "OK");
                PatternView.Clear();
            }
        };

        PinCodeView.PinCodeCompleted += async (_, pin) =>
        {
            if (!_isInitialized) return;

            var hash = _securityService.ComputeHash(pin);
            var isValid = await _securityService.CheckHash(PIN_CODE_HASH_KEY, hash);
            if (isValid)
            {
                // Close the dialog
                await _navigationService.PopModalAsync(true);
            }
            else
            {
                // Show error message
                await DisplayAlert("Error", "Invalid PIN Code. Please try again.", "OK");
                PinCodeView.Clear();
            }
        };
    }
}