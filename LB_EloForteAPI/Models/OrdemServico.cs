using System;

namespace LB_EloForteAPI.Models
{
    public class OrdemServico
    {
        public string Cd_empresa { get; set; } = string.Empty;
        public int Id_os { get; set; }
        public int Nr_os { get; set; }
        public string Cd_piloto { get; set; } = string.Empty;
        public string Cd_auxiliar { get; set; } = string.Empty;
        public string Nm_auxiliar { get; set; } = string.Empty;
        public string Cd_contratante { get; set; } = string.Empty;
        public string Nm_contratante { get; set; } = string.Empty;
        public string Cd_endcontratante { get; set; } = string.Empty;
        public string Ds_endcontratante { get; set; } = string.Empty;
        public string Ds_cidadecontratante { get; set; } = string.Empty;
        public string Uf_contratante { get; set; } = string.Empty;
        public string Fone_contratante { get; set; } = string.Empty;
        public string Celular_contratante { get; set; } = string.Empty;
        public string Nm_fazenda { get; set; } = string.Empty;
        public string Cd_cidadefazenda { get; set; } = string.Empty;
        public string Ds_cidadefazenda { get; set; } = string.Empty;
        public string Uf_fazenda { get; set; } = string.Empty;
        public string Cd_servico { get; set; } = string.Empty;
        public string Ds_servico { get; set; } = string.Empty;
        public string Cd_tabelapreco { get; set; } = string.Empty;
        public string Ds_tabelapreco { get; set; } = string.Empty;
        public decimal? Id_veiculo { get; set; } = null;
        public string Ds_veiculo { get; set; } = string.Empty;
        public DateTime? Dt_abertura_OS { get; set; } = null;
        public DateTime? Dt_termino_OS { get; set; } = null;
        public decimal Hr_ida_ini { get; set; } = decimal.Zero;
        public decimal Hr_ida_fin { get; set; } = decimal.Zero;
        public decimal Hr_operacao_ini { get; set; } = decimal.Zero;
        public decimal Hr_operacao_fin { get; set; } = decimal.Zero;
        public decimal Hr_volta_ini { get; set; } = decimal.Zero;
        public decimal Hr_volta_fin { get; set; } = decimal.Zero;
        public int Totalmistura { get; set; }
        public int Nr_voos { get; set; }
        public int Hectares { get; set; }
        public decimal Vl_unitario { get; set; } = decimal.Zero;
        public string Cultura { get; set; } = string.Empty;
        public int Faixa { get; set; }
        public int Vz_mistura { get; set; }
        public int RendimentoArea { get; set; }
        public decimal Vl_comissao { get; set; } = decimal.Zero;
        public string MotivoCanc { get; set; } = string.Empty;
        public string Obs { get; set; } = string.Empty;
    }
}