using Dapper;
using LB_EloForteAPI.Models;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace LB_EloForteAPI.Repository
{
    public class TabelaPrecoDAO
    {
        public async Task<IEnumerable<TabelaPreco>> GetAsync()
        {
            StringBuilder sql = new StringBuilder();
            sql.AppendLine("select a.CD_TabelaPreco, a.DS_TabelaPreco")
                .AppendLine("from TB_DIV_TabelaPreco a")
                .AppendLine("where isnull(a.st_registro, 'A') <> 'C'");
            using (TConexao conexao = new TConexao())
            {
                if (await conexao.OpenConnectionAsync())
                {
                    var ret = await conexao._conexao.QueryAsync<TabelaPreco>(sql.ToString());
                    return ret;
                }
                else return null;
            }
        }
    }
}