using Prism.Mvvm;
using System;

namespace LB_EloForte.Models
{
    public class OrdemServico: BindableBase
    {
        public string Cd_empresa { get; set; } = string.Empty;
        public int Id_os { get; set; }
        private int _nr_os;
        public int Nr_os { get { return _nr_os; } set { SetProperty(ref _nr_os, value); } }
        public string Cd_piloto { get; set; } = string.Empty;
        public string Cd_auxiliar { get; set; } = string.Empty;
        private string _nm_auxiliar = string.Empty;
        public string Nm_auxiliar { get { return _nm_auxiliar; } set { SetProperty(ref _nm_auxiliar, value); } }
        public string Cd_contratante { get; set; } = string.Empty;
        private string _nm_contratante = string.Empty;
        public string Nm_contratante { get { return _nm_contratante; } set { SetProperty(ref _nm_contratante, value); } }
        public string Cd_endcontratante { get; set; } = string.Empty;
        private string _ds_condcontrante = string.Empty;
        public string Ds_endcontratante { get { return _ds_condcontrante; } set { SetProperty(ref _ds_condcontrante, value); } }
        private string _ds_cidadecontrantante = string.Empty;
        public string Ds_cidadecontratante { get { return _ds_cidadecontrantante; } set { SetProperty(ref _ds_cidadecontrantante, value); } }
        private string _uf_contratante = string.Empty;
        public string Uf_contratante { get { return _uf_contratante; } set { SetProperty(ref _uf_contratante, value); } }
        private string _fone_contratante = string.Empty;
        public string Fone_contratante { get { return _fone_contratante; } set { SetProperty(ref _fone_contratante, value); } }
        private string _celular_contratante = string.Empty;
        public string Celular_contratante { get { return _celular_contratante; } set { SetProperty(ref _celular_contratante, value); } }
        private string _nm_fazenda = string.Empty;
        public string Nm_fazenda { get { return _nm_fazenda; } set { SetProperty(ref _nm_fazenda, value); } }
        public string Cd_cidadefazenda { get; set; } = string.Empty;
        private string _ds_cidadefazenda = string.Empty;
        public string Ds_cidadefazenda { get { return _ds_cidadefazenda; } set { SetProperty(ref _ds_cidadefazenda, value); } }
        private string _uf_fazenda = string.Empty;
        public string Uf_fazenda { get { return _uf_fazenda; } set { SetProperty(ref _uf_fazenda, value); } }
        public string Cd_servico { get; set; } = string.Empty;
        private string _ds_servico = string.Empty;
        public string Ds_servico { get { return _ds_servico; } set { SetProperty(ref _ds_servico, value); } }
        public string Cd_tabelapreco { get; set; } = string.Empty;
        private string _ds_tabelapreco = string.Empty;
        public string Ds_tabelapreco { get { return _ds_tabelapreco; } set { SetProperty(ref _ds_tabelapreco, value); } }
        public decimal? Id_veiculo { get; set; } = null;
        private string _ds_veiculo = string.Empty;
        public string Ds_veiculo { get { return _ds_veiculo; } set { SetProperty(ref _ds_veiculo, value); } }
        private DateTime _dt_abertura_os = DateTime.Now;
        public DateTime Dt_abertura_OS { get { return _dt_abertura_os; } set { SetProperty(ref _dt_abertura_os, value); } }
        private DateTime _dt_termino_os = DateTime.Now;
        public DateTime Dt_termino_OS { get { return _dt_termino_os; } set { SetProperty(ref _dt_termino_os, value); } }
        private decimal _hr_ida_ini = decimal.Zero;
        public decimal Hr_ida_ini 
        { 
            get { return _hr_ida_ini; } 
            set 
            { 
                SetProperty(ref _hr_ida_ini, value);
                if (Hr_ida_fin > decimal.Zero && value > decimal.Zero)
                    Hr_ida_result = Hr_ida_fin - value;
            }
        }
        private decimal _hr_ida_fin = decimal.Zero;
        public decimal Hr_ida_fin 
        { 
            get { return _hr_ida_fin; } 
            set 
            { 
                SetProperty(ref _hr_ida_fin, value);
                if (Hr_ida_ini > decimal.Zero && value > decimal.Zero)
                    Hr_ida_result = value - Hr_ida_ini;
            } 
        }
        private decimal _hr_ida_result = decimal.Zero;
        public decimal Hr_ida_result { get { return _hr_ida_result; }set { SetProperty(ref _hr_ida_result, value); } }
        private decimal _hr_operacao_ini = decimal.Zero;
        public decimal Hr_operacao_ini 
        { 
            get { return _hr_operacao_ini; } 
            set 
            { 
                SetProperty(ref _hr_operacao_ini, value);
                if (Hr_operacao_fin > decimal.Zero && value > decimal.Zero)
                    Hr_operacao_result = Hr_operacao_fin - value;
            } 
        }
        private decimal _hr_operacao_fin = decimal.Zero;
        public decimal Hr_operacao_fin 
        { 
            get { return _hr_operacao_fin; } 
            set 
            { 
                SetProperty(ref _hr_operacao_fin, value);
                if (Hr_operacao_ini > decimal.Zero && value > decimal.Zero)
                    Hr_operacao_result = value - Hr_operacao_ini;
            } 
        }
        private decimal _hr_operacao_result = decimal.Zero;
        public decimal Hr_operacao_result { get { return _hr_operacao_result; }set { SetProperty(ref _hr_operacao_result, value); } }
        private decimal _hr_volta_ini = decimal.Zero;
        public decimal Hr_volta_ini 
        { 
            get { return _hr_volta_ini; } 
            set 
            { 
                SetProperty(ref _hr_volta_ini, value);
                if (value > decimal.Zero && Hr_volta_fin > decimal.Zero)
                    Hr_volta_result = Hr_volta_fin - value;
            } 
        }
        private decimal _hr_volta_fin = decimal.Zero;
        public decimal Hr_volta_fin 
        { 
            get { return _hr_volta_fin; } 
            set 
            { 
                SetProperty(ref _hr_volta_fin, value);
                if (value > decimal.Zero && Hr_volta_ini > decimal.Zero)
                    Hr_volta_result = value - Hr_volta_ini;
            } 
        }
        private decimal _hr_volta_result = decimal.Zero;
        public decimal Hr_volta_result { get { return _hr_volta_result; } set { SetProperty(ref _hr_volta_result, value); } }
        private int _totalmistura;
        public int Totalmistura { get { return _totalmistura; } set { SetProperty(ref _totalmistura, value); } }
        private int _nr_voos;
        public int Nr_voos { get { return _nr_voos; } set { SetProperty(ref _nr_voos, value); } }
        private int _hectares;
        public int Hectares 
        { 
            get { return _hectares; } 
            set 
            { 
                SetProperty(ref _hectares, value);
                Vl_total = value * Vl_unitario;
            } 
        }
        private decimal _vl_unitario = decimal.Zero;
        public decimal Vl_unitario 
        { 
            get { return _vl_unitario; } 
            set 
            { 
                SetProperty(ref _vl_unitario, value);
                Vl_total = Hectares * value;
            } 
        }
        private decimal _vl_total = decimal.Zero;
        public decimal Vl_total { get { return _vl_total; } set { SetProperty(ref _vl_total, value); } }
        private string _cultura = string.Empty;
        public string Cultura { get { return _cultura; } set { SetProperty(ref _cultura, value); } }
        private int _faixa;
        public int Faixa { get { return _faixa; } set { SetProperty(ref _faixa, value); } }
        private int _vz_mistura;
        public int Vz_mistura { get { return _vz_mistura; } set { SetProperty(ref _vz_mistura, value); } }
        private int _rendimentoarea;
        public int RendimentoArea { get { return _rendimentoarea; } set { SetProperty(ref _rendimentoarea, value); } }
        private decimal _vl_comissao = decimal.Zero;
        public decimal Vl_comissao { get { return _vl_comissao; } set { SetProperty(ref _vl_comissao, value); } }
        public string MotivoCanc { get; set; } = string.Empty;
        private string _obs = string.Empty;
        public string Obs { get { return _obs; } set { SetProperty(ref _obs, value); } }
    }
}
