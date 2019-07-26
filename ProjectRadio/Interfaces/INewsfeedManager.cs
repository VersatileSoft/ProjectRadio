using ProjectRadio.Data;
using ProjectRadio.ViewModels.Data;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ProjectRadio.Services.Interfaces
{
    public interface INewsfeedManager
    {
        Task<IList<Newsfeed>> LoadNewsfeeds(Uri URL);
        Task<IList<PodcastCategory>> LoadPodcastCategories(Uri URL);
    }
}