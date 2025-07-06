using MauiAuthPageTemplate.Services;
using MauiAuthPageTemplate.Services.Interfaces;
using MauiAuthPageTemplate.ViewModels;
using System.ComponentModel;

namespace MauiAuthPageTemplate
{
    public partial class App : Application, INotifyPropertyChanged
    {
        private AuthService _authService;
        private SignOutPopupViewModel _signOutPopupViewModel;
        private INavigationService _navigation;

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

        public App(AuthService authService, SignOutPopupViewModel signOutPopupViewModel, INavigationService navigation)
        {
            InitializeComponent();
            _authService = authService;
            _signOutPopupViewModel = signOutPopupViewModel;
            _navigation = navigation;
        }

        protected override Window CreateWindow(IActivationState? activationState)
        {
            return new Window(new AppShell(_authService, _signOutPopupViewModel, _navigation));
        }
    }
}