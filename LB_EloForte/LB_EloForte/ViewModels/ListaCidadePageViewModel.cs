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
    public class ListaCidadePageViewModel : ViewModelBase
    {
        private string _nmCidade = string.Empty;
        public string nmCidade { get { return _nmCidade; } set { SetProperty(ref _nmCidade, value); } }
        private string _uf = string.Empty;
        public string UF { get { return _uf; } set { SetProperty(ref _uf, value); } }
        private Cidade _corrente;
        public Cidade Corrente { get { return _corrente; } set { SetProperty(ref _corrente, value); } }

        private ObservableCollection<Cidade> _cidades;
        public ObservableCollection<Cidade> Cidades { get { return _cidades; } set { SetProperty(ref _cidades, value); } }

        public DelegateCommand BuscarCommand { get; }
        public DelegateCommand SelecionarCommand { get; }

        private readonly IPageDialogService dialogService;
        public ListaCidadePageViewModel(INavigationService navigationService, IPageDialogService _dialogService)
            :base(navigationService)
        {
            dialogService = _dialogService;
            BuscarCommand = new DelegateCommand(async () =>
            {
                try
                {
                    if (Connectivity.NetworkAccess == NetworkAccess.Internet)
                    {
                        using (UserDialogs.Instance.Loading(title: string.Empty, maskType: MaskType.Black))
                        {
                            var lista = await DataService.GetCidadesAsync(Ds_cidade: nmCidade, Uf: UF);
                            Cidades = new ObservableCollection<Cidade>(lista);
                        }
                    }
                    else await dialogService.DisplayAlertAsync("Mensagem", "Sem conexão com a internet.", "OK");
                }
                catch { }
            });
            SelecionarCommand = new DelegateCommand(async () =>
            {
                if (Corrente == null)
                    await dialogService.DisplayAlertAsync("Mensagem", "Obrigatório selecionar cidade.", "OK");
                else
                {
                    try
                    {
                        NavigationParameters param = new NavigationParameters();
                        param.Add("CIDADE", Corrente);
                        await NavigationService.GoBackAsync(param);
                    }
                    catch { }
                }
            });
        }
    }
}
