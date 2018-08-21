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
    public class LoansController : ApiController
    {
        [HttpDelete]
        [Route("Loans/Delete")]
        public async Task<string> Delete(LoansModel Loans)
        {
            return await DBOper.Loans.Delete(Loans.id);
        }
        [HttpGet]
        [Route("Loans/GetList")]
        public async Task<IEnumerable<LoansModel>> GetList(string title, string type_id, string debt,
            string time, string page_size, string page_index)
        {
            return await DBOper.Loans.GetList(title, type_id, debt, time, string.Empty, page_size, page_index);
        }
        [HttpGet]
        [Route("Loans/GetList")]
        public async Task<IEnumerable<LoansModel>> GetList(string title, string type_id, string debt,
            string time, string order, string page_size, string page_index)
        {
            return await DBOper.Loans.GetList(title, type_id, debt, time, order, page_size, page_index);
        }
        [HttpGet]
        [Route("Loans/GetCount")]
        public async Task<long> GetCount(string title, string type_id, string debt, string time)
        {
            return await DBOper.Loans.GetCount(title, type_id, debt, time);
        }
    }
}
