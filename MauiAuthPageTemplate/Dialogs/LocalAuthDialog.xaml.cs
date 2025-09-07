using MauiAuthPageTemplate.ViewModels;
using MauiAuthPageTemplate.Controls;
using System.Diagnostics;
using System.Threading.Tasks;

namespace MauiAuthPageTemplate.Dialogs;

public partial class LocalAuthDialog : ContentPage
{
    private readonly LocalAuthDialogViewModel _viewModel;   

    public LocalAuthDialog(LocalAuthDialogViewModel viewModel)
    {
        InitializeComponent();

        _viewModel = viewModel;
        BindingContext = _viewModel;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        await _viewModel.InitializeAsync();

        if (Resources["AuthSelector"] is Selectors.AuthTemplateSelector selector)
        {
            try
            {
                var template = selector.SelectTemplate(item: _viewModel, container: this);
                // Вставляем выбранный шаблон в ContentView
                var view = template?.CreateContent() as View ?? throw new NullReferenceException("");
                AuthContentView.Content = view;

                // Подписываемся на событие завершения ввода узора, если выбран шаблон узора
                if (view is PatternLockView patternView)
                {
                    patternView.PatternCompleted += OnPatternCompleted;
                }
                // или на событие завершения ввода PIN-кода, если выбран шаблон PIN-кода
                else if (view is PinCodeLockView pinCodeView)
                {
                    pinCodeView.PinCodeCompleted += OnPinCompleted;
                }
            }
            catch (Exception e)
            {
                Trace.WriteLine($"LocalAuthDialog - OnAppearing: {e.Message}");
            }
        }
    }

    private async void OnPinCompleted(object? sender, string pinCode)
    {
        await _viewModel.HandlePinInputAsync(pinCode);
    }

    private async void OnPatternCompleted(object? sender, string pattern)
    {
        await _viewModel.HandlePatternInputAsync(pattern);
    }
}