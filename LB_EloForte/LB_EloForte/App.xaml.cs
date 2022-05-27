using LB_EloForte.ViewModels;
using LB_EloForte.Views;
using Prism;
using Prism.Ioc;
using Xamarin.Essentials.Implementation;
using Xamarin.Essentials.Interfaces;
using Xamarin.Forms;

namespace LB_EloForte
{
    public partial class App
    {
        public static string Cd_piloto { get; set; } = string.Empty;
        public App(IPlatformInitializer initializer)
            : base(initializer)
        {
        }

        protected override async void OnInitialized()
        {
            InitializeComponent();
            await NavigationService.NavigateAsync("LoginPage");
            //await NavigationService.NavigateAsync("NavigationPage/MainPage");
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterSingleton<IAppInfo, AppInfoImplementation>();

            containerRegistry.RegisterForNavigation<NavigationPage>();
            containerRegistry.RegisterForNavigation<MainPage, MainPageViewModel>();
            containerRegistry.RegisterForNavigation<LoginPage, LoginPageViewModel>();
            containerRegistry.RegisterForNavigation<NovaOSPage, NovaOSPageViewModel>();
            containerRegistry.RegisterForNavigation<ListaClientesPage, ListaClientesPageViewModel>();
            containerRegistry.RegisterForNavigation<ListaCidadePage, ListaCidadePageViewModel>();
            containerRegistry.RegisterForNavigation<CancelarOSPage, CancelarOSPageViewModel>();
            containerRegistry.RegisterForNavigation<NovoClientePage, NovoClientePageViewModel>();
        }
    }
}
