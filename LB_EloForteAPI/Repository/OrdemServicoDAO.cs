using LB_EloForteAPI.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using System.Data;

namespace LB_EloForteAPI.Repository
{
    public class OrdemServicoDAO
    {
        public async Task<IEnumerable<OrdemServico>> GetAsync(string Cd_piloto = "",
                                                              string Cd_contratante = "",
                                                              string Cd_cidadeFazenda = "",
                                                              string Nm_fazenda = "",
                                                              string Dt_ini = "",
                                                              string Dt_fin = "")
        {
            StringBuilder sql = new StringBuilder();
            sql.AppendLine("select a.CD_Empresa, a.ID_OS, a.CD_Contratante,")
                .AppendLine("b.NM_Clifor as NM_Contratante, a.CD_EndContratante, a.obs,")
                .AppendLine("c.DS_Endereco as Ds_endcontratante, c.DS_Cidade as Ds_cidadecontratante,")
                .AppendLine("c.UF as Uf_contratante, c.Fone as Fone_contratante, b.Celular as Celular_contratante,")
                .AppendLine("a.NM_Fazenda, a.CD_CidadeFazenda, d.DS_Cidade as Ds_cidadefazenda, e.UF as uf_fazenda, ")
                .AppendLine("a.cd_tabelapreco, h.ds_tabelapreco, a.NR_OS, a.DT_Abertura_OS, isnull(a.DT_Termino_OS, getdate()) as DT_Termino_OS, a.HR_Ida_Ini,")
                .AppendLine("a.HR_Ida_Fin, a.HR_Operacao_Ini, a.HR_Operacao_Fin, a.HR_Volta_Ini, a.HR_Volta_Fin,")
                .AppendLine("a.id_veiculo, g.ds_veiculo, a.TotalMistura, a.Nr_voos, a.Hectares, a.Vl_unitario,")
                .AppendLine("a.cd_servico, f.ds_produto as ds_servico, a.Cultura, a.Faixa, a.Vz_mistura,")
                .AppendLine("a.RendimentoArea, a.Vl_Comissao, a.CD_Auxiliar, i.nm_clifor as NM_Auxiliar")
                .AppendLine("from TB_OSE_Pulverizacao a")
                .AppendLine("left join VTB_FIN_Clifor b")
                .AppendLine("on a.CD_Contratante = b.CD_Clifor")
                .AppendLine("left join VTB_FIN_ENDERECO c")
                .AppendLine("on a.CD_Contratante = c.CD_Clifor")
                .AppendLine("and a.CD_EndContratante = c.CD_Endereco")
                .AppendLine("inner join TB_FIN_Cidade d")
                .AppendLine("on a.CD_CidadeFazenda = d.CD_Cidade")
                .AppendLine("inner join TB_FIN_UF e")
                .AppendLine("on d.CD_UF = e.CD_UF")
                .AppendLine("inner join TB_EST_Produto f")
                .AppendLine("on a.cd_servico = f.cd_produto")
                .AppendLine("left join tb_frt_veiculo g")
                .AppendLine("on a.id_veiculo = g.id_veiculo")
                .AppendLine("left join tb_div_tabelapreco h")
                .AppendLine("on a.cd_tabelapreco = h.cd_tabelapreco")
                .AppendLine("left join tb_fin_clifor i")
                .AppendLine("on a.cd_auxiliar = i.cd_clifor")
                .AppendLine("where isnull(a.cancelado, 0) = 0")
                .AppendLine("and not exists(select 1 from tb_fat_notafiscal x ")
                .AppendLine("               where x.cd_empresa = a.cd_empresa ")
                .AppendLine("               and x.nr_lanctofiscal = a.nr_lanctofiscal ")
                .AppendLine("               and isnull(x.st_registro, 'A') <> 'C')")
                .AppendLine("and a.cd_piloto = '" + Cd_piloto.Trim() + "'");
            if (!string.IsNullOrWhiteSpace(Cd_contratante))
                sql.AppendLine("and a.cd_contratante = '" + Cd_contratante.Trim() + "'");
            if(!string.IsNullOrWhiteSpace(Cd_cidadeFazenda))
                sql.AppendLine("and a.cd_cidadefazenda = '" + Cd_cidadeFazenda.Trim() + "'");
            if (!string.IsNullOrWhiteSpace(Nm_fazenda))
                sql.AppendLine("and a.nm_fazenda like '%" + Nm_fazenda.Trim() + "%'");
            DateTime data;
            if(DateTime.TryParse(Dt_ini, out data))
                sql.AppendLine("and convert(datetime, floor(convert(decimal(30,10), a.dt_abertura_os))) >= '" + data.ToString("yyyyMMdd") + "'");
            if (DateTime.TryParse(Dt_fin, out data))
                sql.AppendLine("and convert(datetime, floor(convert(decimal(30,10), a.dt_abertura_os))) <= '" + data.ToString("yyyyMMdd") + "'");
            using (TConexao conexao = new TConexao())
            {
                if (await conexao.OpenConnectionAsync())
                {
                    var ret = await conexao._conexao.QueryAsync<OrdemServico>(sql.ToString());
                    return ret;
                }
                else return null;
            }
        }
        public async Task<bool> GravarAsync(OrdemServico os)
        {
            try
            {
                using (TConexao conexao = new TConexao())
                {
                    if (await conexao.OpenConnectionAsync())
                    {
                        //Buscar codigo empresa
                        os.Cd_empresa = await conexao._conexao.ExecuteScalarAsync<string>("select a.CD_Empresa from VTB_DIV_EMPRESA a where ISNULL(a.ST_Registro, 'A') <> 'C' and dbo.FVALIDA_NUMEROS(a.NR_CGC) = '12858928000190'");
                        DynamicParameters param = new DynamicParameters();
                        param.Add("@P_CD_EMPRESA", os.Cd_empresa, dbType: DbType.String, direction: ParameterDirection.Input);
                        param.Add("@P_ID_OS", os.Id_os, dbType: DbType.Int32, direction: ParameterDirection.InputOutput);
                        param.Add("@P_CD_PILOTO", os.Cd_piloto, dbType: DbType.String, direction: ParameterDirection.Input);
                        param.Add("@P_RENDIMENTOAREA", os.RendimentoArea, dbType: DbType.Int32, direction: ParameterDirection.Input);
                        param.Add("@P_VL_COMISSAO", os.Vl_comissao, dbType: DbType.Decimal, direction: ParameterDirection.Input);
                        if (string.IsNullOrWhiteSpace(os.Cd_auxiliar))
                            param.Add("@P_CD_AUXILIAR", DBNull.Value, dbType: DbType.String, direction: ParameterDirection.Input);
                        else param.Add("@P_CD_AUXILIAR", os.Cd_auxiliar, dbType: DbType.String, direction: ParameterDirection.Input);
                        if(string.IsNullOrWhiteSpace(os.Cd_contratante))
                            param.Add("@P_CD_CONTRATANTE", DBNull.Value, dbType: DbType.String, direction: ParameterDirection.Input);
                        else param.Add("@P_CD_CONTRATANTE", os.Cd_contratante, dbType: DbType.String, direction: ParameterDirection.Input);
                        if(string.IsNullOrWhiteSpace(os.Cd_endcontratante))
                            param.Add("@P_CD_ENDCONTRATANTE", DBNull.Value, dbType: DbType.String, direction: ParameterDirection.Input);
                        else param.Add("@P_CD_ENDCONTRATANTE", os.Cd_endcontratante, dbType: DbType.String, direction: ParameterDirection.Input);
                        if(string.IsNullOrWhiteSpace(os.Cd_cidadefazenda))
                            param.Add("@P_CD_CIDADEFAZENDA", DBNull.Value, dbType: DbType.String, direction: ParameterDirection.Input);
                        else param.Add("@P_CD_CIDADEFAZENDA", os.Cd_cidadefazenda, dbType: DbType.String, direction: ParameterDirection.Input);
                        param.Add("@P_NR_LANCTOFISCAL", DBNull.Value, dbType: DbType.Decimal, direction: ParameterDirection.Input);
                        if(string.IsNullOrWhiteSpace(os.Cd_servico))
                            param.Add("@P_CD_SERVICO", DBNull.Value, dbType: DbType.String, direction: ParameterDirection.Input);
                        else param.Add("@P_CD_SERVICO", os.Cd_servico, dbType: DbType.String, direction: ParameterDirection.Input);
                        if(string.IsNullOrWhiteSpace(os.Cd_tabelapreco))
                            param.Add("@P_CD_TABELAPRECO", DBNull.Value, dbType: DbType.String, direction: ParameterDirection.Input);
                        else param.Add("@P_CD_TABELAPRECO", os.Cd_tabelapreco, dbType: DbType.String, direction: ParameterDirection.Input);
                        param.Add("@P_ID_VEICULO", os.Id_veiculo, dbType: DbType.Decimal, direction: ParameterDirection.Input);
                        param.Add("@P_NR_OS", os.Nr_os, dbType: DbType.Int32, direction: ParameterDirection.Input);
                        param.Add("@P_NM_FAZENDA", os.Nm_fazenda.ToUpper(), dbType: DbType.String, direction: ParameterDirection.Input);
                        param.Add("@P_DT_ABERTURA_OS", os.Dt_abertura_OS, dbType: DbType.DateTime, direction: ParameterDirection.Input);
                        param.Add("@P_DT_TERMINO_OS", os.Dt_termino_OS, dbType: DbType.DateTime, direction: ParameterDirection.Input);
                        param.Add("@P_HR_IDA_INI", os.Hr_ida_ini, dbType: DbType.Decimal, direction: ParameterDirection.Input);
                        param.Add("@P_HR_IDA_FIN", os.Hr_ida_fin, dbType: DbType.Decimal, direction: ParameterDirection.Input);
                        param.Add("@P_HR_OPERACAO_INI", os.Hr_operacao_ini, dbType: DbType.Decimal, direction: ParameterDirection.Input);
                        param.Add("@P_HR_OPERACAO_FIN", os.Hr_operacao_fin, dbType: DbType.Decimal, direction: ParameterDirection.Input);
                        param.Add("@P_HR_VOLTA_INI", os.Hr_volta_ini, dbType: DbType.Decimal, direction: ParameterDirection.Input);
                        param.Add("@P_HR_VOLTA_FIN", os.Hr_volta_fin, dbType: DbType.Decimal, direction: ParameterDirection.Input);
                        param.Add("@P_TOTALMISTURA", os.Totalmistura, dbType: DbType.Int32, direction: ParameterDirection.Input);
                        param.Add("@P_NR_VOOS", os.Nr_voos, dbType: DbType.Int32, direction: ParameterDirection.Input);
                        param.Add("@P_HECTARES", os.Hectares, dbType: DbType.Int32, direction: ParameterDirection.Input);
                        param.Add("@P_VL_UNITARIO", os.Vl_unitario, dbType: DbType.Decimal, direction: ParameterDirection.Input);
                        param.Add("@P_CULTURA", os.Cultura.ToUpper(), dbType: DbType.String, direction: ParameterDirection.Input);
                        param.Add("@P_FAIXA", os.Faixa, dbType: DbType.Int32, direction: ParameterDirection.Input);
                        param.Add("@P_VZ_MISTURA", os.Vz_mistura, dbType: DbType.Int32, direction: ParameterDirection.Input);
                        param.Add("@P_OBS", os.Obs.ToUpper(), dbType: DbType.String, direction: ParameterDirection.Input);
                        int ret = await conexao._conexao.ExecuteAsync("IA_OSE_PULVERIZACAO", param, commandType: CommandType.StoredProcedure);
                        return ret > 0;
                    }
                    else return false;
                }
            }
            catch { return false; }
        }
        public async Task<bool> CancelarAsync(OrdemServico os)
        {
            try
            {
                DynamicParameters param = new DynamicParameters();
                param.Add("@P_CD_EMPRESA", os.Cd_empresa, dbType: DbType.String, direction: ParameterDirection.Input);
                param.Add("@P_ID_OS", os.Id_os, dbType: DbType.Int32, direction: ParameterDirection.Input);
                param.Add("@P_MOTIVOCANC", os.MotivoCanc, dbType: DbType.String, direction: ParameterDirection.Input);
                using (TConexao conexao = new TConexao())
                {
                    if (await conexao.OpenConnectionAsync())
                    {
                        int ret = await conexao._conexao.ExecuteAsync("EXCLUI_OSE_PULVERIZACAO", param, commandType: CommandType.StoredProcedure);
                        return ret > 0;
                    }
                    else return false;
                }
            }
            catch { return false; }
        }
    }
}