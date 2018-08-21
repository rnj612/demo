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
    public class BankSevController : ApiController
    {
        [HttpDelete]
        [Route("BankSev/Delete")]
        public async Task<string> Delete(BankSevModel BankSev)
        {
            if (!await DBOper.SysUser.GetToken(BankSev.access_id, BankSev.access_token)) return "请重新登录";
            return await DBOper.BankSev.Delete(BankSev.id);
        }
        [HttpGet]
        [Route("BankSev/GetList")]
        public async Task<IEnumerable<BankSevModel>> GetList()
        {
            return await DBOper.BankSev.GetList();
        }
    }
}
