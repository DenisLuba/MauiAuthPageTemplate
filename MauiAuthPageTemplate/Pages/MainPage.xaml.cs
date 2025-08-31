using MauiAuthPageTemplate.Services;
using MauiAuthPageTemplate.Services.Interfaces;
using MauiAuthPageTemplate.Dialogs;
using MauiAuthPageTemplate.ViewModels;

namespace MauiAuthPageTemplate.Pages;

public partial class MainPage : ContentPage
{
    INavigationService _navigationService;
    LocalAuthPreferencesService _preferencesService;
    SelectEnterMethodPopupViewModel _enterMethodPopupViewModel;
    SecurityService _securityService;

    private bool IsShownDialog = false;

    public MainPage(
        INavigationService navigation, 
        LocalAuthPreferencesService preferencesService, 
        SelectEnterMethodPopupViewModel enterMethodPopupViewModel, 
        SecurityService securityService)
    {
        InitializeComponent();

        _navigationService = navigation;
        _preferencesService = preferencesService;
        _enterMethodPopupViewModel = enterMethodPopupViewModel;
        _securityService = securityService;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        // Hide the back button on the navigation bar
        NavigationPage.SetHasBackButton(this, false);

        var methods = await _preferencesService.GetAuthMethodAsync();
        IsShownDialog = methods.HasFlag(LocalAuthMethod.Pattern) ||
                        methods.HasFlag(LocalAuthMethod.PinCode) || 
                        IsShownDialog;

        if (!IsShownDialog)
            await _navigationService.PushModalAsync(new SelectEnterMethodPopup(
                _enterMethodPopupViewModel, 
                _navigationService, 
                _preferencesService, 
                _securityService));


        IsShownDialog = true;
    }
}
