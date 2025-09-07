using MauiAuthPageTemplate.Services;
using MauiAuthPageTemplate.Services.Interfaces;
using MauiAuthPageTemplate.ViewModels;

namespace MauiAuthPageTemplate.Dialogs;

public partial class SelectEnterMethodPopup : ContentPage
{
	public SelectEnterMethodPopup(
		SelectEnterMethodPopupViewModel selectViewModel, 
		LocalAuthDialogViewModel localAuthViewModel,
        INavigationService navigation)
	{
		InitializeComponent();
		BindingContext = selectViewModel;

		selectViewModel.LoginMethodSelected += async (_, _) => await navigation.PopModalAsync();
		selectViewModel.LoginMethodSelected += async (_, _) => await navigation.PushModalAsync(new LocalAuthDialog(localAuthViewModel));
    }
}