using MauiLocalAuth.Dialogs;
using MauiLocalAuth.ViewModels;
using MauiShared.Services;

namespace MauiAuthPageTemplate.Pages;

public partial class MainPage : ContentPage
{
    public MainPage()
    {
        InitializeComponent();  
        Shell.SetNavBarIsVisible(this, false);
        Shell.Current.FlyoutBehavior = FlyoutBehavior.Disabled;
        Shell.Current.Navigated += (s, e) =>
        {
            if (Shell.Current.CurrentPage is MainPage)
            {
                Shell.SetNavBarIsVisible(this, true);
                Shell.Current.FlyoutBehavior = FlyoutBehavior.Flyout;
            }
        };
    }

    protected override void OnAppearing()
    {
        // Скрыть кнопку "назад" на панели навигации
        NavigationPage.SetHasBackButton(this, false);

        base.OnAppearing();
    }
}
