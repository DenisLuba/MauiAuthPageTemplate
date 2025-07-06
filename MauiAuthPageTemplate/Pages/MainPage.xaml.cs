namespace MauiAuthPageTemplate.Pages;

public partial class MainPage : ContentPage
{
    public MainPage()
    {
        InitializeComponent();
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        // Hide the back button on the navigation bar
        NavigationPage.SetHasBackButton(this, false);
    }
}
