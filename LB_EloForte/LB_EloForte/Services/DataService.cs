using LB_EloForte.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;

namespace LB_EloForte.Services
{
    public static class DataService
    {
        //const string url_api = "http://177.222.146.83:61007";
        const string url_api = "http://192.168.1.108:45455";

        public static async Task<string> ValidarLoginAsync(string Login, string Senha)
        {
            try
            {
                HttpClient client = new HttpClient();
                HttpResponseMessage response = await client.GetAsync(url_api + "/api/Login/ValidarLoginAsync?Login=" + Login + "&Senha=" + Senha);
                if (response.IsSuccessStatusCode)
                    return JsonConvert.DeserializeObject<string>(await response.Content.ReadAsStringAsync());
                else return string.Empty;
            }
            catch { return string.Empty; }
        }
        public static async Task<IEnumerable<Veiculo>> GetVeiculosAsync()
        {
            try
            {
                HttpClient client = new HttpClient();
                HttpResponseMessage response = await client.GetAsync(url_api + "/api/Veiculo/GetAsync");
                if (response.IsSuccessStatusCode)
                    return JsonConvert.DeserializeObject<IEnumerable<Veiculo>>(await response.Content.ReadAsStringAsync());
                else return null;
            }
            catch { return null; }
        }
        public static async Task<IEnumerable<TabelaPreco>> GetTabelasAsync()
        {
            try
            {
                HttpClient client = new HttpClient();
                HttpResponseMessage response = await client.GetAsync(url_api + "/api/TabelaPreco/GetAsync");
                if (response.IsSuccessStatusCode)
                    return JsonConvert.DeserializeObject<IEnumerable<TabelaPreco>>(await response.Content.ReadAsStringAsync());
                else return null;
            }
            catch { return null; }
        }
        public static async Task<IEnumerable<Servico>> GetServicosAsync()
        {
            try
            {
                HttpClient client = new HttpClient();
                HttpResponseMessage response = await client.GetAsync(url_api + "/api/Servico/GetAsync");
                if (response.IsSuccessStatusCode)
                    return JsonConvert.DeserializeObject<IEnumerable<Servico>>(await response.Content.ReadAsStringAsync());
                else return null;
            }
            catch { return null; }
        }
        public static async Task<decimal> GetPrecoAsync(string Cd_servico, string Cd_tabelapreco)
        {
            try
            {
                HttpClient client = new HttpClient();
                HttpResponseMessage response = await client.GetAsync(url_api + "/api/Servico/GetPrecoAsync?Cd_servico=" + Cd_servico.Trim() + "&Cd_tabelapreco=" + Cd_tabelapreco.Trim());
                if (response.IsSuccessStatusCode)
                    return JsonConvert.DeserializeObject<decimal>(await response.Content.ReadAsStringAsync());
                else return decimal.Zero;
            }
            catch { return decimal.Zero; }
        }
        public static async Task<IEnumerable<Cidade>> GetCidadesAsync(string Cd_cidade = "",
                                                                      string Ds_cidade = "", 
                                                                      string Uf = "")
        {
            try
            {
                HttpClient client = new HttpClient();
                HttpResponseMessage response = await client.GetAsync(url_api + "/api/Cidade/GetAsync?Cd_cidade=" + Cd_cidade + "&Ds_cidade=" + Ds_cidade + "&Uf=" + Uf);
                if (response.IsSuccessStatusCode)
                    return JsonConvert.DeserializeObject<IEnumerable<Cidade>>(await response.Content.ReadAsStringAsync());
                else return null;
            }
            catch { return null; }
        }
        public static async Task<IEnumerable<Cliente>> GetClientesAsync(string cd_clifor = "", 
                                                                        string Nm_clifor = "", 
                                                                        string Ds_cidade = "")
        {
            try
            {
                HttpClient client = new HttpClient();
                HttpResponseMessage response = await client.GetAsync(url_api + "/api/Cliente/GetAsync?cd_clifor=" + cd_clifor.Trim() + "&Nm_clifor=" + Nm_clifor + "&Ds_cidade=" + Ds_cidade);
                if (response.IsSuccessStatusCode)
                    return JsonConvert.DeserializeObject<IEnumerable<Cliente>>(await response.Content.ReadAsStringAsync());
                else return null;
            }
            catch { return null; }
        }

        public static async Task<IEnumerable<Cliente>> GetAuxiliaresAsync()
        {
            try
            {
                HttpClient client = new HttpClient();
                HttpResponseMessage response = await client.GetAsync(url_api + "/api/Cliente/GetAuxiliaresAsync");
                if (response.IsSuccessStatusCode)
                    return JsonConvert.DeserializeObject<IEnumerable<Cliente>>(await response.Content.ReadAsStringAsync());
                else return null;
            }
            catch { return null; }
        }

        public static async Task<bool> GravarClienteAsync(Cliente cliente)
        {
            try
            {
                HttpClient client = new HttpClient();
                HttpResponseMessage response = await client.PostAsync(url_api + "/api/Cliente/GravarAsync",
                    new StringContent(JsonConvert.SerializeObject(cliente), Encoding.UTF8, "application/json"));
                if (response.IsSuccessStatusCode)
                {
                    string s = await response.Content.ReadAsStringAsync();
                    return JsonConvert.DeserializeObject<bool>(s);
                }
                else return false;
            }
            catch { return false; }
        }

        public static async Task<IEnumerable<OrdemServico>> GetOrdemServicosAsync(string Cd_contratante = "",
                                                                                  string Cd_cidadefazenda = "",
                                                                                  string Nm_fazenda = "",
                                                                                  string Dt_ini = "",
                                                                                  string Dt_fin = "")
        {
            try
            {
                HttpClient client = new HttpClient();
                HttpResponseMessage response = await client.GetAsync(url_api + "/api/OrdemServico/GetOSAsync?Cd_piloto=" + App.Cd_piloto + "&Cd_contratante=" + Cd_contratante +
                    "&Cd_cidadeFazenda=" + Cd_cidadefazenda + "&Nm_fazenda=" + Nm_fazenda + "&Dt_ini=" + Dt_ini + "&Dt_fin=" + Dt_fin);
                if (response.IsSuccessStatusCode)
                    return JsonConvert.DeserializeObject<IEnumerable<OrdemServico>>(await response.Content.ReadAsStringAsync());
                else return null;
            }
            catch { return null; }
        }
        public static async Task<bool> GravarOSAsync(OrdemServico os)
        {
            try
            {
                HttpClient client = new HttpClient();
                HttpResponseMessage response = await client.PostAsync(url_api + "/api/OrdemServico/GravarOSAsync",
                    new StringContent(JsonConvert.SerializeObject(os), Encoding.UTF8, "application/json"));
                if (response.IsSuccessStatusCode)
                {
                    string s = await response.Content.ReadAsStringAsync();
                    return JsonConvert.DeserializeObject<bool>(s);
                }
                else return false;
            }
            catch { return false; }
        }
        public static async Task<bool> CancelarOSAsync(OrdemServico os)
        {
            try
            {
                HttpClient client = new HttpClient();
                HttpResponseMessage response = await client.PostAsync(url_api + "/api/OrdemServico/CancelarOSAsync",
                    new StringContent(JsonConvert.SerializeObject(os), Encoding.UTF8, "application/json"));
                if (response.IsSuccessStatusCode)
                    return JsonConvert.DeserializeObject<bool>(await response.Content.ReadAsStringAsync());
                else return false;
            }
            catch { return false; }
        }
        public static async Task<TEndereco_CEPRest> GetCEPAsync(string cep)
        {
            if (Connectivity.NetworkAccess == NetworkAccess.Internet)
                try
                {
                    HttpClient client = new HttpClient();
                    client.BaseAddress = new Uri("https://viacep.com.br");
                    var response = await client.GetAsync("/ws/" + cep.SoNumero() + "/json");
                    if (response.IsSuccessStatusCode)
                        return JsonConvert.DeserializeObject<TEndereco_CEPRest>(await response.Content.ReadAsStringAsync());
                    else return null;
                }
                catch { return null; }
            else return null;
        }
    }
}
