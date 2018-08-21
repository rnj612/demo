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
    public class BannersController : ApiController
    {
        [HttpGet]
        [Route("Banners/GetList")]
        public async Task<IEnumerable<BannersModel>> GetList()
        {
            return await DBOper.Banners.GetList();
        }
    }
}
