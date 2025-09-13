using MauiAuthPageTemplate.ViewModels;
using MauiShared.Services;

namespace MauiAuthPageTemplate.Dialogs;

public partial class LoginWithPhonePopup : ContentPage
{
	public LoginWithPhonePopup(LoginWithPhoneViewModel viewModel, INavigationService navigation)
	{
		InitializeComponent();
		BindingContext = viewModel;

		viewModel.CloseDialogEvent += async (_, _) => await navigation.PopModalAsync();
		viewModel.CleanEntryEvent += (_, _) => Input.InputText = string.Empty;
    }
}