using Newtonsoft.Json;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using ProjectRadio.Services.Interfaces;
using ProjectRadio.ViewModels.Data;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Xamarin.Forms;

namespace ProjectRadio.ViewModels
{
    public class NewsFeedListViewModel : BindableBase
    {
        //private readonly PodcastCategory _pocastCache;
        private string _title;
        private bool _isBlocked;
        private bool _isLoading = true;
        private readonly string _newsfeedUrl;
        private readonly INewsfeedManager _newsfeedManager;

        public bool IsLoading
        {
            get => _isLoading;
            set => SetProperty(ref _isLoading, value);
        }

        public string Title
        {
            get => _title;
            set => SetProperty(ref _title, value);
        }

        public bool IsBlocked
        {
            get => _isBlocked;
            set => SetProperty(ref _isBlocked, value);
        }

        public ObservableCollection<Newsfeed> Newsfeeds { get; private set; }
        public DelegateCommand<IReadOnlyList<object>> ItemTappedCommand { get; private set; }

        public NewsFeedListViewModel(INewsfeedManager newsfeedManager, ISettings settings)
        {
            _newsfeedManager = newsfeedManager;

            Newsfeeds = new ObservableCollection<Newsfeed>();

            ItemTappedCommand = new DelegateCommand<IReadOnlyList<object>>(NavigateBlock, (o) => !IsBlocked);

            LoadFeed(settings);
        }

        public async void LoadFeed(ISettings settings)
        {
            foreach (Newsfeed newsfeed in await _newsfeedManager.LoadNewsfeeds(new Uri(settings[Setting.NewsUri].ToString())))
            {
                Newsfeeds.Add(newsfeed);
            }

            IsLoading = false;
        }

        private void NavigateBlock(IReadOnlyList<object> obj)
        {
            IsBlocked = true;

            List<object> list = (List<object>)obj;
            Newsfeed[] news = list.ConvertAll(item => (Newsfeed)item).ToArray();

            NavigateToNewsfeed(news[0]);
        }


        public async void NavigateToNewsfeed(Newsfeed News)
        {
            if (News == null)
            {
                throw new ArgumentException("Given newsfeed is null");
            }

            NavigationParameters parameters = new NavigationParameters
            {
                { "newsfeed", News },
                { "data", _newsfeedUrl }
            };

            string entry = JsonConvert.SerializeObject(parameters);
            IsBlocked = false;
            await Shell.Current.GoToAsync($"appshell/newsfeedlist/newsfeeditem?entry={entry}");
        }

        //public void OnNavigatedFrom(INavigationParameters parameters)
        //{
        //    IsBlocked = false;

        //    if (!parameters.ContainsKey("data"))
        //    {
        //        parameters.Add("data", _newsfeedUrl);
        //    }

        //    if (!parameters.ContainsKey("podcast"))
        //    {
        //        parameters.Add("podcast", _pocastCache);
        //    }
        //}

        //public async void OnNavigatedTo(INavigationParameters parameters)
        //{
        //    if (parameters.TryGetValue("podcast", out _pocastCache) && _pocastCache != null)
        //    {
        //        _newsfeedUrl = _pocastCache.Url.ToString();
        //        Title = "Podcasty";
        //    }
        //    else
        //    {
        //        _newsfeedUrl = parameters.GetValue<string>("data");
        //        Title = "Wiadomości";
        //    }

        //    if (!_newsfeedUrl.EndsWith("feed/"))
        //    {
        //        _newsfeedUrl += "feed/";
        //    }

        //    Newsfeeds.Clear();
        //    foreach (Newsfeed newsfeed in await _newsfeedManager.LoadNewsfeeds(new Uri(_newsfeedUrl)))
        //    {
        //        Newsfeeds.Add(newsfeed);
        //    }
        //    IsLoading = false;
        //}
    }
}