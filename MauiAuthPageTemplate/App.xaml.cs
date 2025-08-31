using MauiAuthPageTemplate.Services;
using MauiAuthPageTemplate.Services.Interfaces;
using MauiAuthPageTemplate.ViewModels;
using System.ComponentModel;

namespace MauiAuthPageTemplate
{
    public partial class App : Application, INotifyPropertyChanged
    {
        private readonly AuthService _authService;
        private readonly SignOutPopupViewModel _signOutPopupViewModel;
        private readonly INavigationService _navigation;
        private readonly SecurityService _securityService;
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
            INavigationService navigation,
            SecurityService securityService,
            LocalAuthPreferencesService preferencesService)
        {
            InitializeComponent();
            _authService = authService;
            _signOutPopupViewModel = signOutPopupViewModel;
            _navigation = navigation;
            _securityService = securityService;
            _localAuthPreferencesService = preferencesService;
        }

        protected override Window CreateWindow(IActivationState? activationState)
        {
            return new Window(new AppShell(_authService, _signOutPopupViewModel, _navigation, _securityService, _localAuthPreferencesService));
        }
    }
}