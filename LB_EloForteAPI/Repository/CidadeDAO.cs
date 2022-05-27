using Dapper;
using LB_EloForteAPI.Models;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace LB_EloForteAPI.Repository
{
    public class CidadeDAO
    {
        public async Task<IEnumerable<Cidade>> GetAsync(string Cd_cidade = "",
                                                        string Ds_cidade = "",
                                                        string Uf = "")
        {
            StringBuilder sql = new StringBuilder();
            sql.AppendLine("select a.CD_Cidade, a.DS_Cidade, b.UF")
                .AppendLine("from TB_FIN_Cidade a")
                .AppendLine("inner join TB_FIN_UF b")
                .AppendLine("on a.CD_UF = b.CD_UF");
            if (!string.IsNullOrWhiteSpace(Cd_cidade))
                sql.AppendLine("and a.cd_cidade = '" + Cd_cidade.Trim() + "'");
            if (!string.IsNullOrWhiteSpace(Ds_cidade))
                sql.AppendLine("and a.ds_cidade like '%" + Ds_cidade.Trim() + "%'");
            if (!string.IsNullOrWhiteSpace(Uf))
                sql.AppendLine("and b.uf like '%" + Uf.Trim() + "%'");
            using (TConexao conexao = new TConexao())
            {
                if (await conexao.OpenConnectionAsync())
                {
                    var ret = await conexao._conexao.QueryAsync<Cidade>(sql.ToString());
                    return ret;
                }
                else return null;
            }
        }
    }
}