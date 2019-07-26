using Foundation;
using LabelHtml.Forms.Plugin.iOS;
using Prism;
using Prism.Ioc;
using UIKit;

namespace ProjectRadio.iOS
{
    // The UIApplicationDelegate for the application. This class is responsible for launching the 
    // User Interface of the application, as well as listening (and optionally responding) to 
    // application events from iOS.
    [Register("AppDelegate")]
    public partial class AppDelegate : Xamarin.Forms.Platform.iOS.FormsApplicationDelegate
    {
        public static UIApplication Instance { get; private set; }

        // This method is invoked when the application has loaded and is ready to run. In this 
        // method you should instantiate the window, load the UI into it and then make the window
        // visible.
        //
        // You have 17 seconds to return from this method, or iOS will terminate your application.
        public override bool FinishedLaunching(UIApplication app, NSDictionary options)
        {
            Rg.Plugins.Popup.Popup.Init();

            Xamarin.Forms.Forms.Init();
            LoadApplication(new App(new IOSInitializer()));

            HtmlLabelRenderer.Initialize();

            Instance = app;

            return base.FinishedLaunching(app, options);
        }

        public class IOSInitializer : IPlatformInitializer
        {
            public void RegisterTypes(IContainerRegistry containerRegistry)
            {
                //containerRegistry.Register(
                //    typeof(IFacebookAppService),
                //    typeof(FacebookAppService_iOS));
            }
        }
    }
}