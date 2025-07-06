using MauiAuthPageTemplate.Services.Interfaces;
using MauiAuthPageTemplate.ViewModels;

namespace MauiAuthPageTemplate.Dialogs;

public partial class SignUpPopup : ContentPage
{
	public SignUpPopup(SignUpPopupViewModel viewModel, INavigationService navigation)
	{
		InitializeComponent();
		//Color = Colors.Transparent; // Set the background color to transparent
        BindingContext = viewModel;

        // ������� ��������� ����
        viewModel.RequestClose += async (sender, result) => await navigation.PopModalAsync(); 
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        entryLogin.Focus();
    }
}