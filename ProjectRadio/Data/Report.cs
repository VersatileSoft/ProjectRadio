using Prism.Mvvm;
using System.Collections.ObjectModel;

namespace ProjectRadio.Data
{
    public class Report : BindableBase
    {
        private string _title;
        private string _description;
        private string _name;
        private string _surname;
        private ObservableCollection<ReportImage> _images = new ObservableCollection<ReportImage>();

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

        public string Name
        {
            get => _name;
            set => SetProperty(ref _name, value);
        }

        public string Surname
        {
            get => _surname;
            set => SetProperty(ref _surname, value);
        }

        public ObservableCollection<ReportImage> Images
        {
            get => _images;
            set => SetProperty(ref _images, value);
        }
    }
}