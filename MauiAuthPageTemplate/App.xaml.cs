using MauiAuthPageTemplate.Dialogs;
using MauiAuthPageTemplate.Services;
using MauiAuthPageTemplate.Services.Interfaces;
using MauiAuthPageTemplate.ViewModels;
using System.ComponentModel;

namespace MauiAuthPageTemplate
{
    public partial class App : Application, INotifyPropertyChanged
    {
        private readonly SignOutPopupViewModel _signOutPopupViewModel;
        private readonly LocalAuthDialogViewModel _localAuthDialogViewModel;
        private readonly INavigationService _navigation;
        private readonly LocalAuthPreferencesService _localAuthPreferencesService;

        private bool isBusy;
        public bool IsBusy
        {
            get => isBusy;
            set
            {
                if (isBusy != value)
                {
                    isBusy = value;
                    OnPropertyChanged(nameof(IsBusy));
                }
            }
        }

        public App(
            AuthService authService,
            SignOutPopupViewModel signOutPopupViewModel,
            LocalAuthDialogViewModel localAuthDialogViewModel,
            INavigationService navigation,
            LocalAuthPreferencesService preferencesService)
        {
            InitializeComponent();
            _signOutPopupViewModel = signOutPopupViewModel;
            _localAuthDialogViewModel = localAuthDialogViewModel;
            _navigation = navigation;
            _localAuthPreferencesService = preferencesService;
        }

        protected override Window CreateWindow(IActivationState? activationState)
        {
            var shell = new AppShell(
                _signOutPopupViewModel,
                _navigation);

            // Если пользователь уже аутентифицирован,
            // переходим на страницу с методом входа, который был выбран.
            // Иначе - на AuthPage.
            // Пользователь может выбрать только сочетание методов входа,
            // один из которых должен быть должен быть ПИН-КОД или ПАТТЕРН.
            // Это основной метод входа. Откроется страница с паролем или паттерном.
            // Дополнительно может быть выбран метод входа по отпечатку пальца и/или Face ID.
            MainThread.BeginInvokeOnMainThread(async () =>
            {
                var methods = await _localAuthPreferencesService.GetAuthMethodAsync();

                // Если нет уже выбраных методов входа, значит пользователь открыл приложение в первый раз,
                // тогда переходим на страницу аутентификации
                if (methods == LocalAuthMethod.None)
                {
                    await shell.GoToAsync(GlobalValues.AuthPage);
                }
                else
                {
                    await shell.GoToAsync(GlobalValues.MainPage);

                    if (methods.HasFlag(LocalAuthMethod.PinCode) || methods.HasFlag(LocalAuthMethod.Pattern))
                    {
                        await _navigation.PushModalAsync(new LocalAuthDialog(_localAuthDialogViewModel), true);
                    }
                }
            });

            return new Window(shell);
        }
    }
}