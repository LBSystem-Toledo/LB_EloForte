using LB_EloForteAPI.Models;
using LB_EloForteAPI.Repository;
using System;
using System.Threading.Tasks;
using System.Web.Http;

namespace LB_EloForteAPI.Controllers
{
    public class ServicoController : ApiController
    {
        [HttpGet]
        [ActionName("GetAsync")]
        public async Task<IHttpActionResult> GetAsync()
        {
            try
            {
                return Ok(await new ServicoDAO().GetAsync());
            }
            catch (Exception ex) { return BadRequest(ex.Message.Trim()); }
        }
        [HttpGet]
        [ActionName("GetPrecoAsync")]
        public async Task<IHttpActionResult> GetPrecoAsync(string Cd_servico, string Cd_tabelapreco)
        {
            try
            {
                return Ok(await new ServicoDAO().GetPrecoAsync(Cd_servico, Cd_tabelapreco));
            }
            catch (Exception ex) { return BadRequest(ex.Message.Trim()); }
        }
    }
}