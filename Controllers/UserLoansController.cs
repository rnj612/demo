using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using Util;

namespace Api.Controllers
{
    public class UserLoansController : ApiController
    {
        [HttpPut]
        [Route("UserLoans/Insert")]
        [Authorize]
        public async Task<string> Insert(UserLoansModel user)
        {
            string userId = WebApi.GetUserId(User);
            if (userId.Length == 0) return "获取用户失败";
            return await DBOper.UserLoans.Insert(Convert.ToInt32(userId), user.loans);
        }
    }
}
