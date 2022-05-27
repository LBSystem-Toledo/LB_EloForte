using Acr.UserDialogs;
using LB_EloForte.Services;
using LB_EloForte.Utils;
using Prism.Commands;
using Prism.Navigation;
using Prism.Services;
using Xamarin.Essentials;

namespace LB_EloForte.ViewModels
{
    public class LoginPageViewModel : ViewModelBase
    {
        private string _login = string.Empty;
        public string Login { get { return _login; } set { SetProperty(ref _login, value); } }
        private string _senha = string.Empty;
        public string Senha { get { return _senha; } set { SetProperty(ref _senha, value); } }
        private bool _lembrarsenha = false;
        public bool LembrarSenha
        { get { return _lembrarsenha; } set { SetProperty(ref _lembrarsenha, value); } }

        public DelegateCommand LoginCommand { get; }

        private readonly IPageDialogService dialogService;
        public LoginPageViewModel(INavigationService navigationService, IPageDialogService _dialogService)
            :base(navigationService)
        {
            dialogService = _dialogService;
            LoginCommand = new DelegateCommand(async () =>
            {
                if (string.IsNullOrWhiteSpace(Login))
                {
                    await dialogService.DisplayAlertAsync("Mensagem", "Obrigatório informar LOGIN.", "OK");
                    return;
                }
                if (string.IsNullOrWhiteSpace(Senha))
                {
                    await dialogService.DisplayAlertAsync("Mensagem", "Obrigatório informar SENHA.", "OK");
                    return;
                }
                if (Connectivity.NetworkAccess == NetworkAccess.Internet)
                    using(UserDialogs.Instance.Loading(title: string.Empty, maskType: MaskType.Black))
                    {
                        App.Cd_piloto = await DataService.ValidarLoginAsync(Login, Senha);
                        if (!string.IsNullOrWhiteSpace(App.Cd_piloto))
                        {
                            Arquivo.SetValues(Login, Senha);
                            await NavigationService.NavigateAsync("MainPage");
                        }
                        else await dialogService.DisplayAlertAsync("Mensagem", "Login ou senha invalido.", "OK");
                    }
                else await dialogService.DisplayAlertAsync("Mensagem", "Sem conexão com a internet.", "OK");
            });
        }
        public async override void OnNavigatedTo(INavigationParameters parameters)
        {
            if (Connectivity.NetworkAccess == NetworkAccess.Internet)
                using (UserDialogs.Instance.Loading(title: string.Empty, maskType: MaskType.Black))
                {
                    string valor = Arquivo.GetValues();
                    if (!string.IsNullOrWhiteSpace(valor))
                    {
                        App.Cd_piloto = await DataService.ValidarLoginAsync(valor.Split('|')[0], valor.Split('|')[1]);
                        if (!string.IsNullOrWhiteSpace(App.Cd_piloto))
                            await NavigationService.NavigateAsync("MainPage");
                    }
                }
            else await dialogService.DisplayAlertAsync("Mensagem", "Sem conexão com a internet.", "OK");
        }
    }
}
