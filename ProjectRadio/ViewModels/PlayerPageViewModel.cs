using MediaManager;
using MediaManager.Volume;
using Prism.Commands;
using Prism.Navigation;
using Prism.Services;
using ProjectRadio.Services.Implementation;
using ProjectRadio.Services.Interfaces;
using Xamarin.Forms;

namespace ProjectRadio.ViewModels
{
    public class PlayerPageViewModel : PlayerViewModel, INavigatedAware
    {
        private bool _hasLoaded = false;
        private string _cachedMuteImage;
        private bool _muted;
        private int _volume;
        private int _cachedVolume = CrossMediaManager.Current.VolumeManager.CurrentVolume;
        private ImageSource _muteImage;
        private readonly ISettings _settings;

        public int Volume
        {
            get => _volume;
            set
            {
                SetProperty(ref _volume, value);

                if (CrossMediaManager.Current.VolumeManager.CurrentVolume != value)
                {
                    CrossMediaManager.Current.VolumeManager.CurrentVolume = value;
                }

                VolumeChanged(this, new VolumeChangedEventArgs(value, value != 0));
            }
        }

        public int MaxVolume => CrossMediaManager.Current?.VolumeManager.MaxVolume ?? 100;

        public ImageSource MuteImage
        {
            get => _muteImage;
            set => SetProperty(ref _muteImage, value);
        }

        public PlayerPageViewModel(ISettings settings, IPageDialogService pageDialogService) : base(settings, pageDialogService)
        {
            _setupTimer = false;
            _settings = settings;

            ToggleMuteCommand = new DelegateCommand(ToggleMute);

            MuteImage = _settings[Setting.VolumeUpIcon] as string;
        }

        public void ToggleMute()
        {
            _muted = !_muted;

            if (_muted)
            {
                _cachedVolume = Volume;
                Volume = 0;
            }
            else
            {
                Volume = _cachedVolume;
            }
        }

        protected void SetMuteImage()
        {
            string newImage;
            if (Volume == 0)
            {
                newImage = _settings[Setting.VolumeMuteIcon] as string;
            }
            else
            {
                newImage = _settings[Setting.VolumeUpIcon] as string;
            }

            if (newImage != _cachedMuteImage)
            {
                _cachedMuteImage = newImage;
                MuteImage = newImage;
            }
        }

        public void OnNavigatedFrom(INavigationParameters parameters)
        {
            if (CrossMediaManager.Current != null)
            {
                CrossMediaManager.Current.MediaItemChanged -= MediaItemChanged;
                CrossMediaManager.Current.StateChanged -= StateChanged;
                CrossMediaManager.Current.MediaItemFinished -= MediaItemFinished;
                //CrossMediaManager.Current.PlayingChanged -= PlayingChanged;
            }
        }

        public virtual void OnNavigatedTo(INavigationParameters parameters)
        {
            MediaUrl = parameters.GetValue<string>("data");

            if (CrossMediaManager.Current != null)
            {
                CrossMediaManager.Current.VolumeManager.VolumeChanged += VolumeChanged;
            }

            _hasLoaded = true;
            if (Volume == 0)
            {
                Volume = MaxVolume / 2;
                Volume = CrossMediaManager.Current.VolumeManager.CurrentVolume;
            }

            SetupImage();
        }

        private void VolumeChanged(object sender, VolumeChangedEventArgs e)
        {
            if (Volume != e.NewVolume && _hasLoaded && sender != this)
            {
                Volume = e.NewVolume;
            }

            SetMuteImage();
        }
    }
}