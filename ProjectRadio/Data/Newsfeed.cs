using Prism.Mvvm;
using System;

namespace ProjectRadio.ViewModels.Data
{
    public class Newsfeed : BindableBase
    {
        private string _title;
        private string _description;
        private string _simplifiedDescription;
        private string _image;
        private string _pageURL;
        private DateTime _date;

        public string Title
        {
            get => _title;
            set => SetProperty(ref _title, value);
        }

        public string Description
        {
            get => _description;
            set => SetProperty(ref _description, value);
        }

        public string SimplifiedDescription
        {
            get => _simplifiedDescription;
            set => SetProperty(ref _simplifiedDescription, value);
        }

        public DateTime Date
        {
            get => _date;
            set => SetProperty(ref _date, value);
        }

        public string Image
        {
            get => _image;
            set => SetProperty(ref _image, value);
        }

        public string PageURL
        {
            get => _pageURL;
            set => SetProperty(ref _pageURL, value);
        }
    }
}