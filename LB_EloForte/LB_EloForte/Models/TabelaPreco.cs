using Prism.Mvvm;

namespace LB_EloForte.Models
{
    public class TabelaPreco: BindableBase
    {
        public string Cd_tabelapreco { get; set; } = string.Empty;
        private string _ds_tabelapreco = string.Empty;
        public string Ds_tabelapreco { get { return _ds_tabelapreco; } set { SetProperty(ref _ds_tabelapreco, value); } }
    }
}
