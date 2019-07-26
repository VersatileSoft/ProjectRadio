using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using Prism.Services;
using ProjectRadio.Services.Interfaces;
using System;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace ProjectRadio.ViewModels
{
    public class MainPageViewModel : BindableBase, INavigatedAware, IConfirmNavigationAsync
    {
        protected INavigationService NavigationService;
        private readonly IPageDialogService _pageDialogService;
        private bool _navigationInProcess = false;

        public bool NavigationInProcess
        {
            get => _navigationInProcess;
            set => SetProperty(ref _navigationInProcess, value);
        }

        public DelegateCommand PlayerCommand { get; set; }
        public DelegateCommand NewsCommand { get; set; }
        public DelegateCommand PodcastsCommand { get; set; }
        public DelegateCommand ReportCommand { get; set; }
        public DelegateCommand SocialMediaCommand { get; set; }
        public DelegateCommand ContactCommand { get; set; }

        public MainPageViewModel(ISettings settings,
            INavigationService navigationService,
            IPageDialogService pageDialogService)
        {
            NavigationService = navigationService;
            _pageDialogService = pageDialogService;

            PlayerCommand = new DelegateCommand(
                () =>
                {
                    NavigationInProcess = true;
                    NavigateTo("PlayerPage", settings[Setting.PlayerStreamUri]);
                }, () => !NavigationInProcess);

            NewsCommand = new DelegateCommand(
                () =>
                {
                    NavigationInProcess = true;
                    NavigateTo("NewsfeedListPage", settings[Setting.NewsUri]);
                }, () => !NavigationInProcess);

            PodcastsCommand = new DelegateCommand(
                () =>
                {
                    NavigationInProcess = true;
                    NavigateTo("PodcastCategoryListPage", settings[Setting.PodcastsUri]);
                }, () => !NavigationInProcess);

            ReportCommand = new DelegateCommand(
                () =>
                {
                    NavigationInProcess = true;
                    NavigateTo("ReportPage", settings[Setting.ReportEmail]);
                }, () => !NavigationInProcess);

            SocialMediaCommand = new DelegateCommand(
                async () =>
                {
                    if (Connectivity.NetworkAccess == NetworkAccess.Internet)
                    {
                        try
                        {
                            Device.OpenUri(new Uri("fb://page/" + (settings[Setting.FacebookPageId] as string)));
                        }
                        catch
                        {
                            await Browser.OpenAsync(new Uri("https://www.facebook.com/" + (settings[Setting.FacebookPageId] as string)), BrowserLaunchMode.External);
                        }
                    }
                    else
                    {
                        await pageDialogService.DisplayAlertAsync(
                            "Błąd", "Połączenie z internetem nie jest możliwe", "Anuluj");
                    }
                }, () => !NavigationInProcess);

            ContactCommand = new DelegateCommand(
                () =>
                {
                    NavigationInProcess = true;
                    NavigateTo("ContactPage");
                }, () => !NavigationInProcess);
        }

        private async void NavigateTo(string Path)
        {
            await NavigationService.NavigateAsync(Path);
        }

        private async void NavigateTo(string Path, object parameter)
        {
            if (parameter == null)
            {
                throw new ArgumentException("Parameter is null");
            }

            NavigationParameters parameters = new NavigationParameters
            {
                { "data", parameter },
                { "path", Path }
            };

            await NavigationService.NavigateAsync(Path, parameters);
        }

        public void OnNavigatedFrom(INavigationParameters parameters)
        {
            NavigationInProcess = false;
        }

        public void OnNavigatedTo(INavigationParameters parameters)
        {
        }

        public Task<bool> CanNavigateAsync(INavigationParameters parameters)
        {
            bool hasInternetConnection = Connectivity.NetworkAccess == NetworkAccess.Internet;

            switch (parameters.GetValue<string>("path"))
            {
                case "PlayerPage":
                case "NewsfeedListPage":
                case "PodcastCategoryListPage":
                case "ReportPage":
                case "SocialMedia":
                    if (!hasInternetConnection)
                    {
                        NavigationInProcess = false;
                        _pageDialogService.DisplayAlertAsync(
                            "Błąd", "Połączenie z internetem nie jest możliwe", "Anuluj");
                    }
                    return Task.FromResult(hasInternetConnection);
                default:
                    return Task.FromResult(true);
            }
        }
    }
}