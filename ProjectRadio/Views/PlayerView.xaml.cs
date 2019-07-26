using Prism.Services;
using ProjectRadio.Services.Implementation;
using ProjectRadio.Services.Interfaces;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ProjectRadio.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PlayerView : ContentView
    {
        private readonly PlayerViewModel _viewModel;

        public static readonly BindableProperty MediaUrlProperty = BindableProperty
            .Create("MediaUrl", typeof(string), typeof(PlayerView), "",
                propertyChanged: MediaUrlPropertyChanged);

        private static void MediaUrlPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            PlayerView view = (PlayerView)bindable;
            string value = newValue as string ?? "";

            view.MediaUrl = value;
            if (view._viewModel != null)
            {
                view._viewModel.MediaUrl = value;
            }

            view._viewModel.SetupImage();
        }

        public string MediaUrl
        {
            get => (string)GetValue(MediaUrlProperty);
            set => SetValue(MediaUrlProperty, value);
        }

        public PlayerView()
        {
            InitializeComponent();
        }

        public PlayerView(ISettings settings, IPageDialogService pageDialogService)
        {
            InitializeComponent();

            BindingContext = _viewModel = new PlayerViewModel(settings, pageDialogService);
        }
    }
}