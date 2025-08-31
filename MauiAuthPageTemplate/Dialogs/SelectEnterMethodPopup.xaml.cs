using MauiAuthPageTemplate.Services;
using MauiAuthPageTemplate.Services.Interfaces;
using MauiAuthPageTemplate.ViewModels;

namespace MauiAuthPageTemplate.Dialogs;

public partial class SelectEnterMethodPopup : ContentPage
{
	public SelectEnterMethodPopup(
		SelectEnterMethodPopupViewModel viewModel, 
		INavigationService navigation,
		LocalAuthPreferencesService preferencesService,
		SecurityService securityService)
	{
		InitializeComponent();
		BindingContext = viewModel;

		viewModel.LoginMethodSelected += async (_, _) => await navigation.PopModalAsync();
		viewModel.LoginMethodSelected += async (_, _) => await navigation.PushModalAsync(new LocalAuthDialog(preferencesService, securityService, navigation));
    }
}