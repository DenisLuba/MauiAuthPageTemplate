using MauiAuthPageTemplate.ViewModels;

namespace MauiAuthPageTemplate.Pages;

public partial class AuthPage : ContentPage
{
	public AuthPage(AuthPageViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = viewModel;
    }

	protected override void OnAppearing()
	{
		base.OnAppearing();

		entryLogin.Text = string.Empty;
		entryPassword.Text = string.Empty;
    }
}