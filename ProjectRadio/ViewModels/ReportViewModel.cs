using Plugin.Media;
using Plugin.Media.Abstractions;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using Prism.Services;
using ProjectRadio.Data;
using ProjectRadio.Services.Interfaces;
using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Essentials;

namespace ProjectRadio.ViewModels
{
    public class ReportViewModel : BindableBase
    {
        #region Properties
        private readonly IPageDialogService _pageDialogService;
        private readonly INavigationService _navigationService;
        private string _statuteUrl;
        private readonly string _emailName;
        private Report _report;
        private bool _isStatuteAccepted = false;
        private bool _canTakePhoto;
        private bool _canPickPhoto;

        public string StatuteUrl
        {
            get => _statuteUrl;
            set => SetProperty(ref _statuteUrl, value);
        }

        public Report Report
        {
            get => _report;
            set => SetProperty(ref _report, value);
        }

        public bool IsStatueAccepted
        {
            get => _isStatuteAccepted;
            set => SetProperty(ref _isStatuteAccepted, value);
        }

        public bool CanTakePhoto
        {
            get => _canTakePhoto;
            set => SetProperty(ref _canTakePhoto, value);
        }

        public bool CanPickPhoto
        {
            get => _canPickPhoto;
            set => SetProperty(ref _canPickPhoto, value);
        }

        public bool HasItems
        {
            get
            {
                if (Report == null)
                {
                    return false;
                }
                else
                {
                    return Report.Images.Count > 0;
                }
            }
        }
        #endregion Properties

        public DelegateCommand SendReportCommand { get; private set; }
        public DelegateCommand TakePictureCommand { get; private set; }
        public DelegateCommand PickPictureCommand { get; private set; }
        public DelegateCommand<string> ImagePopupCommand { get; private set; }
        public DelegateCommand<string> DeleteImageCommand { get; private set; }

        public ReportViewModel(
            ISettings settings,
            IPageDialogService pageDialogService,
            INavigationService navigationService)
        {
            _pageDialogService = pageDialogService;
            _navigationService = navigationService;
            _emailName = settings[Setting.ReportEmail] as string;
            _statuteUrl = settings[Setting.StatuteUri] as string;

            Report = new Report();

            SendReportCommand = new DelegateCommand(
               async () => await SendReport(Report), CanSendReport)
                    .ObservesProperty(() => IsStatueAccepted)
                    .ObservesProperty(() => Report.Name)
                    .ObservesProperty(() => Report.Surname)
                    .ObservesProperty(() => Report.Title)
                    .ObservesProperty(() => Report.Description);

            TakePictureCommand = new DelegateCommand(
                async () => Report.Images.Add(new ReportImage { Url = await TakePicture() }),
                      () => CanTakePhoto);

            PickPictureCommand = new DelegateCommand(
                async () => Report.Images.Add(new ReportImage { Url = await PickPicture() }),
                      () => CanPickPhoto);

            ImagePopupCommand = new DelegateCommand<string>(OpenPopup);

            DeleteImageCommand = new DelegateCommand<string>(
                (image) => Report.Images.Remove(Report.Images.First((rep) => rep.Url == image)));

            InitializeCamera();
        }

        private bool CanSendReport()
        {
            if (!IsStatueAccepted ||
                string.IsNullOrEmpty(Report.Name) ||
                string.IsNullOrEmpty(Report.Surname) ||
                string.IsNullOrEmpty(Report.Title) ||
                string.IsNullOrEmpty(Report.Description))
            {
                return false;
            }

            return true;
        }

        private async void OpenPopup(string Source)
        {
            NavigationParameters parameters = new NavigationParameters
            {
                { "image", Source }
            };

            await _navigationService.NavigateAsync("ReportPopup", parameters);
        }

        public async void InitializeCamera()
        {
            await CrossMedia.Current.Initialize();

            CanTakePhoto =
                CrossMedia.Current.IsCameraAvailable &&
                CrossMedia.Current.IsTakePhotoSupported;

            CanPickPhoto =
                CrossMedia.Current.IsPickPhotoSupported;
        }

        // Old Code
        //
        //public async void SendReport(Report ReportToSend) //FIXME move to service?
        //{
        //    if (Connectivity.NetworkAccess != NetworkAccess.Internet)
        //    {
        //        await _pageDialogService.DisplayAlertAsync(
        //            "Błąd", "Połączenie z internetem nie jest możliwe", "Anuluj");
        //        return;
        //    }

        //    IEmailTask task = SendEmail.Current.EmailMessenger;

        //    if (!task.CanSendEmail)
        //    {
        //        await _pageDialogService.DisplayAlertAsync(
        //            "Błąd", "Nie można wysyłać wiadomości email na tym urządzeniu", "Anuluj");
        //        return;
        //    }

        //    EmailMessageBuilder email = new EmailMessageBuilder()
        //        .To(_emailName)
        //        .Subject(ReportToSend.Title)
        //        .Body(ReportToSend.Description + "\n" + ReportToSend.Name + " " + ReportToSend.Surname);

        //    if (task.CanSendEmailAttachments)
        //    {
        //        foreach (ReportImage imagePath in ReportToSend.Images ?? new ObservableCollection<ReportImage>())
        //        {
        //            string[] split = imagePath.Url.Split('.');
        //            string last = split[split.Length - 1];
        //            email.WithAttachment(imagePath.Url, @"image/" +
        //                ((last == "jpg") ? "jpeg" : last));
        //        }
        //    }

        //    task.SendEmail(email.Build());
        //}

        public async Task SendReport(Report ReportToSend) //FIXME move to service? https://docs.microsoft.com/pl-pl/xamarin/essentials/email?context=xamarin%2Fxamarin-forms&tabs=android
        {                                                 //Dodawanie załączników nie działa
            if (Connectivity.NetworkAccess != NetworkAccess.Internet)
            {
                await _pageDialogService.DisplayAlertAsync(
                    "Błąd", "Połączenie z internetem nie jest możliwe", "Anuluj");
                return;
            }
            else
            {
                try
                {
                    EmailMessage message = new EmailMessage
                    {
                        To = _emailName.Split().ToList(),
                        Subject = ReportToSend.Title,
                        Body = ReportToSend.Description + "\n" + ReportToSend.Name + " " + ReportToSend.Surname
                    };

                    foreach (ReportImage imagePath in ReportToSend.Images ?? new ObservableCollection<ReportImage>())
                    {
                        string[] split = imagePath.Url.Split('.');
                        string last = split[split.Length - 1];

                        string file = Path.Combine(imagePath.Url, @"image/" + ((last == "jpg") ? "jpeg" : last));
                        File.WriteAllText(file, "Hello World");

                        message.Attachments.Add(new EmailAttachment(file));
                    }

                    await Email.ComposeAsync(message);
                }
                catch (FeatureNotSupportedException)
                {
                    await _pageDialogService.DisplayAlertAsync(
                        "Błąd", "Nie można wysyłać wiadomości email na tym urządzeniu", "Anuluj");
                    return;
                }
                catch (Exception exc)
                {
                    await _pageDialogService.DisplayAlertAsync(
                        "Błąd", "Nie udało się wysłać wiadomości email\n\n" + exc.ToString(), "Anuluj");
                    return;
                }
            }
        }

        public async Task<string> TakePicture()
        {
            StoreCameraMediaOptions options = new StoreCameraMediaOptions()
            {
                Directory = "Pictures",
                SaveToAlbum = true
            };

            MediaFile file = await CrossMedia.Current.TakePhotoAsync(options);
            if (file == null)
            {
                return null;
            }

            return file.Path;
        }

        public async Task<string> PickPicture()
        {
            MediaFile file = await CrossMedia.Current.PickPhotoAsync();
            if (file == null)
            {
                return null;
            }

            return file.Path;
        }
    }
}