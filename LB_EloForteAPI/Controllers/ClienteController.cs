using LB_EloForteAPI.Models;
using LB_EloForteAPI.Repository;
using System;
using System.Threading.Tasks;
using System.Web.Http;

namespace LB_EloForteAPI.Controllers
{
    public class ClienteController : ApiController
    {
        [HttpGet]
        [ActionName("GetAsync")]
        public async Task<IHttpActionResult> GetAsync(string cd_clifor = "", string Nm_clifor = "", string Ds_cidade = "")
        {
            try
            {
                return Ok(await new ClienteDAO().GetAsync(cd_clifor, Nm_clifor, Ds_cidade));
            }
            catch (Exception ex) { return BadRequest(ex.Message.Trim()); }
        }
        [HttpGet]
        [ActionName("GetAuxiliaresAsync")]
        public async Task<IHttpActionResult> GetAuxiliaresAsync()
        {
            try
            {
                return Ok(await new ClienteDAO().GetAuxiliaresAsync());
            }
            catch (Exception ex) { return BadRequest(ex.Message.Trim()); }
        }
        [HttpPost]
        [ActionName("GravarAsync")]
        public async Task<IHttpActionResult> GravarAsync([FromBody]Cliente cliente)
        {
            try
            {
                return Ok(await new ClienteDAO().GravarAsync(cliente));
            }
            catch (Exception ex) { return BadRequest(ex.Message.Trim()); }
        }
    }
}