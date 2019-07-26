using Prism;
using Prism.Autofac;
using Prism.Ioc;
using Prism.Plugin.Popups;
using ProjectRadio.Services.Implementation;
using ProjectRadio.Services.Interfaces;
using ProjectRadio.ViewModels;
using ProjectRadio.Views;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace ProjectRadio
{
    public partial class App : PrismApplication
    {
        public App() : this(null) { }

        public App(IPlatformInitializer initializer) : base(initializer) { }

        protected override void OnInitialized()
        {
            InitializeComponent();

            MainPage = new AppShell();

            Routing.RegisterRoute("appshell", typeof(AppShell));
            Routing.RegisterRoute("player", typeof(PlayerPage));
            Routing.RegisterRoute("appshell/newsfeedlist/newsfeeditem", typeof(NewsfeedItemPage));
            Routing.RegisterRoute("newsfeeditem", typeof(NewsfeedItemPage));
            Routing.RegisterRoute("newsfeedlist", typeof(NewsfeedListPage));
            //Routing.RegisterRoute("podcastlist/podcastitemslist/podcastitem", typeof(PodcastCategoryListPage)); //finish
            Routing.RegisterRoute("report", typeof(ReportPage));
            Routing.RegisterRoute("contact", typeof(ContactPage));
        }

        protected override void RegisterTypes(IContainerRegistry ContainerRegistry)
        {
            ContainerRegistry.Register(typeof(ISettings), typeof(Settings));
            ContainerRegistry.Register(typeof(INewsfeedManager), typeof(NewsfeedManager));

            ContainerRegistry.RegisterPopupNavigationService();

            ContainerRegistry.RegisterForNavigation<AppShell>();
            ContainerRegistry.RegisterForNavigation<ContactPage, ContactViewModel>();
            ContainerRegistry.RegisterForNavigation<PlayerPage, PlayerPageViewModel>();
            ContainerRegistry.RegisterForNavigation<NewsfeedItemPage, NewsFeedItemViewModel>();
            ContainerRegistry.RegisterForNavigation<NewsfeedListPage, NewsFeedListViewModel>();
            ContainerRegistry.RegisterForNavigation<PodcastCategoryListPage, PodcastCategoryListViewModel>();
            ContainerRegistry.RegisterForNavigation<ReportPage, ReportViewModel>();
            ContainerRegistry.RegisterForNavigation<ReportPopup, ReportPopupViewModel>();
        }
    }

    public enum NavigationAction
    {
        Backward,
        Forward
    }
}