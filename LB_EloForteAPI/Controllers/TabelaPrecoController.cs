using LB_EloForteAPI.Repository;
using System;
using System.Threading.Tasks;
using System.Web.Http;

namespace LB_EloForteAPI.Controllers
{
    public class TabelaPrecoController : ApiController
    {
        [HttpGet]
        [ActionName("GetAsync")]
        public async Task<IHttpActionResult> GetAsync()
        {
            try
            {
                return Ok(await new TabelaPrecoDAO().GetAsync());
            }
            catch (Exception ex) { return BadRequest(ex.Message.Trim()); }
        }
    }
}