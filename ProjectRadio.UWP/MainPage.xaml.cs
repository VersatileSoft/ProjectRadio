using Prism;
using Prism.Ioc;

namespace ProjectRadio.UWP
{
    public sealed partial class MainPage
    {
        public MainPage()
        {
            InitializeComponent();

            LoadApplication(new ProjectRadio.App(new UWPInitializer()));
        }

        public class UWPInitializer : IPlatformInitializer
        {
            public void RegisterTypes(IContainerRegistry containerRegistry)
            {
                //containerRegistry.Register(typeof(IFacebookAppService), typeof(FacebookAppService_UWP));
            }
        }
    }
}