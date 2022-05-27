using Dapper;
using LB_EloForteAPI.Models;
using LB_EloForteAPI.Utils;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Threading.Tasks;

namespace LB_EloForteAPI.Repository
{
    public class ClienteDAO
    {
        public async Task<IEnumerable<Cliente>> GetAsync(string cd_clifor = "",
                                                         string Nm_clifor = "",
                                                         string Ds_cidade = "")
        {
            StringBuilder sql = new StringBuilder();
            sql.AppendLine("select a.CD_Clifor, isnull(a.nm_fantasia, a.NM_Clifor) as NM_Clifor, a.TP_Pessoa,")
                .AppendLine("case when a.tp_pessoa = 'J' then a.nr_cgc else a.nr_cpf end as NR_Docto, ")
                .AppendLine("a.NR_CGC, a.NR_CPF, a.NR_RG, a.CD_Endereco,")
                .AppendLine("a.DS_Endereco, a.Numero, a.Bairro, a.CD_Cidade,")
                .AppendLine("a.DS_Cidade, a.CD_UF, a.UF, a.Fone, a.Celular, a.Insc_Estadual")
                .AppendLine("from VTB_FIN_CLIFOR a")
                .AppendLine("where ISNULL(a.ST_Registro, 'A') <> 'C'")
                .AppendLine("and not exists(select 1 from TB_DIV_Empresa x")
                .AppendLine("				where x.CD_Clifor = a.CD_Clifor)");
            if (!string.IsNullOrWhiteSpace(cd_clifor))
                sql.AppendLine("and a.cd_clifor = '" + cd_clifor.Trim() + "'");
            if (!string.IsNullOrWhiteSpace(Nm_clifor))
                sql.AppendLine("and a.nm_clifor like '%" + Nm_clifor.Trim() + "%'");
            if (!string.IsNullOrWhiteSpace(Ds_cidade))
                sql.AppendLine("and a.ds_cidade like '%" + Ds_cidade.Trim() + "'%");
            using (TConexao conexao = new TConexao())
            {
                if (await conexao.OpenConnectionAsync())
                {
                    var ret = await conexao._conexao.QueryAsync<Cliente>(sql.ToString());
                    return ret;
                }
                else return null;
            }
        }
        public async Task<IEnumerable<Cliente>> GetAuxiliaresAsync()
        {
            StringBuilder sql = new StringBuilder();
            sql.AppendLine("select a.CD_Clifor, isnull(a.nm_fantasia, a.NM_Clifor) as NM_Clifor, a.TP_Pessoa,")
                .AppendLine("case when a.tp_pessoa = 'J' then a.nr_cgc else a.nr_cpf end as NR_Docto, ")
                .AppendLine("a.NR_CGC, a.NR_CPF, a.NR_RG, a.CD_Endereco,")
                .AppendLine("a.DS_Endereco, a.Numero, a.Bairro, a.CD_Cidade,")
                .AppendLine("a.DS_Cidade, a.CD_UF, a.UF, a.Fone, a.Celular, a.Insc_Estadual")
                .AppendLine("from VTB_FIN_CLIFOR a")
                .AppendLine("inner join VTB_DIV_EMPRESA b")
                .AppendLine("on a.cd_empresa = b.CD_Empresa")
                .AppendLine("where ISNULL(a.ST_Registro, 'A') <> 'C'")
                .AppendLine("and not exists(select 1 from TB_DIV_Empresa x")
                .AppendLine("				where x.CD_Clifor = a.CD_Clifor)")
                .AppendLine("and ISNULL(a.st_funcativo, 'N') = 'S'")
                .AppendLine("and ISNULL(a.ST_Funcionarios, 'N') = 'S'")
                .AppendLine("and dbo.FVALIDA_NUMEROS(b.NR_CGC) = '12858928000190'");

            using (TConexao conexao = new TConexao())
            {
                if (await conexao.OpenConnectionAsync())
                {
                    var ret = await conexao._conexao.QueryAsync<Cliente>(sql.ToString());
                    return ret;
                }
                else return null;
            }
        }
        public async Task<bool> GravarAsync(Cliente cliente)
        {
            SqlTransaction t = null;
            try
            {
                using (TConexao conexao = new TConexao())
                {
                    if (await conexao.OpenConnectionAsync())
                    {
                        t = conexao._conexao.BeginTransaction(IsolationLevel.ReadCommitted);
                        //Verificar se cliente ja esta cadastrado para CNPJ/CPF
                        if (string.IsNullOrWhiteSpace(cliente.Cd_clifor))
                        {
                            StringBuilder sql = new StringBuilder();
                            sql.AppendLine("select CD_Clifor ")
                                .AppendLine("from VTB_FIN_CLIFOR")
                                .AppendLine("where ISNULL(ST_Registro, 'A') <> 'C'");
                            if (cliente.Tp_pessoa.Trim().ToUpper().Equals("F"))
                                sql.AppendLine("and dbo.FVALIDA_NUMEROS(NR_CPF) = '" + cliente.Cpf.SoNumero() + "'");
                            else sql.AppendLine("and dbo.FVALIDA_NUMEROS(NR_CGC) = '" + cliente.Cnpj.SoNumero() + "'");
                            cliente.Cd_clifor = await conexao._conexao.ExecuteScalarAsync<string>(sql.ToString(), transaction: t);
                        }
                        DynamicParameters p;
                        if (string.IsNullOrWhiteSpace(cliente.Cd_clifor))
                        {
                            StringBuilder sql = new StringBuilder();
                            sql.AppendLine("DECLARE @V_CD_CLIFOR VARCHAR(10)")
                                .AppendLine("EXEC P_FORMATAZERO  'TB_FIN_Clifor','CD_Clifor', '', @V_CD_CLIFOR, @V_CD_CLIFOR OUTPUT")
                                .AppendLine("INSERT INTO TB_FIN_Clifor(CD_Clifor, NM_Clifor, Id_Regiao, TP_Pessoa, Cd_CondFiscal_Clifor, ID_CategoriaClifor, Email, Celular, ST_Registro, dt_cad, dt_alt)")
                                .AppendLine("VALUES(@V_CD_CLIFOR, @NM_CLIFOR, @ID_REGIAO, @TP_PESSOA, @CD_CONDFISCAL_CLIFOR, @ID_CATEGORIACLIFOR, @EMAIL, @CELULAR, 'A', getdate(), getdate())");
                            p = new DynamicParameters();
                            p.Add("@ID_REGIAO", null);
                            p.Add("@TP_PESSOA", cliente.Tp_pessoa.ToUpper());
                            p.Add("@CD_CONDFISCAL_CLIFOR", "01");
                            p.Add("@NM_CLIFOR", cliente.Nm_clifor.ToUpper());
                            p.Add("@ID_CATEGORIACLIFOR", 1);
                            p.Add("@EMAIL", cliente.Email.ToUpper());
                            p.Add("@CELULAR", cliente.Celular);
                            await conexao._conexao.ExecuteAsync(sql.ToString(), param: p, transaction: t, commandType: CommandType.Text);
                            sql = new StringBuilder();
                            sql.AppendLine("select max(cd_clifor) from tb_fin_clifor");
                            cliente.Cd_clifor = await conexao._conexao.ExecuteScalarAsync<string>(sql.ToString(), transaction: t);
                            if (cliente.Tp_pessoa.Trim().ToUpper().Equals("F"))
                            {
                                sql.Clear();
                                sql.AppendLine("insert into TB_FIN_Clifor_PF(CD_Clifor, NR_CPF, NR_RG, DT_Cad, DT_Alt)")
                                    .AppendLine("values(@CD_CLIFOR, @NR_CPF, @NR_RG, GETDATE(), GETDATE())");
                                p = new DynamicParameters();
                                p.Add("@CD_CLIFOR", cliente.Cd_clifor);
                                p.Add("@NR_CPF", cliente.Cpf);
                                p.Add("@NR_RG", cliente.Rg);
                                await conexao._conexao.ExecuteAsync(sql.ToString(), param: p, transaction: t, commandType: CommandType.Text);
                            }
                            else
                            {
                                sql.Clear();
                                sql.AppendLine("insert into TB_FIN_Clifor_PJ(CD_Clifor, NR_CGC, NM_Fantasia, DT_Cad, DT_Alt)")
                                    .AppendLine("values(@CD_CLIFOR, @NR_CGC, @NM_FANTASIA, GETDATE(), GETDATE())");
                                p = new DynamicParameters();
                                p.Add("@CD_CLIFOR", cliente.Cd_clifor);
                                p.Add("@NR_CGC", cliente.Cnpj);
                                p.Add("@NM_FANTASIA", cliente.Nm_fantasia.ToUpper());
                                await conexao._conexao.ExecuteAsync(sql.ToString(), param: p, transaction: t, commandType: CommandType.Text);
                            }
                            p = new DynamicParameters();
                            p.Add("@P_CD_ENDERECO", dbType: DbType.String, size: 4, direction: ParameterDirection.Output);
                            p.Add("@P_CD_CLIFOR", cliente.Cd_clifor);
                            p.Add("@P_CD_CIDADE", cliente.Cd_cidade);
                            p.Add("@P_CD_PAIS", "1058");
                            p.Add("@P_DS_ENDERECO", cliente.Ds_endereco.ToUpper());
                            p.Add("@P_DS_COMPLEMENTO", cliente.Ds_complemento.ToUpper());
                            p.Add("@P_NUMERO", cliente.Numero.ToUpper());
                            p.Add("@P_BAIRRO", cliente.Bairro.ToUpper());
                            p.Add("@P_PROXIMO", null);
                            p.Add("@P_CEP", cliente.Cep);
                            p.Add("@P_CP", null);
                            p.Add("@P_FONE", cliente.Fone);
                            p.Add("@P_INSC_ESTADUAL", cliente.Insc_estadual);
                            p.Add("@P_ST_NAOCONTRIBUINTE", string.IsNullOrWhiteSpace(cliente.Insc_estadual) ? "S" : "N");
                            p.Add("@P_ST_ENDENTREGA", null);
                            p.Add("@P_ST_ENDCOBRANCA", null);
                            p.Add("@P_ST_REGISTRO", "A");
                            p.Add("@P_LATITUDE", null);
                            p.Add("@P_LONGITUDE", null);
                            p.Add("@P_CD_INTEGRACAO", null);
                            await conexao._conexao.ExecuteAsync("IA_FIN_ENDERECO", p, transaction: t, commandType: CommandType.StoredProcedure);
                        }
                        else
                        {
                            StringBuilder sql = new StringBuilder();
                            sql.AppendLine("update tb_fin_clifor set TP_PESSOA = @TP_PESSOA, ID_REGIAO = @ID_REGIAO, ")
                                .AppendLine("CD_CONDFISCAL_CLIFOR = @CD_CONDFISCAL_CLIFOR, NM_CLIFOR = @NM_CLIFOR, ")
                                .AppendLine("ID_CATEGORIACLIFOR = @ID_CATEGORIACLIFOR, EMAIL = @EMAIL, CELULAR = @CELULAR, DT_ALT = GETDATE() ")
                                .AppendLine("WHERE CD_CLIFOR = @CD_CLIFOR");
                            p = new DynamicParameters();
                            p.Add("@CD_CLIFOR", cliente.Cd_clifor);
                            p.Add("@ID_REGIAO", null);
                            p.Add("@TP_PESSOA", cliente.Tp_pessoa.ToUpper());
                            p.Add("@CD_CONDFISCAL_CLIFOR", "01");
                            p.Add("@NM_CLIFOR", cliente.Nm_clifor.ToUpper());
                            p.Add("@ID_CATEGORIACLIFOR", 1);
                            p.Add("@EMAIL", cliente.Email.ToUpper());
                            p.Add("@CELULAR", cliente.Celular);
                            int ret = await conexao._conexao.ExecuteAsync(sql.ToString(), param: p, transaction: t, commandType: CommandType.Text);
                            sql = new StringBuilder();
                            sql.AppendLine("select max(cd_clifor) from tb_fin_clifor");
                            cliente.Cd_clifor = await conexao._conexao.ExecuteScalarAsync<string>(sql.ToString(), transaction: t);
                            if (cliente.Tp_pessoa.Trim().ToUpper().Equals("F"))
                            {
                                sql.Clear();
                                sql.AppendLine("UPDATE TB_FIN_CLIFOR_PF SET NR_CPF = @NR_CPF, NR_RG = @NR_RG, DT_ALT = GETDATE()")
                                    .AppendLine("WHERE CD_CLIFOR = @CD_CLIFOR");
                                p = new DynamicParameters();
                                p.Add("@CD_CLIFOR", cliente.Cd_clifor);
                                p.Add("@NR_CPF", cliente.Cpf);
                                p.Add("@NR_RG", cliente.Rg);
                                await conexao._conexao.ExecuteAsync(sql.ToString(), param: p, transaction: t, commandType: CommandType.Text);
                            }
                            else
                            {
                                sql.Clear();
                                sql.AppendLine("UPDATE TB_FIN_CLIFOR_PJ SET NR_CGC = @NR_CGC, NM_FANTASIA = @NM_FANTASIA, DT_ALT = GETDATE()")
                                    .AppendLine("WHERE CD_CLIFOR = @CD_CLIFOR");
                                p = new DynamicParameters();
                                p.Add("@CD_CLIFOR", cliente.Cd_clifor);
                                p.Add("@NR_CGC", cliente.Cnpj);
                                p.Add("@NM_FANTASIA", cliente.Nm_fantasia.ToUpper());
                                await conexao._conexao.ExecuteAsync(sql.ToString(), param: p, transaction: t, commandType: CommandType.Text);
                            }
                            //Buscar endereco
                            string cd_endereco = await conexao._conexao.ExecuteScalarAsync<string>("select max(cd_endereco) from tb_fin_endereco where cd_clifor = '" + cliente.Cd_clifor.Trim() + "' and isnull(st_registro, 'A') <> 'C'", transaction: t);
                            p = new DynamicParameters();
                            p.Add("@P_CD_ENDERECO", cd_endereco);
                            p.Add("@P_CD_CLIFOR", cliente.Cd_clifor);
                            p.Add("@P_CD_CIDADE", cliente.Cd_cidade);
                            p.Add("@P_CD_PAIS", "1058");
                            p.Add("@P_DS_ENDERECO", cliente.Ds_endereco.ToUpper());
                            p.Add("@P_DS_COMPLEMENTO", cliente.Ds_complemento.ToUpper());
                            p.Add("@P_NUMERO", cliente.Numero.ToUpper());
                            p.Add("@P_BAIRRO", cliente.Bairro.ToUpper());
                            p.Add("@P_PROXIMO", null);
                            p.Add("@P_CEP", cliente.Cep);
                            p.Add("@P_CP", null);
                            p.Add("@P_FONE", cliente.Fone);
                            p.Add("@P_INSC_ESTADUAL", cliente.Insc_estadual);
                            p.Add("@P_ST_NAOCONTRIBUINTE", string.IsNullOrWhiteSpace(cliente.Insc_estadual) ? "S" : "N");
                            p.Add("@P_ST_ENDENTREGA", null);
                            p.Add("@P_ST_ENDCOBRANCA", null);
                            p.Add("@P_ST_REGISTRO", "A");
                            p.Add("@P_LATITUDE", null);
                            p.Add("@P_LONGITUDE", null);
                            p.Add("@P_CD_INTEGRACAO", null);
                            await conexao._conexao.ExecuteAsync("IA_FIN_ENDERECO", p, transaction: t, commandType: CommandType.StoredProcedure);
                        }

                        t.Commit();
                        return true;
                    }
                    else return false;
                }
            }
            catch 
            {
                if (t != null)
                    t.Dispose();
                return false; 
            }
        }
    }
}