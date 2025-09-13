using MauiAuthPageTemplate.ViewModels;
using MauiShared.Services;

namespace MauiAuthPageTemplate.Dialogs;

public partial class SignOutPopup : ContentPage
{
	public SignOutPopup(SignOutPopupViewModel viewModel, INavigationService navigation)
	{
		InitializeComponent();

		BindingContext = viewModel;

		viewModel.CloseDialogEvent += async (_, _) => await navigation.PopModalAsync();
    }
}