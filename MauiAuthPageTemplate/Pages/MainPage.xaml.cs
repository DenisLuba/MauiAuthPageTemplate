using MauiAuthPageTemplate.Services;
using MauiAuthPageTemplate.Services.Interfaces;
using MauiAuthPageTemplate.Dialogs;

namespace MauiAuthPageTemplate.Pages;

public partial class MainPage : ContentPage
{
    INavigationService _navigationService;
    LocalAuthPreferencesService _preferencesService;

    public MainPage(INavigationService navigation, LocalAuthPreferencesService preferencesService)
    {
        InitializeComponent();

        _navigationService = navigation;
        _preferencesService = preferencesService;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        // Hide the back button on the navigation bar
        NavigationPage.SetHasBackButton(this, false);

        if (await _preferencesService.GetAuthMethodAsync() == LocalAuthMethod.None)
            await _navigationService.PushModalAsync(new SelectEnterMethodPopup());
    }
}
