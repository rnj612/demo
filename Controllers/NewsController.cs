using DBOper;
using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace Api.Controllers
{
    public class NewsController : ApiController
    {
       [HttpGet]
       [Route("News/top")]
        public async Task<IEnumerable<NewsModel>> getNews(string offset="0")
        {
            return await News.getNews(offset);
        }
        [HttpGet]
        [Route("News/info")]
        public async Task<string> getNewsInfo(string path)
        {
            return await News.getNewsInfo(path);
        }
    }
}
