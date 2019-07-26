using Prism.Mvvm;
using System;
using System.Collections.Generic;

namespace ProjectRadio.Data
{
    public class PodcastCategory : BindableBase
    {
        private string _name;
        private bool _hasSubcategories;
        private Uri _url;
        private PodcastCategory _parent;
        private IList<PodcastCategory> _children;

        public string Name
        {
            get => _name;
            set => SetProperty(ref _name, value);
        }

        public bool HasSubcategories
        {
            get => _hasSubcategories;
            set => SetProperty(ref _hasSubcategories, value);
        }

        public Uri Url
        {
            get => _url;
            set => SetProperty(ref _url, value);
        }

        public PodcastCategory Parent
        {
            get => _parent;
            set => SetProperty(ref _parent, value);
        }

        public IList<PodcastCategory> Children
        {
            get => _children;
            set => SetProperty(ref _children, value);
        }
    }
}