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
    public class UserInsusController : ApiController
    {
        [HttpPut]
        [Route("UserInsus/Insert")]
        [Authorize]
        public async Task<string> Insert(UserInsusModel user)
        {
            string userId = WebApi.GetUserId(User);
            if (userId.Length == 0) return "获取用户失败";
            return await DBOper.UserInsus.Insert(Convert.ToInt32(userId), user.insus);
        }
    }
}
