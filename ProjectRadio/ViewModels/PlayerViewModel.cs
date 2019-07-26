using MediaManager;
using MediaManager.Media;
using MediaManager.Playback;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Services;
using ProjectRadio.Services.Interfaces;
using System;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace ProjectRadio.Services.Implementation
{
    public class PlayerViewModel : BindableBase
    {
        #region Properties
        private ImageSource _playImage;
        private string _cachedPlayImage;

        private readonly TimeSpan _fileLength;
        private bool _isDraggingProgressBar = false;
        private bool _isRunning = false;
        private string _mediaUri;
        private string _timeStatus;
        private double _currentProgress;

        private readonly IPageDialogService _pageDialogService;
        private readonly ISettings _settings;

        //private IMediaItem _currentFile;
        private IMediaManager AudioManager => CrossMediaManager.Current;

        protected bool _setupTimer = true;

        public ImageSource PlayImage
        {
            get => _playImage;
            set => SetProperty(ref _playImage, value);
        }

        public bool IsRunning
        {
            get => _isRunning;
            set => SetProperty(ref _isRunning, value);
        }

        public string MediaUrl
        {
            get => _mediaUri;
            set => SetProperty(ref _mediaUri, value);
        }

        public double CurrentProgress
        {
            get => _currentProgress;
            set => SetProperty(ref _currentProgress, value);
        }

        public string TimeStatus
        {
            get => _timeStatus;
            set => SetProperty(ref _timeStatus, value);
        }

        #endregion Properties

        public DelegateCommand TogglePlayerCommand { get; private set; }
        public DelegateCommand ToggleMuteCommand { get; protected set; }

        public DelegateCommand TouchDownCommand { get; private set; }
        public DelegateCommand<double?> TouchUpCommand { get; private set; }

        public PlayerViewModel(ISettings settings, IPageDialogService pageDialogService)
        {
            _pageDialogService = pageDialogService;
            _settings = settings;

            PlayImage = settings[Setting.PlayIcon] as string;

            TogglePlayerCommand = new DelegateCommand(
                () => Toggle(MediaUrl));

            TouchDownCommand = new DelegateCommand(
                () => OnTouchDown());

            TouchUpCommand = new DelegateCommand<double?>(
                (value) => OnTouchUp(value));

            if (AudioManager != null)
            {
                AudioManager.BufferedChanged += PlayingChanged;
                AudioManager.MediaItemChanged += MediaItemChanged;
                AudioManager.MediaItemFinished += MediaItemFinished;
            }
        }

        public void SetupImage()
        {
            if (CrossMediaManager.Current.MediaQueue.Current == null)
            {
                return;
            }

            if (MediaUrl == CrossMediaManager.Current.MediaQueue.Current.MediaUri)
            {
                IsRunning = CrossMediaManager.Current.State == MediaPlayerState.Playing;
                SetImage(IsRunning);
            }
        }

        #region Turn On/Off
        public async void Toggle(string url)
        {
            if (IsRunning)
            {
                TurnOff();
            }
            else
            {
                if (Connectivity.NetworkAccess == NetworkAccess.Internet)
                {
                    if (!IsRunning)
                    {
                        TurnOn(url);
                    }
                }
                else
                {
                    await _pageDialogService.DisplayAlertAsync("Błąd", "Połączenie z internetem nie jest możliwe", "Anuluj");
                }
            }
        }

        private async void TurnOn(string uri)
        {
            MediaItem file = new MediaItem(uri)
            {
                MediaType = MediaType.Hls,
                MediaLocation = MediaLocation.Remote,
                MediaUri = uri
            };

            await AudioManager.Play(file);
            if (Device.RuntimePlatform != Device.iOS)
            {
                CrossMediaManager.Current.NotificationManager?.UpdateNotification(); //(file)?
            }

            SetImage(true);
            IsRunning = true;
        }

        private async void TurnOff()
        {
            await AudioManager.Stop();
            CurrentProgress = 0;
            TimeStatus = "";
            if (Device.RuntimePlatform != Device.iOS)
            {
                CrossMediaManager.Current.NotificationManager?.UpdateNotification(); //off
            }

            SetImage(false);
            IsRunning = false;
        }


        protected void SetImage(bool IsPlaying)
        {
            string newImage;
            if (IsPlaying)
            {
                newImage = _settings[Setting.PauseIcon] as string;
            }
            else
            {
                newImage = _settings[Setting.PlayIcon] as string;
            }

            if (newImage != _cachedPlayImage)
            {
                PlayImage = newImage;
                _cachedPlayImage = newImage;
            }
        }
        #endregion Turn On/Off

        #region Drag progress bar
        private void OnTouchDown()
        {
            _isDraggingProgressBar = true;
        }

        private void OnTouchUp(double? Position)
        {
            _isDraggingProgressBar = false;

            if (_fileLength.TotalMilliseconds > 0)
            {
                TimeSpan time = TimeSpan.FromMilliseconds((Position ?? 0) / 100 * _fileLength.TotalMilliseconds);
                AudioManager.SeekTo(time);
            }
        }
        #endregion Drag progress bar

        protected void PlayingChanged(object sender, BufferedChangedEventArgs e)
        {
            //if (MediaUrl == AudioManager.MediaQueue.Current.MediaUri)
            //{
            //    if (!_isDraggingProgressBar)
            //    {
            //        if (Device.RuntimePlatform == Device.Android)
            //        {
            //            CurrentProgress = e.Position.TotalSeconds;
            //        }
            //        else
            //        {
            //            CurrentProgress = e.Position.TotalSeconds * 100;
            //        }

            //        _fileLength = e.Duration;

            //        if (_setupTimer)
            //        {
            //            if (e.Position.TotalSeconds == 0)
            //            {
            //                TimeStatus = "";
            //            }
            //            else
            //            {
            //                TimeStatus = string.Format("{0} / {1}",
            //                    e.Position.ToString(@"mm\:ss"),
            //                    e.Duration.ToString(@"mm\:ss"));
            //            }
            //        }
            //    }
            //}
        }

        protected void MediaItemChanged(object sender, MediaItemEventArgs e)
        {
            if (MediaUrl != e.MediaItem.MediaUri)
            {
                SetImage(false);
                IsRunning = false;
            }
        }

        protected void StateChanged(object sender, StateChangedEventArgs e)
        {
            SetImage(e.State == MediaPlayerState.Playing);
            if (Device.RuntimePlatform != Device.iOS)
            {
                CrossMediaManager.Current.NotificationManager?.UpdateNotification(); //(_currentFile, e.State)?
            }
        }

        protected void MediaItemFinished(object sender, MediaItemEventArgs e)
        {
            if (MediaUrl == e.MediaItem.MediaUri)
            {
                TurnOff();
                CurrentProgress = 0;
            }
        }
    }
}