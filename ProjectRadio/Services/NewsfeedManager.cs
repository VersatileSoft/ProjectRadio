using HtmlAgilityPack;
using ProjectRadio.Data;
using ProjectRadio.Services.Interfaces;
using ProjectRadio.ViewModels.Data;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web;

namespace ProjectRadio.Services.Implementation
{
    public class NewsfeedManager : INewsfeedManager
    {
        private IList<Newsfeed> NewsfeedCache { get; set; }
        private Uri LatestNewsfeedUrlCache { get; set; }

        private IList<PodcastCategory> PodcastCache { get; set; }

        #region Newsfeeds
        public async Task<IList<Newsfeed>> LoadNewsfeeds(Uri URL)
        {
            if (LatestNewsfeedUrlCache != URL)
            {
                LatestNewsfeedUrlCache = URL;
                HtmlDocument doc = await new HtmlWeb().LoadFromWebAsync(URL.ToString());
                NewsfeedCache = Load(doc);
            }

            return NewsfeedCache;
        }

        public IList<Newsfeed> Load(HtmlDocument Document)
        {
            HtmlNodeCollection nodes = Document.DocumentNode.SelectNodes("//feed/entry");
            List<Newsfeed> newsfeeds = new List<Newsfeed>();
            foreach (HtmlNode node in nodes)
            {
                newsfeeds.Add(new Newsfeed()
                {
                    Title = HttpUtility.HtmlDecode(node.SelectSingleNode("./title").InnerText),
                    Description = HttpUtility.HtmlDecode(node.SelectSingleNode("./content").InnerHtml),
                    SimplifiedDescription = HttpUtility.HtmlDecode(node.SelectSingleNode("./subtitle").InnerText),
                    Date = DateTime.Parse(node.SelectSingleNode("./published")?.InnerText),
                    Image = HttpUtility.HtmlDecode(node.SelectSingleNode("./content/img")?.Attributes["src"].Value),
                    PageURL = HttpUtility.HtmlDecode(node.SelectSingleNode("./link")?.Attributes["href"].Value),
                });
            }

            return newsfeeds;
        }
        #endregion Newsfeeds

        #region Podcasts
        public async Task<IList<PodcastCategory>> LoadPodcastCategories(Uri URL)
        {
            if (PodcastCache != null)
            {
                return PodcastCache;
            }

            List<PodcastCategory> podcasts = await GetListFilledWithChilden(null, URL);

            if (podcasts.Count > 0)
            {
                PodcastCache = podcasts;
            }

            return podcasts;
        }

        public async Task<List<PodcastCategory>> GetListFilledWithChilden(PodcastCategory Category, Uri BaseUrl)
        {
            if ((BaseUrl == null && Category == null) || (Category != null && !Category.HasSubcategories))
            {
                return null;
            }

            List<PodcastCategory> list = new List<PodcastCategory>();
            List<PodcastCategory> subcategories = await GetCurrentLevelChildrens((Category == null) ? BaseUrl : Category.Url);

            foreach (PodcastCategory item in subcategories)
            {
                item.Parent = Category;
                item.Children = (item.HasSubcategories) ? await GetListFilledWithChilden(item, null) : null;
                list.Add(item);
            }
            return list;
        }

        public async Task<List<PodcastCategory>> GetCurrentLevelChildrens(Uri URL)
        {
            List<PodcastCategory> podcasts = new List<PodcastCategory>();

            HtmlDocument doc = await new HtmlWeb().LoadFromWebAsync(URL.ToString());
            foreach (HtmlNode node in doc.DocumentNode.SelectSingleNode(".//main").SelectNodes("./div"))
            {
                HtmlNode anchor = node.SelectSingleNode(".//a");
                PodcastCategory podcast = new PodcastCategory()
                {
                    Name = anchor.InnerText,
                    HasSubcategories = node.GetAttributeValue("class", "").Contains("cat-parent"),
                    Url = GetUri(anchor.GetAttributeValue("href", "")),
                };
                podcasts.Add(podcast);
            }

            if (podcasts.Count > 0)
            {
                return podcasts;
            }
            else
            {
                return null;
            }
        }
        #endregion Podcasts

        public static Uri GetUri(string URL)
        {
            if (string.IsNullOrWhiteSpace(URL))
            {
                return null;
            }

            try
            {
                return new Uri(URL);
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}