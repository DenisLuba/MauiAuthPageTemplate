using MauiAuthPageTemplate.Services;
using MauiAuthPageTemplate.Services.Interfaces;
using MauiAuthPageTemplate.Dialogs;
using MauiAuthPageTemplate.ViewModels;

namespace MauiAuthPageTemplate.Pages;

public partial class MainPage : ContentPage
{
    LocalAuthDialogViewModel _localAuthDialogViewModel;
    INavigationService _navigationService;
    LocalAuthPreferencesService _preferencesService;
    SelectEnterMethodPopupViewModel _enterMethodPopupViewModel;

    private bool IsShownDialog = false;

    public MainPage(
        LocalAuthDialogViewModel localAuthDialogViewModel,
        INavigationService navigation, 
        LocalAuthPreferencesService preferencesService, 
        SelectEnterMethodPopupViewModel enterMethodPopupViewModel)
    {
        InitializeComponent();

        _localAuthDialogViewModel = localAuthDialogViewModel;
        _navigationService = navigation;
        _preferencesService = preferencesService;
        _enterMethodPopupViewModel = enterMethodPopupViewModel;
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
                _localAuthDialogViewModel,
                _navigationService));


        IsShownDialog = true;
    }
}
