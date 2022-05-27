using Prism.Mvvm;

namespace LB_EloForte.Models
{
    public class Veiculo: BindableBase
    {
        public decimal Id_veiculo { get; set; }
        private string _ds_veiculo = string.Empty;
        public string Ds_veiculo { get { return _ds_veiculo; } set { SetProperty(ref _ds_veiculo, value); } }
        private string _modelo = string.Empty;
        public string Modelo { get { return _modelo; } set { SetProperty(ref _modelo, value); } }
    }
}
