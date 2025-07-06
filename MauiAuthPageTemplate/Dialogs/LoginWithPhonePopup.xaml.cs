using MauiAuthPageTemplate.Services.Interfaces;
using MauiAuthPageTemplate.ViewModels;

namespace MauiAuthPageTemplate.Dialogs;

public partial class LoginWithPhonePopup : ContentPage
{
	public LoginWithPhonePopup(LoginWithPhoneViewModel viewModel, INavigationService navigation)
	{
		InitializeComponent();
		BindingContext = viewModel;

		viewModel.CloseDialogEvent += async (_, _) => await navigation.PopModalAsync();
    }
}