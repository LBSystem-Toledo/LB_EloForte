using LB_EloForteAPI.Models;
using LB_EloForteAPI.Repository;
using System;
using System.Threading.Tasks;
using System.Web.Http;

namespace LB_EloForteAPI.Controllers
{
    public class OrdemServicoController : ApiController
    {
        [HttpGet]
        [ActionName("GetOSAsync")]
        public async Task<IHttpActionResult> GetOSAsync(string Cd_piloto = "",
                                                        string Cd_contratante = "",
                                                        string Cd_cidadeFazenda = "",
                                                        string Nm_fazenda = "",
                                                        string Dt_ini = "",
                                                        string Dt_fin = "")
        {
            try
            {
                var ret = await new OrdemServicoDAO().GetAsync(Cd_piloto, Cd_contratante, Cd_cidadeFazenda, Nm_fazenda, Dt_ini, Dt_fin);
                return Ok(ret);
            }
            catch (Exception ex) { return BadRequest(ex.Message.Trim()); }
        }
        [HttpPost]
        [ActionName("GravarOSAsync")]
        public async Task<IHttpActionResult> GravarOSAsync([FromBody]OrdemServico os)
        {
            try
            {
                return Ok(await new OrdemServicoDAO().GravarAsync(os));
            }
            catch (Exception ex) { return BadRequest(ex.Message.Trim()); }
        }
        [HttpPost]
        [ActionName("CancelarOSAsync")]
        public async Task<IHttpActionResult> CancelarOSAsync([FromBody]OrdemServico os)
        {
            try
            {
                return Ok(await new OrdemServicoDAO().CancelarAsync(os));
            }
            catch (Exception ex) { return BadRequest(ex.Message.Trim()); }
        }
    }
}