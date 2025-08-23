using MauiAuthPageTemplate.Services.Interfaces;
using MauiAuthPageTemplate.ViewModels;

namespace MauiAuthPageTemplate.Dialogs;

public partial class ResetPasswordPopup : ContentPage
{
    public ResetPasswordPopup(ResetPasswordPopupViewModel viewModel, INavigationService navigation)
    {
        InitializeComponent();
        BindingContext = viewModel;

        viewModel.EmailConfirmed += async (_, _) => await navigation.PopModalAsync();
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();

        if (BindingContext is ResetPasswordPopupViewModel viewModel)
            viewModel.Email = string.Empty;
    }
}