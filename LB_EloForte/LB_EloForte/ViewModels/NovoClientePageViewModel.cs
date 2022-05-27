using Acr.UserDialogs;
using LB_EloForte.Models;
using LB_EloForte.Services;
using Prism.Commands;
using Prism.Navigation;
using Prism.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Xamarin.Essentials;

namespace LB_EloForte.ViewModels
{
    public class NovoClientePageViewModel : ViewModelBase
    {
        private string _titulo = string.Empty;
        public string Titulo { get { return _titulo; } set { SetProperty(ref _titulo, value); } }
        private bool _novo = true;
        public bool Novo { get { return _novo; } set { SetProperty(ref _novo, value); } }
        private bool _isFisica = false;
        public bool isFisica { get { return _isFisica; } set { SetProperty(ref _isFisica, value); } }
        private bool _isJuridica = false;
        public bool isJuridica { get { return _isJuridica; } set { SetProperty(ref _isJuridica, value); } }
        private Cliente _cliente;
        public Cliente Cliente { get { return _cliente; } set { SetProperty(ref _cliente, value); } }
        private TipoPessoa _tipopessoacorrente;
        public TipoPessoa TipoPessoaCorrente
        {
            get { return _tipopessoacorrente; }
            set
            {
                SetProperty(ref _tipopessoacorrente, value);
                isFisica = value.Tp_pessoa.Equals("F");
                isJuridica = value.Tp_pessoa.Equals("J");
            }
        }
        private ObservableCollection<TipoPessoa> _tipospessoas;
        public ObservableCollection<TipoPessoa> TiposPessoas { get { return _tipospessoas; } set { SetProperty(ref _tipospessoas, value); } }

        public DelegateCommand BuscarCepCommand { get; }
        public DelegateCommand BuscarCidadeCommand { get; }
        public DelegateCommand SalvarCommand { get; }
        public DelegateCommand CancelarCommand { get; }

        private readonly IPageDialogService dialogService;
        public NovoClientePageViewModel(INavigationService navigationService, IPageDialogService _dialogService)
            : base(navigationService)
        {
            dialogService = _dialogService;

            TiposPessoas = new ObservableCollection<TipoPessoa>();
            TiposPessoas.Add(new TipoPessoa { Tp_pessoa = "J", Tipo_pessoa = "JURIDICA" });
            TiposPessoas.Add(new TipoPessoa { Tp_pessoa = "F", Tipo_pessoa = "FISICA" });

            Titulo = "NOVO CLIENTE";
            Cliente = new Cliente();

            BuscarCepCommand = new DelegateCommand(async () =>
            {
                if (Cliente.Cep.SoNumero().Length != 8)
                {
                    await dialogService.DisplayAlertAsync("Mensagem", "CEP Invalido.", "OK");
                    return;
                }
                if (Connectivity.NetworkAccess == NetworkAccess.Internet)
                {
                    using (UserDialogs.Instance.Loading(title: string.Empty, maskType: MaskType.Black))
                    {
                        var ret = await DataService.GetCEPAsync(Cliente.Cep);
                        if (ret != null)
                        {
                            if (!string.IsNullOrEmpty(ret.logradouro.Trim()))
                                Cliente.Ds_endereco = ret.logradouro;
                            if (!string.IsNullOrEmpty(ret.ibge.Trim()))
                            {
                                IEnumerable<Cidade> cid = await DataService.GetCidadesAsync(ret.ibge);
                                if (cid.Count() > 0)
                                {
                                    Cliente.Cd_cidade = cid.First().Cd_cidade;
                                    Cliente.Ds_cidade = cid.First().Ds_cidade;
                                    Cliente.Uf = cid.First().UF;
                                }
                            }
                            if (!string.IsNullOrEmpty(ret.bairro.Trim()))
                                Cliente.Bairro = ret.bairro;
                        }
                    }
                }
                else
                    await dialogService.DisplayAlertAsync("Mensagem", "Sem conexão com a internet.", "OK");
            });
            BuscarCidadeCommand = new DelegateCommand(async () =>
            {
                await NavigationService.NavigateAsync("ConsultaCidadePage", null);
            });
            SalvarCommand = new DelegateCommand(async () =>
            {
                if (string.IsNullOrWhiteSpace(Cliente.Nm_clifor))
                {
                    await dialogService.DisplayAlertAsync("Mensagem", "Obrigatório informar Razão Social/Nome cliente.", "OK");
                    return;
                }
                if (!Cliente.Cnpj.ValidaCNPJ() && !Cliente.Cpf.ValidaCPF())
                {
                    await dialogService.DisplayAlertAsync("Mensagem", "Obrigatório informar CNPJ ou CPF valido.", "OK");
                    return;
                }
                if (string.IsNullOrWhiteSpace(Cliente.Ds_endereco))
                {
                    await dialogService.DisplayAlertAsync("Mensagem", "Obrigatório informar Rua.", "OK");
                    return;
                }
                if (string.IsNullOrWhiteSpace(Cliente.Numero))
                {
                    await dialogService.DisplayAlertAsync("Mensagem", "Obrigatório informar Numero.", "OK");
                    return;
                }
                if (string.IsNullOrWhiteSpace(Cliente.Bairro))
                {
                    await dialogService.DisplayAlertAsync("Mensagem", "Obrigatório informar Bairro.", "OK");
                    return;
                }
                if (string.IsNullOrWhiteSpace(Cliente.Cd_cidade))
                {
                    await dialogService.DisplayAlertAsync("Mensagem", "Obrigatório informar Cidade.", "OK");
                    return;
                }
                if (TipoPessoaCorrente == null)
                {
                    await dialogService.DisplayAlertAsync("Mensagem", "Obrigatório informar tipo pessoa.", "OK");
                    return;
                }
                try
                {
                    if (Connectivity.NetworkAccess == NetworkAccess.Internet)
                    {
                        using (UserDialogs.Instance.Loading(title: string.Empty, maskType: MaskType.Black))
                        {
                            Cliente.Tp_pessoa = TipoPessoaCorrente.Tp_pessoa;
                            if (await DataService.GravarClienteAsync(Cliente))
                            {
                                await dialogService.DisplayAlertAsync("Mensagem", "Cliente gravado com sucesso.", "OK");
                                await NavigationService.GoBackAsync();
                            }
                            else await dialogService.DisplayAlertAsync("Mensagem", "Erro ao gravar cliente.", "OK");
                        }
                    }
                    else
                        await dialogService.DisplayAlertAsync("Mensagem", "Sem conexão com a internet.", "OK");
                }
                catch (Exception ex)
                { await dialogService.DisplayAlertAsync("Erro", ex.Message.Trim(), "OK"); }
            });
            CancelarCommand = new DelegateCommand(() =>
            {
                if (Novo)
                    Cliente = new Cliente();
            });
        }
    }
}
