using Prism.Mvvm;

namespace LB_EloForte.Models
{
    public class TipoPessoa
    {
        public string Tp_pessoa { get; set; } = string.Empty;
        public string Tipo_pessoa { get; set; } = string.Empty;
    }

    public class Cliente: BindableBase
    {
        public string Cd_clifor { get; set; } = string.Empty;
        private string _nm_clifor = string.Empty;
        public string Nm_clifor { get { return _nm_clifor; } set { SetProperty(ref _nm_clifor, value); } }
        private string _nm_fantasia = string.Empty;
        public string Nm_fantasia { get { return _nm_fantasia; } set { SetProperty(ref _nm_fantasia, value); } }
        private string _nr_docto = string.Empty;
        public string Nr_docto { get { return _nr_docto; } set { SetProperty(ref _nr_docto, value); } }
        private string _tp_pessoa = string.Empty;
        public string Tp_pessoa { get { return _tp_pessoa; } set { SetProperty(ref _tp_pessoa, value); } }
        private string _cnpj = string.Empty;
        public string Cnpj { get { return _cnpj; } set { SetProperty(ref _cnpj, value); } }
        private string _cpf = string.Empty;
        public string Cpf { get { return _cpf; } set { SetProperty(ref _cpf, value); } }
        private string _rg = string.Empty;
        public string Rg { get { return _rg; } set { SetProperty(ref _rg, value); } }
        private string _cep = string.Empty;
        public string Cep { get { return _cep; } set { SetProperty(ref _cep, value); } }
        public string Cd_endereco { get; set; } = string.Empty;
        private string _ds_endereco = string.Empty;
        public string Ds_endereco { get { return _ds_endereco; } set { SetProperty(ref _ds_endereco, value); } }
        private string _numero = string.Empty;
        public string Numero { get { return _numero; } set { SetProperty(ref _numero, value); } }
        private string _bairro = string.Empty;
        public string Bairro { get { return _bairro; } set { SetProperty(ref _bairro, value); } }
        public string Cd_cidade { get; set; } = string.Empty;
        private string _ds_cidade = string.Empty;
        public string Ds_cidade { get { return _ds_cidade; } set { SetProperty(ref _ds_cidade, value); } }
        public string Cd_uf { get; set; } = string.Empty;
        private string _uf = string.Empty;
        public string Uf { get { return _uf; } set { SetProperty(ref _uf, value); } }
        private string _ds_complemento = string.Empty;
        public string Ds_complemento { get { return _ds_complemento; } set { SetProperty(ref _ds_complemento, value); } }
        private string _fone = string.Empty;
        public string Fone { get { return _fone; } set { SetProperty(ref _fone, value); } }
        private string _celular = string.Empty;
        public string Celular { get { return _celular; } set { SetProperty(ref _celular, value); } }
        private string _insc_estadual = string.Empty;
        public string Insc_estadual { get { return _insc_estadual; } set { SetProperty(ref _insc_estadual, value); } }
    }
}
