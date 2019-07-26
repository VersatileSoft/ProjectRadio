using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using ProjectRadio.Data;
using ProjectRadio.Services.Interfaces;
using System;
using System.Collections.ObjectModel;

namespace ProjectRadio.ViewModels
{
    public class PodcastCategoryListViewModel : BindableBase, INavigatedAware
    {
        private readonly INewsfeedManager _newsfeedManager;
        private readonly INavigationService _navigationService;

        private ObservableCollection<PodcastCategory> _podcasts;
        //private readonly PodcastCategory _parentCategory;

        private bool _isBlocked;
        private bool _isLoading = true;
        private string _dataCache;
        private Uri _podcastUrl;

        public ObservableCollection<PodcastCategory> Podcasts
        {
            get => _podcasts;
            set => SetProperty(ref _podcasts, value);
        }

        public bool IsLoading
        {
            get => _isLoading;
            set => SetProperty(ref _isLoading, value);
        }

        public bool IsBlocked
        {
            get => _isBlocked;
            set => SetProperty(ref _isBlocked, value);
        }

        public DelegateCommand<PodcastCategory> CategoryClickedCommand { get; private set; }

        public PodcastCategoryListViewModel(
            INewsfeedManager newsfeedManager,
            INavigationService navigationService)
        {
            _newsfeedManager = newsfeedManager;
            _navigationService = navigationService;

            Podcasts = new ObservableCollection<PodcastCategory>();

            CategoryClickedCommand = new DelegateCommand<PodcastCategory>(CategoryClicked, (o) => !IsBlocked);
        }

        private async void CategoryClicked(PodcastCategory ClickedPodcast)
        {
            NavigationParameters parameters = new NavigationParameters
            {
                { "podcast", ClickedPodcast },
                { "data", _dataCache },
                { "action", NavigationAction.Forward }
            };

            IsBlocked = true;
            if (ClickedPodcast.HasSubcategories)
            {
                await _navigationService.NavigateAsync("PodcastCategoryListPage", parameters);
            }
            else
            {
                await _navigationService.NavigateAsync("NewsfeedListPage", parameters);
            }
        }

        public void OnNavigatedFrom(INavigationParameters parameters)
        {
            IsBlocked = false;

            if (!parameters.ContainsKey("data"))
            {
                parameters.Add("data", _dataCache);
            }
        }


        public async void OnNavigatedTo(INavigationParameters parameters)
        {
            _dataCache = parameters.GetValue<string>("data");

            if (parameters.TryGetValue("podcast", out PodcastCategory podcast))
            {
                _podcastUrl = podcast.Url;
            }
            else
            {
                _podcastUrl = new Uri(_dataCache);
            }

            Podcasts.Clear();
            if (podcast == null || (podcast.Parent == null && podcast.Children == null))
            {
                foreach (PodcastCategory item in await _newsfeedManager.LoadPodcastCategories(_podcastUrl))
                {
                    Podcasts.Add(item);
                }
            }
            else
            {
                foreach (PodcastCategory item in (podcast.Parent == null) ? podcast.Children : podcast.Parent.Children) // ????
                {
                    Podcasts.Add(item);
                }
            }
            IsLoading = false;
        }
    }
}