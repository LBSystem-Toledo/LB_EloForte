using Acr.UserDialogs;
using LB_EloForte.Models;
using LB_EloForte.Services;
using Prism.Commands;
using Prism.Navigation;
using Prism.Services;
using Xamarin.Essentials;

namespace LB_EloForte.ViewModels
{
    public class CancelarOSPageViewModel : ViewModelBase
    {
        private OrdemServico OS { get; set; }
        private string _motivocanc = string.Empty;
        public string MotivoCanc { get { return _motivocanc; } set { SetProperty(ref _motivocanc, value); } }

        public DelegateCommand ConfirmarCommand { get; }

        private readonly IPageDialogService dialogService;
        public CancelarOSPageViewModel(INavigationService navigationService, IPageDialogService _dialogService)
            :base(navigationService)
        {
            dialogService = _dialogService;

            ConfirmarCommand = new DelegateCommand(async () =>
            {
                if(string.IsNullOrWhiteSpace(MotivoCanc))
                {
                    await dialogService.DisplayAlertAsync("Mensagem", "Obrigatório informar motivo cancelamento.", "OK");
                    return;
                }
                var ret = await dialogService.DisplayAlertAsync("Pergunta", "Confirma cancelamento OS?", "SIM", "NÃO");
                if (ret)
                    if (Connectivity.NetworkAccess == NetworkAccess.Internet)
                        using (UserDialogs.Instance.Loading(title: string.Empty, maskType: MaskType.Black))
                        {
                            OS.MotivoCanc = MotivoCanc;
                            await DataService.CancelarOSAsync(OS);
                            await dialogService.DisplayAlertAsync("Mensagem", "OS cancelada com sucesso.", "OK");
                            await navigationService.GoBackAsync();
                        }
                    else await dialogService.DisplayAlertAsync("Mensagem", "Sem conexão com a internet.", "OK");
            });
        }
        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            OS = parameters["OS"] as OrdemServico;
        }
    }
}
