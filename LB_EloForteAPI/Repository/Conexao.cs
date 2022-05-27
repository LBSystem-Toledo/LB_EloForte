using System;
using System.Configuration;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace LB_EloForteAPI.Repository
{
    public class TConexao : IDisposable
    {
        public SqlConnection _conexao { get; private set; }
        public TConexao()
        {
            _conexao = new SqlConnection(ConfigurationManager.ConnectionStrings["stringConexao"].ConnectionString);
        }

        public async Task<bool> OpenConnectionAsync()
        {
            try
            {
                await _conexao.OpenAsync();
                return true;
            }
            catch { return false; }
        }

        public void Dispose()
        {
            if (_conexao != null)
            {
                if (_conexao.State == System.Data.ConnectionState.Open)
                    _conexao.Close();
                _conexao = null;
            }
        }
    }
}