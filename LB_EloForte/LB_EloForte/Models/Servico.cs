using Prism.Mvvm;

namespace LB_EloForte.Models
{
    public class Servico: BindableBase
    {
        public string Cd_servico { get; set; } = string.Empty;
        private string _ds_servico = string.Empty;
        public string Ds_servico { get { return _ds_servico; } set { SetProperty(ref _ds_servico, value); } }
    }
}
