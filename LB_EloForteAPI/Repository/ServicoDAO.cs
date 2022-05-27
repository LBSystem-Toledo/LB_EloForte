using Dapper;
using LB_EloForteAPI.Models;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace LB_EloForteAPI.Repository
{
    public class ServicoDAO
    {
        public async Task<IEnumerable<Servico>> GetAsync()
        {
            StringBuilder sql = new StringBuilder();
            sql.AppendLine("select a.cd_produto as CD_Servico, a.ds_produto as DS_Servico")
                .AppendLine("from TB_EST_Produto a")
                .AppendLine("inner join TB_EST_TpProduto b")
                .AppendLine("on a.TP_Produto = b.TP_Produto")
                .AppendLine("and ISNULL(b.ST_Servico, 'N') = 'S'")
                .AppendLine("where isnull(a.st_registro, 'A') <> 'C'");
            using (TConexao conexao = new TConexao())
            {
                if (await conexao.OpenConnectionAsync())
                {
                    var ret = await conexao._conexao.QueryAsync<Servico>(sql.ToString());
                    return ret;
                }
                else return null;
            }
        }
        public async Task<decimal> GetPrecoAsync(string Cd_servico, string Cd_tabelapreco)
        {
            using (TConexao conexao = new TConexao())
            {
                if (await conexao.OpenConnectionAsync())
                {
                    //Buscar codigo empresa
                    string Cd_empresa = await conexao._conexao.ExecuteScalarAsync<string>("select a.CD_Empresa from VTB_DIV_EMPRESA a where ISNULL(a.ST_Registro, 'A') <> 'C' and dbo.FVALIDA_NUMEROS(a.NR_CGC) = '12858928000190'");
                    DynamicParameters p = new DynamicParameters();
                    p.Add("@empresa", Cd_empresa, dbType: System.Data.DbType.String, direction: System.Data.ParameterDirection.Input);
                    p.Add("@servico", Cd_servico, dbType: System.Data.DbType.String, direction: System.Data.ParameterDirection.Input);
                    p.Add("@tabela", Cd_tabelapreco, dbType: System.Data.DbType.String, direction: System.Data.ParameterDirection.Input);
                    var ret = await conexao._conexao.ExecuteScalarAsync<decimal>("select dbo.F_PRECO_VENDA(@empresa, @servico, @tabela)", p);
                    return ret;
                }
                else return decimal.Zero;
            }
        }
    }
}