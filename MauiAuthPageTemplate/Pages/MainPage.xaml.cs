using MauiLocalAuth.Dialogs;
using MauiLocalAuth.ViewModels;
using MauiShared.Services;

namespace MauiAuthPageTemplate.Pages;

public partial class MainPage : ContentPage
{
    public MainPage()
    {
        InitializeComponent();            
    }

    protected override void OnAppearing()
    {
        // Скрыть кнопку "назад" на панели навигации
        NavigationPage.SetHasBackButton(this, false);

        base.OnAppearing();
    }
}
