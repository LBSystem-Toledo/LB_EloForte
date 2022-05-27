using LB_EloForteAPI.Repository;
using System;
using System.Threading.Tasks;
using System.Web.Http;

namespace LB_EloForteAPI.Controllers
{
    public class CidadeController : ApiController
    {
        [HttpGet]
        [ActionName("GetAsync")]
        public async Task<IHttpActionResult> GetAsync(string Cd_cidade = "", 
                                                      string Ds_cidade = "", 
                                                      string Uf = "")
        {
            try
            {
                return Ok(await new CidadeDAO().GetAsync(Cd_cidade, Ds_cidade, Uf));
            }
            catch (Exception ex) { return BadRequest(ex.Message.Trim()); }
        }
    }
}