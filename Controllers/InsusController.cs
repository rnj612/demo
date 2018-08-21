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
    public class InsusController : ApiController
    {
        [HttpDelete]
        [Route("Insus/Delete")]
        public async Task<string> Delete(InsusModel Insus)
        {
            return await DBOper.Insus.Delete(Insus.id);
        }
        [HttpGet]
        [Route("Insus/GetList")]
        public async Task<IEnumerable<InsusModel>> GetList(string title, string page_size, string page_index)
        {
            return await DBOper.Insus.GetList(title, string.Empty, page_size, page_index);
        }
        [HttpGet]
        [Route("Insus/GetList")]
        public async Task<IEnumerable<InsusModel>> GetList(string title, string order, string page_size, string page_index)
        {
            return await DBOper.Insus.GetList(title, order, page_size, page_index);
        }
        [HttpGet]
        [Route("Insus/GetCount")]
        public async Task<long> GetCount(string title)
        {
            return await DBOper.Insus.GetCount(title);
        }
    }
}
