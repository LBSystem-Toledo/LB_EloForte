using Acr.UserDialogs;
using LB_EloForte.Interface;
using LB_EloForte.Models;
using LB_EloForte.Services;
using LB_EloForte.Utils;
using Prism.Commands;
using Prism.Navigation;
using Prism.Services;
using System;
using System.Collections.ObjectModel;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace LB_EloForte.ViewModels
{
    public class MainPageViewModel : ViewModelBase
    {
        private string Cd_contratante { get; set; } = string.Empty;
        private string Cd_cidade { get; set; } = string.Empty;

        private string _nm_contratante;
        public string Nm_contratante { get { return _nm_contratante; } set { SetProperty(ref _nm_contratante, value); } }
        private string _ds_cidade;
        public string Ds_cidade { get { return _ds_cidade; } set { SetProperty(ref _ds_cidade, value); } }
        private string _nm_fazenda = string.Empty;
        public string Nm_fazenda { get { return _nm_fazenda; } set { SetProperty(ref _nm_fazenda, value); } }
        private DateTime _dt_ini = DateTime.Now;
        public DateTime Dt_ini { get { return _dt_ini; } set { SetProperty(ref _dt_ini, value); } }
        private DateTime _dt_fin = DateTime.Now;
        public DateTime Dt_fin { get { return _dt_fin; } set { SetProperty(ref _dt_fin, value); } }

        private ObservableCollection<OrdemServico> _os;
        public ObservableCollection<OrdemServico> OS { get { return _os; } set { SetProperty(ref _os, value); } }

        public DelegateCommand NovoCommand { get; }
        public DelegateCommand<OrdemServico> AlterarCommand { get; }
        public DelegateCommand<OrdemServico> ExcluirCommand { get; }
        public DelegateCommand BuscarCommand { get; }
        public DelegateCommand BuscarClienteCommand { get; }
        public DelegateCommand BuscarCidadeCommand { get; }
        public DelegateCommand SairCommand { get; }

        private readonly IPageDialogService dialogService;
        public MainPageViewModel(INavigationService navigationService, IPageDialogService _dialogService)
            : base(navigationService)
        {
            dialogService = _dialogService;
            NovoCommand = new DelegateCommand(async () =>
            {
                await navigationService.NavigateAsync("NovaOSPage");
            });
            AlterarCommand = new DelegateCommand<OrdemServico>(async (OrdemServico os) =>
            {
                if (os != null)
                {
                    NavigationParameters param = new NavigationParameters();
                    param.Add("OS", os);
                    await navigationService.NavigateAsync("NovaOSPage", param);
                }
            });
            ExcluirCommand = new DelegateCommand<OrdemServico>(async (OrdemServico os) =>
            {
                if (os != null)
                {
                    NavigationParameters param = new NavigationParameters();
                    param.Add("OS", os);
                    await navigationService.NavigateAsync("CancelarOSPage", param);
                }
            });
            BuscarCommand = new DelegateCommand(async () =>
            {
                try
                {
                    if (Connectivity.NetworkAccess == NetworkAccess.Internet)
                        using (UserDialogs.Instance.Loading(title: string.Empty, maskType: MaskType.Black))
                        {
                            var lista = await DataService.GetOrdemServicosAsync(Cd_contratante, Cd_cidade, Nm_fazenda, Dt_ini.ToString("dd/MM/yyyy"), Dt_fin.ToString("dd/MM/yyyy"));
                            OS = new ObservableCollection<OrdemServico>(lista);
                        }
                    else await dialogService.DisplayAlertAsync("Mensagem", "Sem conexão com a internet.", "OK");
                }
                catch(Exception ex) { await dialogService.DisplayAlertAsync("Erro", ex.Message.Trim(), "OK"); }
            });
            BuscarClienteCommand = new DelegateCommand(async () =>
            {
                await NavigationService.NavigateAsync("ListaClientesPage");
            });
            BuscarCidadeCommand = new DelegateCommand(async () =>
            {
                await navigationService.NavigateAsync("ListaCidadePage");
            });
            SairCommand = new DelegateCommand(() =>
            {
                Arquivo.DeleteFile();
                DependencyService.Get<ICloseApplication>()?.closeApplication();
            });
        }
        public async override void OnNavigatedTo(INavigationParameters parameters)
        {
            if (Connectivity.NetworkAccess == NetworkAccess.Internet)
                using (UserDialogs.Instance.Loading(title: string.Empty, maskType: MaskType.Black))
                {
                    var lista = await DataService.GetOrdemServicosAsync(Dt_ini: Dt_ini.ToString("dd/MM/yyyy"),
                                                                    Dt_fin: Dt_fin.ToString("dd/MM/yyyy"));
                    OS = new ObservableCollection<OrdemServico>(lista);
                    if (parameters != null)
                    {
                        if (parameters.ContainsKey("CLIENTE"))
                        {
                            Cd_contratante = (parameters["CLIENTE"] as Cliente).Cd_clifor;
                            Nm_contratante = (parameters["CLIENTE"] as Cliente).Nm_clifor;
                        }
                        if (parameters.ContainsKey("CIDADE"))
                        {
                            Cd_cidade = (parameters["CIDADE"] as Cidade).Cd_cidade;
                            Ds_cidade = (parameters["CIDADE"] as Cidade).Ds_cidade;
                        }
                    }
                }
            else await dialogService.DisplayAlertAsync("Mensagem", "Sem conexão com a internet.", "OK");
        }
    }
}
