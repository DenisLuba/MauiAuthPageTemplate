using MauiAuthPageTemplate.Services.Interfaces;
using MauiAuthPageTemplate.ViewModels;

namespace MauiAuthPageTemplate.Dialogs;

public partial class SelectEnterMethodPopup : ContentPage
{
	public SelectEnterMethodPopup(SelectEnterMethodPopupViewModel viewModel, INavigationService navigation)
	{
		InitializeComponent();
		BindingContext = viewModel;

		viewModel.LoginMethodSelected += async (_, _) => await navigation.PopModalAsync();
    }
}