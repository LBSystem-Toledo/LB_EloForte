using LB_EloForteAPI.Repository;
using System;
using System.Threading.Tasks;
using System.Web.Http;

namespace LB_EloForteAPI.Controllers
{
    public class LoginController : ApiController
    {
        [HttpGet]
        [ActionName("ValidarLoginAsync")]
        public async Task<IHttpActionResult> ValidarLoginAsync(string Login, string Senha)
        {
            try
            {
                return Ok(await new LoginDAO().ValidarLoginAsync(Login, Senha));
            }
            catch(Exception ex) { return BadRequest(ex.Message.Trim()); }
        }
    }
}