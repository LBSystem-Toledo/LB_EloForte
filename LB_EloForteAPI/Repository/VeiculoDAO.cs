using Dapper;
using LB_EloForteAPI.Models;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace LB_EloForteAPI.Repository
{
    public class VeiculoDAO
    {
        public async Task<IEnumerable<Veiculo>> GetAsync()
        {
            StringBuilder sql = new StringBuilder();
            sql.AppendLine("select a.ID_Veiculo, a.DS_Veiculo, a.Modelo")
                .AppendLine("from TB_FRT_Veiculo a")
                .AppendLine("where isnull(a.st_registro, 'A') <> 'C'");
            using (TConexao conexao = new TConexao())
            {
                if (await conexao.OpenConnectionAsync())
                {
                    var ret = await conexao._conexao.QueryAsync<Veiculo>(sql.ToString());
                    return ret;
                }
                else return null;
            }
        }
    }
}