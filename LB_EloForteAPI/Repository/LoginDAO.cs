using System.Data;
using System.Text;
using System.Threading.Tasks;
using Dapper;

namespace LB_EloForteAPI.Repository
{
    public class LoginDAO
    {
        public async Task<string> ValidarLoginAsync(string Login, string Senha)
        {
            using (TConexao conexao = new TConexao())
            {
                if (await conexao.OpenConnectionAsync())
                {
                    StringBuilder sql = new StringBuilder();
                    sql.AppendLine("select cd_clifor")
                        .AppendLine("from VTB_FIN_CLIFOR a")
                        .AppendLine("where ISNULL(a.ST_Funcionarios, 'N') = 'S'")
                        .AppendLine("and ISNULL(a.st_funcativo, 'N') = 'S'")
                        .AppendLine("and a.LoginGarcomApp = @login")
                        .AppendLine("and a.SenhaGarcomApp = @senha");
                    DynamicParameters param = new DynamicParameters();
                    param.Add("@login", Login, dbType: DbType.String, direction: ParameterDirection.Input);
                    param.Add("@senha", Senha, dbType: DbType.String, direction: ParameterDirection.Input);
                    string ret = await conexao._conexao.ExecuteScalarAsync<string>(sql.ToString(), param, commandType: CommandType.Text);
                    return ret;
                }
                else return string.Empty;
            }
        }
    }
}