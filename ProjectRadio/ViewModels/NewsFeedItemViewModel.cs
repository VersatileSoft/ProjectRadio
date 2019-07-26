using HtmlAgilityPack;
using LabelHtml.Forms.Plugin.Abstractions;
using Prism.Mvvm;
using Prism.Services;
using ProjectRadio.Data;
using ProjectRadio.Services.Interfaces;
using ProjectRadio.ViewModels.Data;
using ProjectRadio.Views;
using System;
using System.Text;
using Xamarin.Forms;

namespace ProjectRadio.ViewModels
{
    [QueryProperty("Entry", "entry")]
    public class NewsFeedItemViewModel : BindableBase
    {
        private readonly ISettings _settings;
        private readonly IPageDialogService _pageDialogService;

        private bool _isLoading;
        private string _title;
        private string _json;
        //private readonly string _newsfeedCategoryUrl;
        private Newsfeed _data;
        private View _contentDescription;
        private readonly PodcastCategory _podcastCache;

        //private readonly NavigationParameters param;

        //private readonly List<PodcastCategory> _podcastCategoryCache;

        public string Entry
        {
            get => _json;
            set => SetProperty(ref _json, value);
        }

        //public string Json { get; set; }

        public Newsfeed Data
        {
            get => _data;
            set => SetProperty(ref _data, value);
        }

        public View ContentDescription
        {
            get => _contentDescription;
            set => SetProperty(ref _contentDescription, value);
        }

        public string Title
        {
            get => _title;
            set => SetProperty(ref _title, value);
        }

        public bool IsLoading
        {
            get => _isLoading;
            set => SetProperty(ref _isLoading, value);
        }

        public NewsFeedItemViewModel(ISettings settings, IPageDialogService pageDialogService)
        {
            _settings = settings;
            _pageDialogService = pageDialogService;
            IsLoading = true;

            //NavigationParameters param = JsonConvert.DeserializeObject<NavigationParameters>(Json);

            //Data = param.GetValue<Newsfeed>("newsfeed");
            //param.TryGetValue("podcast", out _podcastCache);
            //_newsfeedCategoryUrl = param.GetValue<string>("data");

            //SetupView();
            //IsLoading = false;
        }

        public async void SetupView()
        {
            StackLayout stack = new StackLayout();

            HtmlDocument doc = new HtmlDocument();
            doc.LoadHtml(Data.Description);

            if (_podcastCache != null)
            {
                try
                {
                    HtmlDocument podcast = await new HtmlWeb().LoadFromWebAsync(Data.PageURL.ToString());
                    foreach (HtmlNode subnode in podcast.DocumentNode
                        .SelectNodes(".//article/script"))
                    {
                        foreach (string substring in subnode.InnerText.Split('"'))
                        {
                            if (!substring.EndsWith(".mp3"))
                            {
                                continue;
                            }

                            stack.Children.Add(new PlayerView(_settings, _pageDialogService)
                            {
                                MediaUrl = substring
                            });
                        }
                    }
                }
                catch (Exception)
                {
                    //await _pageDialogService.DisplayAlertAsync("Błąd", 
                    //    "Wystąpił błąd związany z otwarciem tego podcastu", "Anuluj");
                }
            }

            StringBuilder cache = new StringBuilder();
            foreach (HtmlNode node in doc.DocumentNode.SelectNodes("/p"))
            {
                if (node.InnerHtml.Contains("audioplayer"))
                {
                    node.InnerHtml = node.InnerHtml.Replace("[", "<").Replace("]", ">");
                }

                if (node.InnerText == @"&nbsp;")
                {
                    continue;
                }

                if (node.ChildNodes.Count == 1)
                {
                    if (node.InnerHtml.Contains("script"))
                    {
                        continue;
                    }

                    if (node.InnerHtml.Contains("gallery"))
                    {
                        continue;
                    }
                }

                HtmlNode playerNode = node.SelectSingleNode(".//audioplayer");
                if (playerNode != null)
                {
                    PlayerView player = new PlayerView(_settings, _pageDialogService)
                    {
                        MediaUrl = playerNode.GetAttributeValue("file", "")
                    };

                    if (!string.IsNullOrEmpty(cache.ToString()))
                    {
                        HtmlLabel hLabel = new HtmlLabel
                        {
                            Text = cache.ToString()
                        };
                        stack.Children.Add(hLabel);
                        cache = new StringBuilder();
                    }

                    stack.Children.Add(player);

                }
                else
                {
                    cache.AppendLine(node.OuterHtml);
                }

            }

            if (!string.IsNullOrEmpty(cache.ToString()))
            {
                HtmlLabel label = new HtmlLabel
                {
                    Text = cache.ToString()
                };

                stack.Children.Add(label);
            }

            ContentDescription = stack;
        }

        //public void OnNavigatedFrom(INavigationParameters parameters)
        //{
        //    if (!parameters.ContainsKey("data"))
        //    {
        //        parameters.Add("data", _newsfeedCategoryUrl);
        //    }

        //    if (!parameters.ContainsKey("podcast"))
        //    {
        //        parameters.Add("podcast", _podcastCache);
        //    }
        //}

        //public void OnNavigatedTo()
        //{
        //    Data = param.GetValue<Newsfeed>("newsfeed");
        //    param.TryGetValue("podcast", out _podcastCache);
        //    _newsfeedCategoryUrl = param.GetValue<string>("data");

        //    //if (_podcastCache != null)
        //    //{
        //    //    Title = "Podcast";
        //    //}
        //    //else
        //    //{
        //    //    Title = "Wiadomość";
        //    //}

        //    SetupView();
        //    IsLoading = false;
        //}
    }
}