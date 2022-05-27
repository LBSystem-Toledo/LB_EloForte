using Acr.UserDialogs;
using LB_EloForte.Models;
using LB_EloForte.Services;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using Prism.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Xamarin.Essentials;

namespace LB_EloForte.ViewModels
{
    public class NovaOSPageViewModel : ViewModelBase
    {
        private bool _isExpanded = false;
        public bool isExpanded { get { return _isExpanded; } set { SetProperty(ref _isExpanded, value); } }
        private Cliente _cliente;
        public Cliente Cliente { get { return _cliente; } set { SetProperty(ref _cliente, value); } }
        private Cidade _cidade;
        public Cidade Cidade { get { return _cidade; } set { SetProperty(ref _cidade, value); } }
        private Veiculo _veiculo;
        public Veiculo Veiculo { get { return _veiculo; } set { SetProperty(ref _veiculo, value); } }
        private ObservableCollection<Veiculo> _veiculos;
        public ObservableCollection<Veiculo> Veiculos { get { return _veiculos; } set { SetProperty(ref _veiculos, value); } }
        private TabelaPreco _tabela;
        public TabelaPreco Tabela { get { return _tabela; } set { SetProperty(ref _tabela, value); } }
        private ObservableCollection<TabelaPreco> _tabelas;
        public ObservableCollection<TabelaPreco> Tabelas { get { return _tabelas; } set { SetProperty(ref _tabelas, value); } }
        private Servico _servico;
        public Servico Servico { get { return _servico; } set { SetProperty(ref _servico, value); } }
        private ObservableCollection<Servico> _servicos;
        public ObservableCollection<Servico> Servicos { get { return _servicos; } set { SetProperty(ref _servicos, value); } }
        private Cliente _auxiliar;
        public Cliente Auxiliar { get { return _auxiliar; } set { SetProperty(ref _auxiliar, value); } }
        private ObservableCollection<Cliente> _auxiliares;
        public ObservableCollection<Cliente> Auxiliares { get { return _auxiliares; } set { SetProperty(ref _auxiliares, value); } }
        private OrdemServico _os;
        public OrdemServico OS { get { return _os; } set { SetProperty(ref _os, value); } }

        public DelegateCommand BuscarClienteCommand { get; }
        public DelegateCommand ExpandirClienteCommand { get; }
        public DelegateCommand BuscarCidadeCommand { get; }
        public DelegateCommand SalvarCommand { get; }
        public DelegateCommand BuscarPrecoItem { get; }

        private readonly IPageDialogService dialogService;
        public NovaOSPageViewModel(INavigationService navigationService, IPageDialogService _dialogService)
            :base(navigationService)
        {
            dialogService = _dialogService;
            OS = new OrdemServico();

            BuscarClienteCommand = new DelegateCommand(async () =>
            {
                await NavigationService.NavigateAsync("ListaClientesPage");
            });
            ExpandirClienteCommand = new DelegateCommand(() => isExpanded = !isExpanded);
            BuscarCidadeCommand = new DelegateCommand(async () =>
            {
                await navigationService.NavigateAsync("ListaCidadePage");
            });
            BuscarPrecoItem = new DelegateCommand(async () =>
            {
                if (Servico != null && Tabela != null)
                    if (Connectivity.NetworkAccess == NetworkAccess.Internet)
                        using (UserDialogs.Instance.Loading(title: string.Empty, maskType: MaskType.Black))
                        {
                            try
                            {
                                decimal valor = await DataService.GetPrecoAsync(Servico.Cd_servico, Tabela.Cd_tabelapreco);
                                if (valor > decimal.Zero)
                                    OS.Vl_unitario = valor;
                            }
                            catch { }
                        }
                    else await dialogService.DisplayAlertAsync("Mensagem", "Sem conexão com a internet.", "OK");
            });
            SalvarCommand = new DelegateCommand(async () =>
            {
                try
                {
                    if (string.IsNullOrWhiteSpace(OS.Nm_fazenda))
                    {
                        await dialogService.DisplayAlertAsync("Mensagem", "Obrigatório informar fazenda.", "OK");
                        return;
                    }
                    if (Cidade == null)
                    {
                        await dialogService.DisplayAlertAsync("Mensagem", "Obrigatório informar cidade da fazenda.", "OK");
                        return;
                    }
                    if(Servico == null)
                    {
                        await dialogService.DisplayAlertAsync("Mensagem", "Obrigatório informar serviço prestado.", "OK");
                        return;
                    }
                    if (Cliente != null)
                    {
                        OS.Cd_contratante = Cliente.Cd_clifor;
                        OS.Cd_endcontratante = Cliente.Cd_endereco;
                    }
                    OS.Cd_cidadefazenda = Cidade.Cd_cidade;
                    if (Tabela != null)
                        OS.Cd_tabelapreco = Tabela.Cd_tabelapreco;
                    if (Servico != null)
                        OS.Cd_servico = Servico.Cd_servico;
                    if (Veiculo != null)
                        OS.Id_veiculo = Veiculo.Id_veiculo;
                    if (Auxiliar != null)
                        OS.Cd_auxiliar = Auxiliar.Cd_clifor;
                    OS.Cd_piloto = App.Cd_piloto;
                    if (Connectivity.NetworkAccess == NetworkAccess.Internet)
                        using (UserDialogs.Instance.Loading(title: string.Empty, maskType: MaskType.Black))
                        {
                            if (await DataService.GravarOSAsync(OS))
                            {
                                await dialogService.DisplayAlertAsync("Mensagem", "Ordem Serviço gravada com sucesso.", "OK");
                                await navigationService.GoBackAsync();
                            }
                            else await dialogService.DisplayAlertAsync("Erro", "Erro ao gravar OS.", "OK");
                        }
                    else await dialogService.DisplayAlertAsync("Mensagem", "Sem conexão com a internet.", "OK");
                }
                catch(Exception ex)
                { await dialogService.DisplayAlertAsync("Erro", ex.Message.Trim(), "OK"); }
            });
        }
        public async override void OnNavigatedTo(INavigationParameters parameters)
        {
            try
            {
                if (Connectivity.NetworkAccess == NetworkAccess.Internet)
                {
                    using (UserDialogs.Instance.Loading(title: string.Empty, maskType: MaskType.Black))
                    {
                        var lv = await DataService.GetVeiculosAsync();
                        if (lv != null)
                            Veiculos = new ObservableCollection<Veiculo>(lv);
                        var lt = await DataService.GetTabelasAsync();
                        if (lt != null)
                            Tabelas = new ObservableCollection<TabelaPreco>(lt);
                        var ls = await DataService.GetServicosAsync();
                        if (ls != null)
                            Servicos = new ObservableCollection<Servico>(ls);
                        var aux = await DataService.GetAuxiliaresAsync();
                        if (aux != null)
                            Auxiliares = new ObservableCollection<Cliente>(aux);
                        if (parameters != null)
                        {
                            if (parameters.ContainsKey("CLIENTE"))
                                Cliente = parameters["CLIENTE"] as Cliente;
                            if (parameters.ContainsKey("CIDADE"))
                                Cidade = parameters["CIDADE"] as Cidade;
                            if (parameters.ContainsKey("OS"))
                            {
                                OS = parameters["OS"] as OrdemServico;
                                var lista = await DataService.GetClientesAsync(cd_clifor: OS.Cd_contratante);
                                Cliente = lista?.FirstOrDefault();
                                var cd = await DataService.GetCidadesAsync(Cd_cidade: OS.Cd_cidadefazenda);
                                Cidade = cd.FirstOrDefault(p => p.Cd_cidade == OS.Cd_cidadefazenda);
                                Veiculo = Veiculos.FirstOrDefault(p => p.Id_veiculo == OS.Id_veiculo);
                                Tabela = Tabelas.FirstOrDefault(p => p.Cd_tabelapreco == OS.Cd_tabelapreco);
                                Servico = Servicos.FirstOrDefault(p => p.Cd_servico == OS.Cd_servico);
                                Auxiliar = Auxiliares.FirstOrDefault(p => p.Cd_clifor == OS.Cd_auxiliar);
                            }
                        }
                    }
                }
                else await dialogService.DisplayAlertAsync("Mensagem", "Sem conexão com a internet.", "OK");
            }
            catch { }
        }
    }
}
