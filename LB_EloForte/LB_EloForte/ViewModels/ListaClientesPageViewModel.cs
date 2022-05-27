using Acr.UserDialogs;
using LB_EloForte.Models;
using LB_EloForte.Services;
using Prism.Commands;
using Prism.Navigation;
using Prism.Services;
using System.Collections.ObjectModel;
using Xamarin.Essentials;

namespace LB_EloForte.ViewModels
{
    public class ListaClientesPageViewModel : ViewModelBase
    {
        private string _nmCidade = string.Empty;
        public string nmCidade { get { return _nmCidade; } set { SetProperty(ref _nmCidade, value); } }
        private string _nmCliente = string.Empty;
        public string nmCliente { get { return _nmCliente; } set { SetProperty(ref _nmCliente, value); } }
        private Cliente _corrente;
        public Cliente Corrente { get { return _corrente; } set { SetProperty(ref _corrente, value); } }

        private ObservableCollection<Cliente> _clientes;
        public ObservableCollection<Cliente> Clientes { get { return _clientes; } set { SetProperty(ref _clientes, value); } }

        public DelegateCommand NovoCommand { get; }
        public DelegateCommand BuscarCommand { get; }
        public DelegateCommand<Cliente> SelecionarCommand { get; }

        private readonly IPageDialogService dialogService;
        public ListaClientesPageViewModel(INavigationService navigationService, IPageDialogService _dialogService)
            :base(navigationService)
        {
            dialogService = _dialogService;
            NovoCommand = new DelegateCommand(async () => await NavigationService.NavigateAsync("NovoClientePage"));
            BuscarCommand = new DelegateCommand(async () =>
            {
                try
                {
                    if (Connectivity.NetworkAccess == NetworkAccess.Internet)
                        using (UserDialogs.Instance.Loading(title: string.Empty, maskType: MaskType.Black))
                        {
                            var lista = await DataService.GetClientesAsync(Nm_clifor: nmCliente, Ds_cidade: nmCidade);
                            Clientes = new ObservableCollection<Cliente>(lista);
                        }
                    else await dialogService.DisplayAlertAsync("Mensagem", "Sem conexão com a internet.", "OK");
                }
                catch { }
            });
            SelecionarCommand = new DelegateCommand<Cliente>(async (Cliente c) =>
            {
                if (c == null)
                    await dialogService.DisplayAlertAsync("Mensagem", "Obrigatório selecionar cliente.", "OK");
                else
                {
                    try
                    {
                        NavigationParameters param = new NavigationParameters();
                        param.Add("CLIENTE", c);
                        await NavigationService.GoBackAsync(param);
                    }
                    catch { }
                }
            });
        }
    }
}
