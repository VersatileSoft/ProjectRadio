using Prism.Commands;
using Prism.Mvvm;
using Prism.Services;
using ProjectRadio.Services.Interfaces;
using Xamarin.Essentials;

namespace ProjectRadio.ViewModels
{
    public class ContactViewModel : BindableBase
    {
        public DelegateCommand PhoneCallCommand { get; private set; }
        public DelegateCommand SendEmailCommand { get; private set; }
        public DelegateCommand OpenLocationCommand { get; private set; }

        public ContactViewModel(IPageDialogService pageDialogService, ISettings settings)
        {
            PhoneCallCommand = new DelegateCommand(async () =>
            {
                try
                {
                    PhoneDialer.Open(settings[Setting.PhoneNumber] as string);
                }
                catch
                {
                    await pageDialogService.DisplayAlertAsync(
                        "Błąd", "Nie udało się zadzownić, spróbuj ponownie później", "Anuluj");
                }
            });

            SendEmailCommand = new DelegateCommand(async () =>
            {
                try
                {
                    EmailMessage message = new EmailMessage
                    {
                        To = { settings[Setting.ReportEmail] as string },
                    };

                    await Email.ComposeAsync(message);
                }
                catch
                {
                    await pageDialogService.DisplayAlertAsync(
                        "Błąd", "Nie udało się wysłać wiadomości email", "Anuluj");
                }
            });

            OpenLocationCommand = new DelegateCommand(async () =>
            {
                try
                {
                    Placemark placemark = new Placemark
                    {
                        CountryName = "United States",
                        AdminArea = "WA",
                        Thoroughfare = "Microsoft Building 25",
                        Locality = "Redmond"
                    };
                    MapLaunchOptions options = new MapLaunchOptions { Name = "Microsoft Building 25" };

                    await Map.OpenAsync(placemark, options);
                }
                catch
                {
                    await pageDialogService.DisplayAlertAsync(
                        "Błąd", "Nie udało się otworzyć lokalizacji.\nBrak aplikacji map.", "Anuluj");
                }
            });
        }
    }
}