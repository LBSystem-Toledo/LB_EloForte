using Prism.Mvvm;

namespace LB_EloForte.Models
{
    public class Cidade: BindableBase
    {
        public string Cd_cidade { get; set; } = string.Empty;
        private string _ds_cidade = string.Empty;
        public string Ds_cidade { get { return _ds_cidade; } set { SetProperty(ref _ds_cidade, value); } }
        private string _uf = string.Empty;
        public string UF { get { return _uf; } set { SetProperty(ref _uf, value); } }
    }
}
