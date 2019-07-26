using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using LabelHtml.Forms.Plugin.Droid;
using MediaManager;
using Plugin.CurrentActivity;
using Rg.Plugins.Popup;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace ProjectRadio.Droid
{
    [Activity(MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        public static Context AppContext { get; private set; }

        public override void OnBackPressed()
        {
            if (Popup.SendBackPressed(base.OnBackPressed))
            {
                // Do something if there are some pages in the `PopupStack`
            }
            else
            {
                // Do something if there are not any pages in the `PopupStack`
            }
        }

        protected override void OnCreate(Bundle bundle)
        {
            base.SetTheme(Resource.Style.MainTheme);
            base.OnCreate(bundle);
            AppContext = ApplicationContext;

            Forms.SetFlags("CollectionView_Experimental");

            //Xamarin.FormsMaps.Init(this, bundle);
            HtmlLabelRenderer.Initialize();
            CrossMediaManager.Current.Init(this);
            CrossCurrentActivity.Current.Init(this, bundle);
            Popup.Init(this, bundle);
            Forms.Init(this, bundle);
            FormsMaterial.Init(this, bundle);
            ExperimentalFeatures.Enable(ExperimentalFeatures.EmailAttachments);

            //CachedImageRenderer.Init(enableFastRenderer: true);

            LoadApplication(new App());

            Window.SetStatusBarColor(Android.Graphics.Color.White);
            Window.SetTitleColor(Android.Graphics.Color.Black);
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, Permission[] grantResults)
        {
            Plugin.Permissions.PermissionsImplementation.Current.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
    }
}