using Prism.Mvvm;
using Prism.Navigation;
using Xamarin.Forms;

namespace ProjectRadio.ViewModels
{
    public class ReportPopupViewModel : BindableBase, INavigatedAware
    {
        private ImageSource _image;
        public ImageSource Image
        {
            get => _image;
            set => SetProperty(ref _image, value);
        }

        public void OnNavigatedFrom(INavigationParameters parameters)
        {
        }

        public void OnNavigatedTo(INavigationParameters parameters)
        {
            Image = parameters.GetValue<string>("image");
        }
    }
}